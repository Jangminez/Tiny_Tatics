using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup loadingCanvas;
    [SerializeField] private AnimationCurve fadeOutCurve;
    [SerializeField] private AnimationCurve fadeInCurve;
    [SerializeField] private float fadeDuration = 1f;

    private SoundManager soundManager;

    public void LoadSceneAsync(string sceneName, Action onSceneLoaded = null)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onSceneLoaded));
    }

    IEnumerator LoadSceneAsyncCoroutine(string sceneName, Action onSceneLoaded = null)
    {
        if (SoundManager.Instance != null)
        {
            if (soundManager == null)
            {
                soundManager = SoundManager.Instance;
            }

            soundManager.SetSFXVolume(0.5f);
            soundManager.PlaySFX("Click");
        }
        
        yield return StartCoroutine(FadeRoutine(fadeOutCurve, true));

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        yield return null;

        if (sceneName == "BattleScene") 
        {
            if (ManaSystem.Instance != null)
                ManaSystem.Instance.ResetMana();

            var cardSlotManager = GameObject.FindObjectOfType<CardSlotManager>();
        }

        onSceneLoaded?.Invoke();

        yield return StartCoroutine(FadeRoutine(fadeInCurve, false));
    }

    private IEnumerator FadeRoutine(AnimationCurve curve, bool blockRaycasts)
    {
        float time = 0f;
        loadingCanvas.blocksRaycasts = blockRaycasts;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            loadingCanvas.alpha = curve.Evaluate(t);
            time += Time.deltaTime;
            yield return null;
        }

        loadingCanvas.alpha = curve.Evaluate(1f);
        loadingCanvas.blocksRaycasts = blockRaycasts;
    }
}