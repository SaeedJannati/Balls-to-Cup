using System.Collections;
using System.Collections.Generic;
using BallsToCup.General.Popups;
using UnityEngine;
namespace BallsToCup.General.Popups
{
    public class LoadingPanelView:MonoBehaviour,IPopupView
    {
        #region Fields

        private LoadingPanelEventController _eventController;

        #endregion

        #region Unity events

        private void OnDestroy()
        {
            _eventController.onDispose.Trigger();
        }

        #endregion
        #region Properties

        [field: SerializeField] public CanvasGroup canvasGroup { get; private set; }=default;


        #endregion

        #region Methods

        public LoadingPanelView SetEventController(LoadingPanelEventController eventController)
        {
            _eventController = eventController;
            return this;
        }


        #endregion
    }
}
