using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.Core;
using BallsToCup.Core.Gameplay;
using BallsToCup.General;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace BallsToCup.Core
{
    public class TubeView : MonoBehaviour, IEventListener
    {
        #region Fields

        [field: SerializeField] public Transform tubePivot { get; private set; }
        [SerializeField] private TubeEdgeHandler _edgeHandler;
        private TubeEventController _eventController;

        #endregion

        #region Unity actions

        private void Start()
        {
            RegisterToEvents();
        }

        private void OnDestroy()
        {
            UnregisterFromEvents();
        }

        #endregion

        #region Methods

        public TubeView SetEventController(TubeEventController eventController)
        {
            _eventController = eventController;
            return this;
        }

        public void RegisterToEvents()
        {
            _edgeHandler.onBallTrigger.Add(OnBallTrigger);
        }

        public void UnregisterFromEvents()
        {
            _edgeHandler.onBallTrigger.Remove(OnBallTrigger);
        }

        private void OnBallTrigger(bool isGettingOut)
        {
            _eventController.onBallTriggerEdge.Trigger(isGettingOut);
        }

        #endregion
    }
}