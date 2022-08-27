using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] UnityEngine.UI.Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;

    Color color;
    const string GAMEPLAY = "Game";
    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator LoadCoroutine(string sceneName)
    {
        //异步同时加载场景
        var loadingOperation= SceneManager.LoadSceneAsync(sceneName);
        //将当前场景禁用
        loadingOperation.allowSceneActivation = false;
        transitionImage.gameObject.SetActive(true);
        while (color.a < 1f)
        {
           color.a= Mathf.Clamp01( color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;

        }
        loadingOperation.allowSceneActivation = true;
        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;

        }
        transitionImage.gameObject.SetActive(false);
    }
    public void LoadGameplayScene()
    {
        StartCoroutine(LoadCoroutine(GAMEPLAY));

    }
}
