using System;
using BallsToCup.Core;
using BallsToCup.Core.Gameplay;
using BallsToCup.General;
using UnityEngine;
using Zenject;

public class TubeLogic : IDisposable
{
  #region Fields

  [Inject] private TubeEventController _eventController;
  [Inject] private IDraggable _draggable;
  [Inject] private LevelManagerEventController _levelManagerEventController;
  [Inject] private FlowControllerEventController _flowControllerEventController;
  [Inject] private GameManagerEventController _gameManagerEventController;
  [Inject] private BallGeneratorModel _ballGeneratorModel;
  private readonly TubeView _view;
  private Camera _camera;
  private Vector2 _currentPointerPos;
  private Vector2 _deltaPointerPos;
  private Vector2 _pivotPointOnScreen;
  private float _moveThreshold;
  private float _maxRotFactor;
  private bool _inputEnable;
  private float _maxDistance;
  private float _lastTime;
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
    _view.SetEventController(_eventController).SetMaxRotVelocity(_ballGeneratorModel.maxTubeRotVelocity);
  }

  private void GetRotationInfo()
  {
    var info=_levelManagerEventController.onCurrentLevelRotateControllInfoRequest.GetFirstResult();
    if(info==default)
      return;
    _moveThreshold = info.moveThreshold;
    _maxRotFactor = info.maxSensitivity;
  }

  public void Dispose()
  {
    UnregisterFromEvent();
    GC.SuppressFinalize(this);
  }

  private void RegisterToEvents()
  {
    _eventController.onPivotTransformRequest.Add(OnPivotTransformRequest);
    _eventController.onBallTriggerEdge.Add(OnBallTriggerEdge);
    _flowControllerEventController.onEnableInput.Add(OnInputEnable);
    if (_draggable == default)
      return;
    _draggable.onDrag += OnDrag;
    _draggable.onDragDelta += OnDragDelta;
    _draggable.onDragBegin += OnDragBegin;
  }

  private void UnregisterFromEvent()
  {
    _eventController.onPivotTransformRequest.Remove(OnPivotTransformRequest);
    _eventController.onBallTriggerEdge.Remove(OnBallTriggerEdge);
    _flowControllerEventController.onEnableInput.Remove(OnInputEnable);
    if (_draggable == default)
      return;
    _draggable.onDrag -= OnDrag;
    _draggable.onDragDelta -= OnDragDelta;
  }

  private void OnBallTriggerEdge(bool isGettingOut)
  {
    _gameManagerEventController.onBallGotOutOfTube.Trigger(isGettingOut);
  }

  private void OnInputEnable(bool enable)
  {
    _inputEnable = enable;
    ClacMaxPossibleDistanceInScreen();
  }

  private Transform OnTubePivotRequest() => _view.tubePivot;
  private Vector3 OnPivotTransformRequest() => _view.tubePivot.position;


  void GetPivotPosOnScreen()
  {
    _pivotPointOnScreen = _camera.WorldToScreenPoint(_view.tubePivot.position);

  }

  private void OnDragBegin(Vector2 pointerPos)
  {
    _lastTime = Time.time;
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
    var rotationFactor = CalcFactorBasedOnDistanceToTubePivot();
    var deltaTime = Time.time - _lastTime;
    
    _lastTime = Time.time;
    var deltaAngle = (currentAngle - lastAngle) * rotationFactor*180.0f / Mathf.PI;
    var velocity = deltaAngle / deltaTime;
    if (velocity*velocity > _ballGeneratorModel.maxTubeRotVelocity*_ballGeneratorModel.maxTubeRotVelocity)
    {
      var sign = velocity > 0 ? 1 : -1;
      velocity = _ballGeneratorModel.maxTubeRotVelocity*sign;
        
      deltaAngle = velocity * deltaTime;
    }

  

    // _view.tubePivot.Rotate(Vector3.forward,deltaAngle );
    _view.AddToDeltaAngle(deltaAngle);
  }

  float CalcFactorBasedOnDistanceToTubePivot()
  {
    var distance = Vector2.Distance(_currentPointerPos, _pivotPointOnScreen);
    // if(distance>_maxDistance)
    // BtcLogger.Log($"distance:{distance}");
    return _maxRotFactor / _maxDistance *distance ;
  }

  private void ClacMaxPossibleDistanceInScreen()
  {
    var topDistance =Vector2.Distance( _pivotPointOnScreen , Vector2.zero);
    var bottomDistance =Vector2.Distance( _pivotPointOnScreen , new Vector2(Screen.width, Screen.height));
    _maxDistance= Mathf.Max(topDistance, bottomDistance);

    
  }

  private float CalcAngleWithXAxis(Vector2 pos)
  {
    var angle = Mathf.Atan(pos.y / pos.x);
    return pos switch
    {
      { y: > 0, x: > 0 } => angle,
      { y: < 0, x: > 0 } => angle+Mathf.PI*2.0f,
      { y: < 0, x: < 0 } => angle + Mathf.PI,
      { y: > 0, x: < 0 } => angle - Mathf.PI+Mathf.PI*2.0f,
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
