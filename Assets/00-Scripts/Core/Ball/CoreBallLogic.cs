using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.General;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class CoreBallLogic : IDisposable,IInitializable,IEventListener
    {
        #region Fields

        private readonly CoreBallView _view; 

        #endregion

        #region Constructors

        public CoreBallLogic(CoreBallView view)
        {
            _view = view;
        }


        #endregion
        #region Methods

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            RegisterToEvents();
        }
        public void RegisterToEvents()
        {
         
        }

        public void UnregisterFromEvents()
        {

        }
        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<CoreBallView, CoreBallLogic>
        {
            
        }


        #endregion
 
    }
}

