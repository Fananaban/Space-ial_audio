using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

namespace sceneLoading
{
    public class sceneLoader : MonoBehaviour
    {
        public bool loadDone = false;
        public bool unLoadDone = false;
        float fadeTime = 0.5f;
        public void loadScene(string newScenePath, string oldScenePath)
        {
            StartCoroutine(loadCoroutine(newScenePath, oldScenePath));
        }

        IEnumerator loadCoroutine(string newScenePath, string oldScenePath)
        {
            //Unload Previous scene;
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScenePath);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            SteamVR_Fade.View(Color.black, 0f);
            SteamVR_Fade.View(Color.clear, fadeTime);
            yield return new WaitForSeconds(fadeTime);

            //Load Next Scene;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newScenePath);
            while (asyncLoad.progress <= 0.85f)
            {
                yield return null;
            }
            asyncLoad.allowSceneActivation = false;

            //Start Camera Fade;
            SteamVR_Fade.View(Color.black, fadeTime);
            yield return new WaitForSeconds(fadeTime);
            asyncLoad.allowSceneActivation = true;
            SteamVR_Fade.View(Color.clear, fadeTime);
            yield return new WaitForSeconds(fadeTime);
            //Set the new scene to be active;
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath("newScenePath"));
        }
    }
}
