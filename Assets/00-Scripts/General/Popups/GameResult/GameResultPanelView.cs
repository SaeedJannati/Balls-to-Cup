using UnityEngine;

namespace BallsToCup.General.Popups
{
    public class GameResultPanelView: MonoBehaviour, IPopupView
    {
        #region Fields
        [SerializeField] private CanvasGroup _canvasGroup;
        private GameResultPanelEventController _eventController;

        #endregion

        #region Properties

        public CanvasGroup canvasGroup => _canvasGroup;

        #endregion
        #region Monobehaviour callbacks

        private void OnDestroy()
        {
            _eventController.onDispose.Trigger();
        }

        #endregion
        #region Methods

        public GameResultPanelView SetEventController(GameResultPanelEventController eventController)
        {
            _eventController = eventController;
            return this;
        }
        #endregion
    }
}