using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BallsToCup.General;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace BallsToCup.Meta.UI
{
    public class MetaSelectLevelPanel : MonoBehaviour
    {
        #region Fields

        [Inject] private AddressableLoader _addressable;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PlayerProgressManager _progressManager;
        [SerializeField,Expandable] private MetaSelectLevelPanelModel _model;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _levelsViewsParent;
        private bool _isClosing;
        private List<MetaSelectLevelLevelView> _leveViews = new();
        #endregion

        #region Methods

        public void BringUp()
        {
            gameObject.SetActive(true);
            FadePanel(true,PopulateLevels);
        }

        public void BringDown()
        {
            FadePanel(false, () =>
            {
                _isClosing = false;
                gameObject.SetActive(false);
            });

        }

        void FadePanel(bool fadeIn, Action onComplete = default)
        {
            var initAlpha = fadeIn ? 0.0f : 1.0f;
            var destAlpha = fadeIn ? 1.0f : 0.0f;
            _canvasGroup.alpha = initAlpha;
            _canvasGroup
                .DOFade(destAlpha, _model.fadePeriod)
                .SetEase(Ease.InOutSine)
                .onComplete = () => { onComplete?.Invoke(); };
        }

        public void OnCloseClick()
        {
            if (_isClosing)
                return;
            _isClosing = true;
            BringDown();
        }

        private async void PopulateLevels()
        {
            _levelsViewsParent.ClearChildren();
            _leveViews = new();
            var levelViewReference = await _addressable.LoadAssetReference(_model.levelViewReference);
            if(!levelViewReference.TryGetComponent(out MetaSelectLevelLevelView levelView))
                return;
            var levelsData = _progressManager.GetLevelsProgress();
            var currentLevel = levelsData.LastOrDefault(i => i.hasReached)?.index??0;
            MetaSelectLevelLevelView view;
            for (int i = 0,e=levelsData.Count; i < e; i++)
            {
                view = Instantiate(levelView, _levelsViewsParent)
                    .SetLevel(i)
                    .SetOnPlayAction(OnLevelSelect)
                    .SetStarsGroupActive(levelsData[i].hasReached)
                    .SetPlayButtonActive(levelsData[i].hasReached)
                    .SetStarCount(levelsData[i].starsCount);
                if (i == currentLevel)
                {
                    view.SetAsCurrentLevel(true);
                    continue;
                }
                view.SetAsCurrentLevel(false);
                view._backTransform.localScale=Vector3.zero;
                _leveViews.Add(view);
            }

            StartCoroutine(ShowLevelsWithEffect());

        }

        IEnumerator ShowLevelsWithEffect()
        {
            var delay = new WaitForSeconds(_model.levelItemGenerationDelayInBetween);
            for (int i = 0,e=_leveViews.Count; i < e; i++)
            {
                _leveViews[i]._backTransform.DOScale(1.0f, _model.levelItemScalePeriod).SetEase(Ease.OutBounce);
                yield return delay;
            }
        }

        void OnLevelSelect(int levelIndex)
        {
            _progressManager.OnSelectLevel(levelIndex);
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(1);});
        }

        #endregion
    }
}