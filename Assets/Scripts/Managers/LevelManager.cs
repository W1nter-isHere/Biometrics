using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Channels;
using Core;
using Dialogue.Scripts;
using Dialogue.Scripts.DataKeepers;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public EventChannel eventChannel;
        public GameObject blocker;
        public static GameObject HUDCanvas;
        
        private void Awake()
        {
            blocker.SetActive(false);
        }

        public void LoadMainMenu()
        {
            LoadLevel(0);
        }
        
        public void FadeBlack()
        {
            blocker.SetActive(true);
            LeanTween.value(blocker, color => blocker.GetComponent<Image>().color = color, blocker.GetComponent<Image>().color, Color.black, 0.2f);
        }

        public void LoadLevel(int sceneToLoad, bool useWhite = false, float waitTime = 0.2f)
        {
            StartCoroutine(LoadLevelCoroutine(sceneToLoad, -1, useWhite, false, delegate {  }, waitTime));
        }
        
        public void LoadLevel(int sceneToLoad, Action inBetweenCallback, bool useWhite = false, float waitTime = 0.2f)
        {
            StartCoroutine(LoadLevelCoroutine(sceneToLoad, -1, useWhite, false, inBetweenCallback, waitTime));
        }
        
        public void LoadLevel(int sceneToLoad, int passageIDToLookFor, bool useWhite = false, float waitTime = 0.2f)
        {
            StartCoroutine(LoadLevelCoroutine(sceneToLoad, passageIDToLookFor, useWhite, true, delegate {  }, waitTime));
        }
        
        public void LoadLevel(int sceneToLoad, int passageIDToLookFor, Action inBetweenCallback, bool useWhite = false, float waitTime = 0.2f)
        {
            StartCoroutine(LoadLevelCoroutine(sceneToLoad, passageIDToLookFor, useWhite, true, inBetweenCallback, waitTime));
        }

        private IEnumerator LoadLevelCoroutine(int sceneToLoad, int passageIDToLookFor, bool useWhite, bool lookForPassage, Action inBetweenCallback, float waitTime)
        {
            blocker.SetActive(true);
            // load transition color
            LeanTween.value(blocker, color => blocker.GetComponent<Image>().color = color, blocker.GetComponent<Image>().color, useWhite ? Color.white : Color.black, waitTime);

            // stop proceeding until screen is black
            yield return new WaitForSeconds(waitTime);

            try
            {
                inBetweenCallback.Invoke();
            }
            catch
            {
                // ignored
            }

            switch (sceneToLoad)
            {
                case 0:
                    Destroy(PlayerController.Instance.gameObject);
                    Destroy(FindObjectOfType<Camera>().gameObject);
                    break;
                case 11 when EscapeManager.Instance != null:
                    Destroy(EscapeManager.Instance.gameObject);
                    break;
            }

            if (sceneToLoad != SceneManager.GetActiveScene().buildIndex)
            {
                var dataKeepers = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IDataKeeper>();
                foreach (var dataKeeper in dataKeepers)
                {
                    Destroy(dataKeeper.GetGO());
                }
            }

            if (EscapeManager.Instance != null)
            {
                EscapeManager.Instance.Object.Reset();
            }
            
            // load scene
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneToLoad);

            // keep loading until it is finished loading
            while (!loadSceneAsync.isDone)
            {
                yield return null;
            }

            if (lookForPassage)
            {
                try
                {
                    // get the passage manager in the newly loaded scene
                    var passageManager = Array.Find(SceneManager.GetSceneByBuildIndex(sceneToLoad).GetRootGameObjects(), o => o.GetComponent<PassageManager>() != null);

                    if (passageManager != null)
                    {
                        var pm = passageManager.GetComponent<PassageManager>();
                        try
                        {
                            // teleport player to the passage position
                            PlayerController.Instance.Object.transform.position = pm.Passages[passageIDToLookFor].transform.position;
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new Exception($"Tried to transition into scene: {sceneToLoad}, passage: {passageIDToLookFor}, but passage with id is not found");
                        }
                    }
                    else
                    {
                        throw new Exception("Tried to transition into a scene with no passage manager");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception("Tried to transition into a scene with no passage");
                }
            }
            
            // // unload the old scene
            // var unloadSceneAsync = SceneManager.UnloadSceneAsync(_currentLoadedScene);
            //
            // // wait until its unloaded
            // while (!unloadSceneAsync.isDone)
            // {
            //     yield return null;
            // }
            
            if (HUDCanvas != null)
            {
                HUDCanvas.SetActive(sceneToLoad != 0);
            }
            AudioManager.LevelMusic(sceneToLoad);

            // remove transition color
            LeanTween.value(blocker, color => blocker.GetComponent<Image>().color = color, blocker.GetComponent<Image>().color, new Color(0, 0, 0, 0), waitTime);
            yield return new WaitForSeconds(waitTime);
            blocker.SetActive(false);
        }
    }
}