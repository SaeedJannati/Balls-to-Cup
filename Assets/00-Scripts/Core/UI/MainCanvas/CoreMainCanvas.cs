using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.Core.Gameplay;
using BallsToCup.General;
using BallsToCupGeneral.Audio;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core.UI
{
    public class CoreMainCanvas : MonoBehaviour, IEventListener
    {
        #region Fields

        [Inject] private GameManagerEventController _gameManagerEventController;
        [Inject] private TubeEventController _tubeEventController;
        [SerializeField] private TMP_Text _text_totallBalls;
        [SerializeField] private TMP_Text _text_ballsInCup;
        [SerializeField] private CorePauseMenu _pauseMenu;
        [SerializeField] private AudioPlayer _clickAudioPalyer;
        [SerializeField] private AudioPlayer _ballGetInsideCupAudio;
        [SerializeField] private AudioPlayer    _ballsCreationWave;
        [SerializeField] private AudioPlayer _winAudioPlayer;
        [SerializeField] private AudioPlayer _loseAudioPlayer;
        [SerializeField] private AudioPlayer _tubeDragAudioPlayer;
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
            _gameManagerEventController.onGameWon.Add(OnGameWon);
            _gameManagerEventController.onGameLose.Add(OnGameLose);
            _tubeEventController.onPlayTubeDragAudioRequest.Add(OnPlayTubeDragAudioRequest);
            _tubeEventController.onBallWaveGeneration.Add(OnBallWaveGeneration);
        }

        public void UnregisterFromEvents()
        {
            _gameManagerEventController.onTotalBallsChange.Remove(OnTotalBallsChange);
            _gameManagerEventController.onBallsInCupChange.Remove(OnBallsInCupChange);
            _gameManagerEventController.onGameWon.Remove(OnGameWon);
            _gameManagerEventController.onGameLose.Remove(OnGameLose);
            _tubeEventController.onPlayTubeDragAudioRequest.Remove(OnPlayTubeDragAudioRequest);
            _tubeEventController.onBallWaveGeneration.Remove(OnBallWaveGeneration);
        }

        private void OnBallWaveGeneration()
        {
            _ballsCreationWave.Play();
        }

        private void OnPlayTubeDragAudioRequest(bool play)
        {
  
            if (play)
            {
                _tubeDragAudioPlayer.Play();
                return;
            }
        
            _tubeDragAudioPlayer.FadeOutStop(.3f);

        }

        private void OnGameLose()
        {
            _loseAudioPlayer.Play();
        }

        private void OnGameWon(int startsCount)
        {
            _winAudioPlayer.Play();
            
        }

        private void OnBallsInCupChange(int count)
        {
            _ballGetInsideCupAudio.Play();
            _text_ballsInCup.text = count.ToString();
        }

        private void OnTotalBallsChange(int count)
        {
            _text_totallBalls.text = count.ToString();
        }

        public void OnPauseMenuClick()
        {
            _clickAudioPalyer.Play();
            _pauseMenu.BringUp();
        }

        public void OnFinishClick()
        {
            _clickAudioPalyer.Play();
            _gameManagerEventController.onGameEnd.Trigger();
        }

        #endregion
    }
}