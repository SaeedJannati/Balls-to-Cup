using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Random = UnityEngine.Random;

namespace BallsToCupGeneral.Audio
{
    public class AudioPlayHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private AudioSource _audioSource;

        #endregion
        #region Methods

        public void SetRandomPitch(float toleranceRatio)
        {
            var sign = Random.Range(0, 2) > 0 ? -1 : 1;
            var delta = Random.Range(0.0f, toleranceRatio);
            _audioSource.pitch = 1.0f * (1.0f + sign * delta);
        }

        public void SetPitch(float pitch)
        {
            _audioSource.pitch = pitch;
        }

        public float GetLength()
        {
            if (_audioSource == default)
                return 0.0f;
            if (_audioSource.clip == default)
                return 0.0f;
            
            return _audioSource.clip.length;
        }

        public AudioPlayHandler Play(AudioClip clip, AudioMixerGroup audioMixerGroup, float volume = 1.0f,
            bool loop = false)
        {
            _audioSource.pitch = 1.0f;
            _audioSource.clip = clip;
            _audioSource.outputAudioMixerGroup = audioMixerGroup;
            _audioSource.volume = volume;
            _audioSource.loop = loop;
            _audioSource.Play();
            if (loop)
            {
                var destVolume = _audioSource.volume;
                _audioSource.volume = 0.0f;
                _audioSource.DOFade(destVolume, 1.0f);
            }

            if (loop)
                return this;
            SetRandomPitch(.1f);
            if (!gameObject.activeInHierarchy)
                return this;
            StartCoroutine(DisableRoutine(_audioSource.clip.length));
            return this;
        }

        public AudioPlayHandler FadeInPlay(AudioClip clip, AudioMixerGroup group, float volume, float fadeDuration)
        {
            Play(clip, group, volume,true);
            var destVolume = _audioSource.volume;
            _audioSource.volume = 0.0f;
            _audioSource.DOFade(destVolume, fadeDuration);
            return this;
        }
        public AudioPlayHandler FadeOutStop(AudioClip clip, AudioMixerGroup group, float volume, float fadeDuration)
        {
            // Play(clip, group, volume,true);
            var destVolume = _audioSource.volume;
            _audioSource.volume = destVolume;
            _audioSource.DOFade(0.0f, fadeDuration).onComplete = () =>
            {
                Stop();
            };
            return this;
        }
        
        public void Fade(Action onComplete = default)
        {
            var tween = _audioSource.DOFade(0.0f, 1.0f);
            tween.onComplete += () =>
            {
                onComplete?.Invoke();
                _audioSource.clip = default;
                gameObject.SetActive(false);
            };
        }

        public void FadeOutStop(float fadeDuration)
        {
            if (!_audioSource.isPlaying)
                return;
            var fadeTween =
                _audioSource.DOFade(0.0f, fadeDuration);
            fadeTween.onComplete += () =>
            {
                _audioSource.clip = default;
                gameObject.SetActive(false);
            };
        }

        public void Stop()
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
            _audioSource.clip = default;
            gameObject.SetActive(false);
        }

        IEnumerator DisableRoutine(float period)
        {
            yield return new WaitForSeconds(period);
            _audioSource.clip = default;
            gameObject.SetActive(false);
        }

        #endregion
    }
}