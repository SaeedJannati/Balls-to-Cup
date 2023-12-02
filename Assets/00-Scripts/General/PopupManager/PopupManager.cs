using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BallsToCup.General.Popups
{
    public class PopupManager
    {
        #region Fields

        [Inject] private PopupManagerModel _model;
        [Inject] private AddressableLoader _addressableLoader;
        public Action<(PopupName popupName, IPopupLogic logic)> onPopupCreated { get; set; }
        private LoadingPanelLogic _loadingPanel;

        private bool _showingLoading;

        #region Factories

        [Inject] private LoadingPanelLogic.Factory _loadingPanelFactory;
        [Inject] private MessageBoxPanelLogic.Factory _messageBoxFactory;
        [Inject] private GameResultPanelLogic.Factory _gameResultFactory;
        #endregion
        #endregion
        #region Methods

        public async Task ShowLoading()
        {
            _showingLoading = true;
            _loadingPanel ??= (LoadingPanelLogic)await RequestPopup(PopupName.Loading);
            if (_loadingPanel._hasDisposed)
            {
                _loadingPanel = (LoadingPanelLogic)await RequestPopup(PopupName.Loading);
            }

            _loadingPanel.Show();
            await Task.Yield();
            _showingLoading = false;
        }

        public async void HideLoading()
        {
            while (_showingLoading)
            {
                await Task.Yield();
            }

            _loadingPanel?.Hide();
        }

        public async Task<MessageBoxPanelLogic> RequestMessageBox()
        {
            return (MessageBoxPanelLogic)await RequestPopup(PopupName.MessageBox);
        }

        public async Task<IPopupLogic> RequestPopup(PopupName popupName)
        {
            Resources.UnloadUnusedAssets();
            var panelInfo = await CreatePopupView(popupName);
            if (panelInfo == default)
                throw new Exception($"No view found for popup :{popupName.ToString()}");
            if(!Application.isPlaying)
                throw new Exception($"Not in the play mode atm.");
            return TryCreateLogic(popupName, panelInfo);
        }

        IPopupLogic TryCreateLogic(PopupName popupName, (IPopupView view, GameObject panel) panelInfo)
        {
            try
            {
                var logic = CreateLogic(popupName, panelInfo.view);
                onPopupCreated?.Invoke((popupName, logic));
                return logic;
            }
            catch (Exception e)
            {
                GameObject.Destroy(panelInfo.panel);
                BtcLogger.Log(e.Message, "red");
                return default;
            }
        }

        async Task<(IPopupView view, GameObject panel)> CreatePopupView(PopupName popupName)
        {
            var info = _model.popUpInfos.FirstOrDefault(i => i.popupName == popupName);
            if (info == default)
                return default;
            var req = Resources.LoadAsync(info.path);
            while (!req.isDone)
            {
                await Task.Yield();
            }
            if(!Application.isPlaying)
                throw new Exception($"Not in the play mode atm.");
            if (req.asset == default)
                return default;
            var panel = GameObject.Instantiate(req.asset) as GameObject;
            if (panel == default)
                return default;
            if (!panel.TryGetComponent(out IPopupView view))
            {
                GameObject.Destroy(panel);
                return default;
            }

            return (view, panel);
        }

        IPopupLogic CreateLogic(PopupName popupName, IPopupView view)
        {
            return popupName switch
            {
                PopupName.Loading => _loadingPanelFactory.Create(view),
                PopupName.MessageBox=>_messageBoxFactory.Create(view),
                PopupName.GameResult=>_gameResultFactory.Create(view),
                _ => throw new Exception(
                    $"Use specific method for creating this type of popup,Type:{popupName.ToString()}"),
            };
        }



        #endregion
    }
}