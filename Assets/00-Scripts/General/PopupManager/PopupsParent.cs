using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BallsToCup.General.Popups
{
    public class PopupsParent : MonoBehaviour, IEventListener
    {
        #region Fields

        [Inject] private PopupManager _popupManager;

        //for test purposes 
        [SerializeField] private PopupName _popupName;
        #endregion

        #region Monobehaviour callbacks

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

        public void RegisterToEvents()
        {
            _popupManager.onPopupCreated += OnPopupCreated;
        }

        private void OnPopupCreated((PopupName popupName, IPopupLogic logic) info)
        {
            info.logic.GetPanelObject().transform.SetParent(transform);
        }

        public void UnregisterFromEvents()
        {
            _popupManager.onPopupCreated -= OnPopupCreated;
        }
        [Button]
       private async void RequestPopup()
        {
          await  _popupManager.RequestPopup(
                _popupName);
        }
         #endregion
    }
}