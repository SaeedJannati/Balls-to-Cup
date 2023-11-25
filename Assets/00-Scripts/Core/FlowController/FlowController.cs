using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace BallsToCup.Core.Gameplay
{
    public class FlowController : IInitializable, IDisposable
    {
        #region Fields

        [Inject] private FlowControllerEventController _eventController;

        #endregion

        #region Methods

        public void Dispose()
        {
            _eventController.Dispose();
        }

        public void Initialize()
        {
        }

        #endregion
    }
}