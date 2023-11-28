using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BallsToCup.General;
using BallsToCupGeneral.Audio;
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
        [SerializeField] private AudioPlayer _clickAudio;
        [SerializeField] private AudioPlayer _spawnLevelAudio;
        private bool _isClosing;
        private List<MetaSelectLevelLevelView> _levelsViews = new();
        #endregion

        #region Unity actions

        private void OnDisable()
        {
            ClearLevelViews();
        }

        #endregion
        #region Methods

        void ClearLevelViews()
        {
            _levelsViewsParent.ClearChildren();
            _levelsViews = new();
        }
        

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
            _clickAudio.Play();
            if (_isClosing)
                return;
            _isClosing = true;
            BringDown();
        }

        private async void PopulateLevels()
        {
            ClearLevelViews();
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
                view._backTransform.localScale=Vector3.zero;
                _levelsViews.Add(view);
                if (i == currentLevel)
                {
                    view.SetAsCurrentLevel(true);
                    continue;
                }
                view.SetAsCurrentLevel(false);
            }

            StartCoroutine(ShowLevelsWithEffect());

        }

        IEnumerator ShowLevelsWithEffect()
        {
            var delay = new WaitForSeconds(_model.levelItemGenerationDelayInBetween);
            yield return delay;
            for (int i = 0,e=_levelsViews.Count; i < e; i++)
            {
                _spawnLevelAudio.Play();
                _levelsViews[i]._backTransform.DOScale(1.0f, _model.levelItemScalePeriod).SetEase(Ease.OutBounce);
                yield return delay;
            }
        }

        void OnLevelSelect(int levelIndex)
        {
            _clickAudio.Play();
            _progressManager.OnSelectLevel(levelIndex);
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(1);});
        }

        #endregion
    }
}