using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("过渡效果设置")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private bool isTransitioning = false;

    void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 使管理器在场景切换时保留
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 初始化过渡效果
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    // 公共加载方法
    public void LoadScene(string sceneName, bool useFade = true)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionCoroutine(sceneName, useFade));
        }
    }

    // 带进度回调的异步加载
    public void LoadSceneAsync(string sceneName, System.Action<float> onProgress = null)
    {
        if (!isTransitioning)
        {
            StartCoroutine(AsyncLoadCoroutine(sceneName, onProgress));
        }
    }

    private IEnumerator TransitionCoroutine(string sceneName, bool useFade)
    {
        isTransitioning = true;

        // 淡出效果
        if (useFade && fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        // 加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 显示加载进度
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            LoadingUI ui = UIManager.Instance.OpenUI("Loading") as LoadingUI;
            ui.UpdateProgress(progress);

            // 当进度接近完成时，允许场景激活
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // 淡入效果
        if (useFade && fadeImage != null)
        {
            yield return StartCoroutine(FadeIn());
        }

        isTransitioning = false;
    }

    private IEnumerator AsyncLoadCoroutine(string sceneName, System.Action<float> onProgress)
    {
        isTransitioning = true;

        // 淡出效果
        yield return StartCoroutine(FadeOut());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;

            // 调用进度回调
            onProgress?.Invoke(progress);

            LoadingUI ui = UIManager.Instance.OpenUI("Loading") as LoadingUI;
            ui.UpdateProgress(progress);

            if (asyncOperation.progress >= 0.9f)
            {
                // 等待淡入效果完成后激活场景
                yield return StartCoroutine(FadeIn());
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        isTransitioning = false;
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;

        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
}
