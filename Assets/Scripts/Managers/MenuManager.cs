using System;
using System.Collections;
using Channels;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        public InputChannel inputChannel;
        public AudioMixer audioMixer;

        public Slider masterVolumeSlider;
        public Slider objectVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider uiVolumeSlider;
        public Slider sfxVolumeSlider;
        public Slider dialogueVolumeSlider;
        
        private void Awake()
        {
            transform.Find("Alert").GetComponent<CanvasGroup>().alpha = 0;
            transform.Find("Alert").gameObject.SetActive(false);
            
            transform.Find("SettingsMenu").GetComponent<CanvasGroup>().alpha = 0;
            transform.Find("SettingsMenu").gameObject.SetActive(false);
        }

        private void Start()
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0);
            objectVolumeSlider.value = PlayerPrefs.GetFloat("ObjectVolume", 0);
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0);
            uiVolumeSlider.value = PlayerPrefs.GetFloat("UIVolume", 0);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0);
            dialogueVolumeSlider.value = PlayerPrefs.GetFloat("DialogueVolume", 0);

            audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 0));
            audioMixer.SetFloat("ObjectVolume", PlayerPrefs.GetFloat("ObjectVolume", 0));
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0));
            audioMixer.SetFloat("UIVolume", PlayerPrefs.GetFloat("UIVolume", 0));
            audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0));
            audioMixer.SetFloat("DialogueVolume", PlayerPrefs.GetFloat("DialogueVolume", 0));
        }

        private void OnEnable()
        {
            inputChannel.OnEscapePressed += Ok;
            inputChannel.OnEscapePressed += RemoveSettings;
        }

        private void OnDisable()
        {
            inputChannel.OnEscapePressed -= Ok;
            inputChannel.OnEscapePressed -= RemoveSettings;
        }

        public void NewGame()
        {
            PlayerPrefs.SetInt("SavedLevel", 1);
            PlayerPrefs.SetInt("DeathCount", 0);
            FindObjectOfType<LevelManager>().LoadLevel(1, 0);
        }
        
        public void LoadGame()
        {
            var savedLevel = PlayerPrefs.GetInt("SavedLevel", -1);
            if (savedLevel == -1)
            {
                Alert(false);
            }
            else
            {
                FindObjectOfType<LevelManager>().LoadLevel(savedLevel, 0);
            }
        }

        public void Settings()
        {
            Settings(false);
        }
        
        public void Quit()
        {
            Application.Quit();
        }

        public void Ok()
        {
            Alert(true);
        }

        private void RemoveSettings()
        {
            Settings(true);
        }

        private void Alert(bool remove)
        {
            if (remove)
            {
                if (transform.Find("Alert").GetComponent<CanvasGroup>().alpha == 0) return;
                LeanTween.value(gameObject, f => transform.Find("Alert").GetComponent<CanvasGroup>().alpha = f, 1, 0, 0.1f);
                StartCoroutine(RemoveAlert());
            }
            else
            {
                transform.Find("Alert").gameObject.SetActive(true);
                if (Math.Abs(transform.Find("Alert").GetComponent<CanvasGroup>().alpha - 1) < 0.1) return;
                LeanTween.value(gameObject, f => transform.Find("Alert").GetComponent<CanvasGroup>().alpha = f, 0, 1, 0.1f);
            }

            IEnumerator RemoveAlert()
            {
                yield return new WaitForSeconds(0.1f);
                transform.Find("Alert").gameObject.SetActive(false);
            }
        }
        
        private void Settings(bool remove)
        {
            if (remove)
            {
                if (transform.Find("SettingsMenu").GetComponent<CanvasGroup>().alpha == 0) return;
                LeanTween.value(gameObject, f => transform.Find("SettingsMenu").GetComponent<CanvasGroup>().alpha = f, 1, 0, 0.1f);
                StartCoroutine(RemoveSettingsMenu());
            }
            else
            {
                transform.Find("SettingsMenu").gameObject.SetActive(true);
                if (Math.Abs(transform.Find("SettingsMenu").GetComponent<CanvasGroup>().alpha - 1) < 0.1) return;
                LeanTween.value(gameObject, f => transform.Find("SettingsMenu").GetComponent<CanvasGroup>().alpha = f, 0, 1, 0.1f);
            }

            IEnumerator RemoveSettingsMenu()
            {
                yield return new WaitForSeconds(0.1f);
                transform.Find("SettingsMenu").gameObject.SetActive(false);
            }
        }

        public void MasterSlider(float value)
        {
            audioMixer.SetFloat("MasterVolume", value);
            PlayerPrefs.SetFloat("MasterVolume", value);
        }
        
        public void ObjectsSlider(float value)
        {
            audioMixer.SetFloat("ObjectVolume", value);
            PlayerPrefs.SetFloat("ObjectVolume", value);
        }
        
        public void MusicSlider(float value)
        {
            audioMixer.SetFloat("MusicVolume", value);
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
        
        public void UISlider(float value)
        {
            audioMixer.SetFloat("UIVolume", value);
            PlayerPrefs.SetFloat("UIVolume", value);
        }
        
        public void SFXSlider(float value)
        {
            audioMixer.SetFloat("SFXVolume", value);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
        
        public void DialogueSlider(float value)
        {
            audioMixer.SetFloat("DialogueVolume", value);
            PlayerPrefs.SetFloat("DialogueVolume", value);
        }
    }
}