using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void Load(string sceneName, Action OnLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(sceneName, OnLoaded));
        }

        private IEnumerator LoadScene(string sceneName, Action OnLoaded = null)
        {
            if (sceneName == SceneManager.GetActiveScene().name)
            {
                OnLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextSceneAsync = SceneManager.LoadSceneAsync(sceneName);

            while (!waitNextSceneAsync.isDone)
            {
                Debug.Log("NotDone");
                yield return null;
            }

            Debug.Log("SceneLaod");
            OnLoaded?.Invoke();
        }
    }
}    