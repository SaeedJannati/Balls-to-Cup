using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallsToCup.Meta.UI
{
    public class MetaSelectLevelLevelView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _starsGroup;
        [SerializeField] private GameObject _lastLevelIndicator;
        [SerializeField] private GameObject _playButton;
        [field: SerializeField] public List<GameObject> _starsFore { get; private set; }
        [SerializeField] private TMP_Text _text_level;
        [field: SerializeField] public Transform _backTransform;
        private int _levelIndex;
        private Action<int> _onPlayClickAction;
        #endregion

        #region Methods

        public MetaSelectLevelLevelView SetOnPlayAction(Action<int> action)
        {
            _onPlayClickAction = action;
            return this;
        }

        public MetaSelectLevelLevelView SetLevel(int level)
        {
            _levelIndex = level;
            _text_level.text = level.ToString();
            return this;
        }

        public MetaSelectLevelLevelView SetPlayButtonActive(bool enable)
        {
            _playButton.SetActive(enable);
            return this;
        }

        public MetaSelectLevelLevelView SetStarsGroupActive(bool enable)
        {
            _starsGroup.SetActive(enable);
            return this;
        }

        public MetaSelectLevelLevelView SetAsCurrentLevel(bool isCurrentLevel)
        {
            _lastLevelIndicator.SetActive(isCurrentLevel);
            return this;
        }

        public MetaSelectLevelLevelView SetStarCount(int starCount)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < starCount)
                {
                    _starsFore[i].SetActive(true);
                    continue;
                }
                _starsFore[i].SetActive(false);
            }

            return this;
        }

        public void OnPlayClick()
        {
            _onPlayClickAction?.Invoke(_levelIndex);
        }

        #endregion
    }
}

