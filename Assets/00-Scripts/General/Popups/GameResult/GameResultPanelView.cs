using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BallsToCup.General.Popups
{
    public class GameResultPanelView: MonoBehaviour, IPopupView
    {
        #region Fields
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text_title;
        [SerializeField] private TMP_Text _text_message;
        [field: SerializeField] public List<Image> starsForegrounds { get; private set; }
        [SerializeField] private GameObject _nextButton;
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

        public GameResultPanelView SetTitle(string title)
        {
            _text_title.text = title;
            return this;
        }

        public GameResultPanelView SetMessage(string title)
        {
            _text_message.text = title;
            return this;
        }

        public GameResultPanelView SetNextButtonEnable(bool enable)
        {
            _nextButton.SetActive(enable);
            return this;
        }

        public void OnHomeClick()
        {
            _eventController.onHomeClick.Trigger();
        }

        public void OnRetryClick()
        {
            _eventController.onRetryClick.Trigger();
        }

        public void OnNextLevelClick()
        {
            _eventController.onNextLevelClick.Trigger();
        }

        #endregion
    }
}