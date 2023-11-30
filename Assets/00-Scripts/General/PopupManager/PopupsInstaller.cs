using BallsToCup.General.Popups;
using UnityEngine;
using Zenject;

public class PopupsInstaller : Installer<PopupsInstaller>
{
   #region Methods

   public override void InstallBindings()
   {
      BindFactories();
      BindEventControllers();
   }

   private void BindEventControllers()
   {
      Container.BindInterfacesAndSelfTo<LoadingPanelEventController>().AsTransient();
      Container.BindInterfacesAndSelfTo<MessageBoxPanelEventController>().AsTransient();
      Container.BindInterfacesAndSelfTo<GameResultPanelEventController>().AsTransient();
   }

   private void BindFactories()
   {
      Container.BindFactory<IPopupView, LoadingPanelLogic, LoadingPanelLogic.Factory>();
      Container.BindFactory<IPopupView, MessageBoxPanelLogic, MessageBoxPanelLogic.Factory>();
      Container.BindFactory<IPopupView, GameResultPanelLogic, GameResultPanelLogic.Factory>();
      
   }

   #endregion
}