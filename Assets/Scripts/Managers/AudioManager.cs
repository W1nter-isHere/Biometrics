using System;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private Sound[] sounds;
        [SerializeField] private SoundCategory[] categories;
        private AudioSource _currentPlayingMusic;
        private AudioSource _alarm;
        
        private void OnSoundRequested(ESounds sound, float fadeIn)
        {
            var s = Array.Find(sounds, s => s.soundID == sound);
            var mixerGroup = Array.Find(categories, c => c.categoryID == sound.GetCategory());

            if (s.source == null)
            {
                GameObject o;
                s.source = (o = gameObject).AddComponent<AudioSource>();
                LeanTween.value(o, volume => s.source.volume = volume, 0, s.volume, fadeIn);
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.outputAudioMixerGroup = mixerGroup.group;
            }
            s.source.clip = s.GetClip();

            if (sound == ESounds.Alarm)
            {
                _alarm = s.source;
            }
            
            if (_currentPlayingMusic != null && _currentPlayingMusic.clip != s.source.clip)
            {
                if (sound.GetCategory() == ESoundCategory.Music)
                {
                    _currentPlayingMusic.Stop();
                }
                
                s.source.Play();
            }
            else if (_currentPlayingMusic == null)
            {
                s.source.Play();
            }
            
            if (sound.GetCategory() == ESoundCategory.Music)
            {
                _currentPlayingMusic = s.source;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            LevelMusic(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LevelMusic(int newScene)
        {
            switch (newScene)
            {
                case 0:
                    PlayAudio(ESounds.Menu, 2f);
                    break;
                case 1:
                    PlayAudio(ESounds.MainTheme, 2f);
                    break;
                case 11:
                    PlayAudio(ESounds.Menu, 2f);
                    break;
                default:
                    PlayAudio(ESounds.MainTheme, 2f);
                    break;
            }
        }

        public static void PlayAudio(ESounds sound, float fadeIn = 0f)
        {
            FindObjectOfType<AudioManager>().OnSoundRequested(sound, fadeIn);
        }

        public static void StopAudio()
        {
            var manager = FindObjectOfType<AudioManager>();
            if (manager._currentPlayingMusic == null) return;
            manager._currentPlayingMusic.Stop();
        }

        public static void StopAlarm()
        {
            var manager = FindObjectOfType<AudioManager>();
            if (manager._alarm == null) return;
            manager._alarm.Stop();
        }
    }
}
