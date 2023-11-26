using System;
using BallsToCup.Core;
using BallsToCup.Core.Gameplay;
using UnityEngine;
using Zenject;

public class TubeLogic : IDisposable
{
  #region Fields

  [Inject] private TubeEventController _eventController;
  [Inject] private IDraggable _draggable;
  [Inject] private LevelManagerEventController _levelManagerEventController;
  [Inject] private FlowControllerEventController _flowControllerEventController;
  private readonly TubeView _view;
  private Camera _camera;
  private Vector2 _currentPointerPos;
  private Vector2 _deltaPointerPos;
  private Vector2 _pivotPointOnScreen;
  private float _moveThreshold;
  private float _rotationSpeed;
  private bool _inputEnable;
  #endregion

  #region Constructor

  public TubeLogic(TubeView view)
  {
    _view = view;
  }


  #endregion

  #region Methods

  [Inject]
  private void Initialise()
  {
    _camera = Camera.main;
    InitialiseView();
    RegisterToEvents();
    GetPivotPosOnScreen();
    GetRotationInfo();
  }

  private void InitialiseView()
  {
    _view.SetEventController(_eventController);
  }

  private void GetRotationInfo()
  {
    var info=_levelManagerEventController.onCurrentLevelRotateControllInfoRequest.GetFirstResult();
    if(info==default)
      return;
    _moveThreshold = info.moveThreshold;
    _rotationSpeed = info.sensitivityVelocity;
  }

  public void Dispose()
  {
    UnregisterFromEvent();
    GC.SuppressFinalize(this);
  }

  private void RegisterToEvents()
  {
    _eventController.onPivotTransformRequest.Add(OnPivotTransformRequest);
    _flowControllerEventController.onEnableInput.Add(OnInputEnable);
    if (_draggable == default)
      return;
    _draggable.onDrag += OnDrag;
    _draggable.onDragDelta += OnDragDelta;
  }

  private void UnregisterFromEvent()
  {
    _eventController.onPivotTransformRequest.Remove(OnPivotTransformRequest);
    _flowControllerEventController.onEnableInput.Remove(OnInputEnable);
    if (_draggable == default)
      return;
    _draggable.onDrag -= OnDrag;
    _draggable.onDragDelta -= OnDragDelta;
  }

  private void OnInputEnable(bool enable)=> _inputEnable = enable;

  private Vector3 OnPivotTransformRequest() => _view.tubePivot.position;


  void GetPivotPosOnScreen()
  {
    _pivotPointOnScreen = _camera.WorldToScreenPoint(_view.tubePivot.position);

  }
  
  private void OnDrag(Vector2 pointerPos)
  {
    if(!_inputEnable)
      return;
    _currentPointerPos = pointerPos;
  }

  private void OnDragDelta(Vector2 pointerDeltaPos)
  {
    if(!_inputEnable)
      return;
    _deltaPointerPos = pointerDeltaPos;
    CheckForRotation();
  }

  private void CheckForRotation()
  {
    if (_deltaPointerPos.sqrMagnitude <_moveThreshold*_moveThreshold)
      return;
    var currentPos = _currentPointerPos - _pivotPointOnScreen;
    var lastPos = _currentPointerPos - _deltaPointerPos - _pivotPointOnScreen;
    var currentAngle = CalcAngleWithXAxis(currentPos);
    var lastAngle = CalcAngleWithXAxis(lastPos);
    var deltaAngle = (currentAngle - lastAngle) * _rotationSpeed;
   _view.tubePivot.Rotate(Vector3.forward, deltaAngle * 180.0f / Mathf.PI);
  }

 

  private float CalcAngleWithXAxis(Vector2 pos)
  {
    var angle = Mathf.Atan(pos.y / pos.x);
    return pos switch
    {
      { y: > 0, x: > 0 } => angle,
      { y: < 0, x: > 0 } => angle,
      { y: < 0, x: < 0 } => angle + Mathf.PI,
      { y: > 0, x: < 0 } => angle - Mathf.PI,
      _ => angle
    };
  }
  #endregion

  #region Factory

  public class Factory : PlaceholderFactory<TubeView, TubeLogic>
  {
  }


  #endregion


}
