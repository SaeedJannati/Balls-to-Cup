using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BallsToCup.General.Popups
{
    public class MessageBoxPanelView : MonoBehaviour, IPopupView
    {
        #region Fields

        [SerializeField] private Button _button_first;
        [SerializeField] private Button _button_second;
        [SerializeField] private TMP_Text _text_firstButton;
        [SerializeField] private TMP_Text _text_secondButton;
        [SerializeField] private TMP_Text _text_title;
        [SerializeField] private TMP_Text _text_message;
        [SerializeField] private CanvasGroup _canvasGroup;
        private MessageBoxPanelEventController _eventController;

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

        public MessageBoxPanelView SetEventController(MessageBoxPanelEventController eventController)
        {
            _eventController = eventController;
            return this;
        }


        public void OnFirstButtonClick()
        {
            _eventController?.onFirstButtonClick.Trigger();
        }

        public void OnSecondButtonClicked()
        {
            _eventController?.onSecondButtonClick.Trigger();
        }



        public void SetTitle(string title)
        {
            _text_title.text = title;
        }

        public void SetMessage(string message)
        {
            _text_message.text = message;
        }

        public void SetFirstButtonText(string value)
        {
            _text_firstButton.text = value;
        }

        public void SetSecondButtonText(string value)
        {
            _text_secondButton.text = value;
        }


        public void EnableSecondButton(bool enable)
        {
            _button_second.gameObject.SetActive(enable);
        }

        #endregion
    }
}