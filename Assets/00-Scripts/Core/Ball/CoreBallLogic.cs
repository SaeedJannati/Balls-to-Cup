using System;
using BallsToCup.General;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class CoreBallLogic : IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GameManagerEventController _gameManagerEventController;
        [Inject] private YCriterion _yCriterion;
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

        [Inject]
        public void Initialise()
        {
            RegisterToEvents();
            _gameManagerEventController.onBallCreated.Trigger();
            _view
                .SetGoBelowYCriterionAction(OnGoBelowYCriterion)
                .SetYCriterion(_yCriterion.GetYCriterion);
        }

        public void RegisterToEvents()
        {
        }

        public void UnregisterFromEvents()
        {
        }

        void OnGoBelowYCriterion()
        {
            _gameManagerEventController.onBallGoBelowYCriterion.Trigger();
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<CoreBallView, CoreBallLogic>
        {
        }

        #endregion
    }
}