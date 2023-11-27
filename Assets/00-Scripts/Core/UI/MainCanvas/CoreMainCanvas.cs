using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.Core.Gameplay;
using BallsToCup.General;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core.UI
{
    public class CoreMainCanvas : MonoBehaviour,IEventListener
    {
        #region Fields
        [Inject] private GameManagerEventController _gameManagerEventController;
        [SerializeField] private TMP_Text _text_totallBalls;
        [SerializeField] private TMP_Text _text_ballsInCup;
        [SerializeField] private CorePauseMenu _pauseMenu;
        #endregion

        #region Unity actions

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
            _gameManagerEventController.onTotalBallsChange.Add(OnTotalBallsChange);
            _gameManagerEventController.onBallsInCupChange.Add(OnBallsInCupChange);
        }

        public void UnregisterFromEvents()
        {
            _gameManagerEventController.onTotalBallsChange.Remove(OnTotalBallsChange);
            _gameManagerEventController.onBallsInCupChange.Remove(OnBallsInCupChange);
        }

        private void OnBallsInCupChange(int count)
        {
            _text_ballsInCup.text = count.ToString();
        }

        private void OnTotalBallsChange(int count)
        {
            _text_totallBalls.text = count.ToString();
        }

        public void OnPauseMenuClick()
        {
            _pauseMenu.BringUp();
        }

        public void OnFinishClick()
        {
            _gameManagerEventController.onGameEnd.Trigger();
        }

        #endregion

      
    }
}

