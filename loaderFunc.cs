using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

namespace sceneLoading
{
    public class loaderFunc : MonoBehaviour
    {
        List<GameObject> rootObjects = new List<GameObject>();
        public string newScenePath;
        string oldScenePath;
        float fadeTime = 0.5f;
        public void load()
        {
            oldScenePath = SceneManager.GetActiveScene().name;
            StartCoroutine(loadCoroutine(newScenePath, oldScenePath));
        }

        IEnumerator loadCoroutine(string newScenePath, string oldScenePath)
        {
            SteamVR_Fade.View(Color.clear, 0f);
            SteamVR_Fade.View(Color.black, fadeTime);
            yield return new WaitForSeconds(fadeTime);
            AsyncOperation scene = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
            SceneManager.GetSceneByName(oldScenePath);
            while (!scene.isDone)
            {
                yield return null;
            }
            Scene loadingScene = SceneManager.GetSceneByName("LoadingScene");
            GameObject loader = loadingScene.GetRootGameObjects()[1];
            loader.GetComponent<sceneLoader>().loadScene(newScenePath, oldScenePath);
            SceneManager.SetActiveScene(loadingScene);
        }
    }
}

