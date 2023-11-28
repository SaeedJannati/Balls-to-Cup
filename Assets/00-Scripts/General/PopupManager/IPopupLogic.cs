using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace BallsToCup.General.Popups
{
    public interface IPopupLogic
    {
        GameObject GetPanelObject();
        void Close();

        private async Task Fade(CanvasGroup canvasGroup, bool fadeIn, float period)
        {
            var initAlpha = fadeIn ? 0.0f : 1.0f;
            var destAlpha = fadeIn ? 1.0f : 0.0f;
            canvasGroup.alpha = initAlpha;
            canvasGroup.DOFade(destAlpha, period);
            await Task.Delay((int)(1000 * period));
        }

//default enter and exit animations override them for different popups if you want different animations such as scale or etc.
        public async void OnEnter(CanvasGroup canvasGroup, float period = .3f, Action onComplete = default)
        {
            await Fade(canvasGroup, true, period);
            canvasGroup.alpha = 1.0f;
            onComplete?.Invoke();
        }

        public async void OnExit(CanvasGroup canvasGroup, float period = .3f, Action onComplete = default)
        {
            await Fade(canvasGroup, false, period);
            canvasGroup.alpha = 0.0f;
            onComplete?.Invoke();
        }
    }
}