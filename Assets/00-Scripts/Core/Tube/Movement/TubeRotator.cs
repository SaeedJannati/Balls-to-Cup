using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.Core;
using BallsToCup.General;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core
{
    public class TubeRotator : MonoBehaviour
{
    #region Fields

    [Inject] private IDraggable _draggable;
    [Inject] private TubeRotatorModel _model;
    [SerializeField] private Transform _tubePivot;
    private Camera _camera;
    private Vector2 _currentPointerPos;
    private Vector2 _deltaPointerPos;
    private Vector2 _pivotPointOnScreen;
    #endregion

    #region Unity actions

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        Initialise();
    }

    private void OnDestroy()
    {
        UnregisterFromEvent();
    }

    #endregion
    #region Methods



    private void Initialise()
    {
        RegisterToEvents();
        GetPivotPosOnScreen();
    }

    [Button]
    void GetPivotPosOnScreen()
    {
        _pivotPointOnScreen= _camera.WorldToScreenPoint(_tubePivot.position);;
    }

    private void OnDrag(Vector2 pointerPos)
    {
        _currentPointerPos = pointerPos;
    }

    private void OnDragDelta(Vector2 pointerDeltaPos)
    {
        _deltaPointerPos = pointerDeltaPos;
        CheckForRotation();
    }

    private void CheckForRotation()
    {
        if(_deltaPointerPos.sqrMagnitude<_model.deltaPointerPosTreshhold*_model.deltaPointerPosTreshhold)
            return;
        var currentPos = _currentPointerPos - _pivotPointOnScreen;
        var lastPos = _currentPointerPos - _deltaPointerPos - _pivotPointOnScreen;
        var currentAngle = CalcAngleWithXAxis(currentPos);
        var lastAngle=CalcAngleWithXAxis(lastPos);
        var deltaAngle = (currentAngle - lastAngle) * _model.rotationSpeed;
        _tubePivot.Rotate(Vector3.forward,deltaAngle*180.0f/Mathf.PI);
    }

    private void RegisterToEvents()
    {
        if(_draggable==default)
            return;
        _draggable.onDrag += OnDrag;
        _draggable.onDragDelta += OnDragDelta;
    }

    private void UnregisterFromEvent()
    {
        if(_draggable==default)
            return;
        _draggable.onDrag -= OnDrag;
        _draggable.onDragDelta -= OnDragDelta;
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
   
}
}

