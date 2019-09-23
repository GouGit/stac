using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("SceneLoader").AddComponent<SceneLoader>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        { Destroy(this); return; }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static void LoadScene(int index, float delay = 0)
    {
        Time.timeScale = 1;
        if (delay != 0)
        {
            Instance.LoadSceneWithDelay(index, delay);
            return;
        }
        SceneManager.LoadScene(index);
    }

    public static void LoadScene(string name, float delay = 0)
    {
        Time.timeScale = 1;
        if(delay != 0)
        {
            Instance.LoadSceneWithDelay(name, delay);
            return;
        }
        SceneManager.LoadScene(name);
    }

    public void LoadSceneWithDelay(int index, float delay)
    {
        StartCoroutine(CO_LoadSceneWithDelay(delay, () => LoadScene(index)));
    }

    public void LoadSceneWithDelay(string name, float delay)
    {
        StartCoroutine(CO_LoadSceneWithDelay(delay, () => LoadScene(name)));
    }

    public static IEnumerator CO_LoadSceneWithDelay(float delay, UnityEngine.Events.UnityAction action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static void LoadScene(string name, SceneOption option)
    {
        SceneOptionTransporter transporter = Instantiate(Resources.Load("SceneOptionTransporter") as GameObject).GetComponent<SceneOptionTransporter>();
        transporter.sceneOption = option;
        LoadSceneWithFadeStatic(name);
    }

    public static string GetNowSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static int GetNowSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadSceneWithFadeStatic(int index)
    {
        var fadePanel = GameObject.Find("FadePanel").GetComponent<FadeUI>();
        fadePanel.m_OnFadeInEnd += (obj) =>
        {
            LoadScene(index);
        };
        fadePanel.FadeIn();

        // 씬에 FadeUI가 2개 이상 있을 시 문제가 되므로 이름으로 오브젝트를 찾음
        //FadeUI[] fadePanels = FindObjectsOfType<FadeUI>();
        //foreach (var fadePanel in fadePanels)
        //{
        //    fadePanel.ClearCallbacks();
        //    fadePanel.m_OnFadeInEnd += (obj) =>
        //    {
        //        LoadScene(index);
        //    };
        //    fadePanel.FadeIn();
        //    break;
        //}
    }

    public static void LoadSceneWithFadeStatic(string name)
    {
        var fadePanel = GameObject.Find("FadePanel").GetComponent<FadeUI>();
        fadePanel.m_OnFadeInEnd += (obj) =>
        {
            LoadScene(name);
        };
        fadePanel.FadeIn();

        // 씬에 FadeUI가 2개 이상 있을 시 문제가 되므로 이름으로 오브젝트를 찾음
        //FadeUI[] fadePanels = FindObjectsOfType<FadeUI>();
        //foreach (var fadePanel in fadePanels)
        //{
        //    fadePanel.ClearCallbacks();
        //    fadePanel.m_OnFadeInEnd += (obj) =>
        //  {
        //      LoadScene(name);
        //  };
        //    fadePanel.FadeIn();
        //}
    }

    public void LoadSceneWithFade(string name)
    {
        // Debug.Log(name);
        FadeUI[] fadePanels = FindObjectsOfType<FadeUI>();
        foreach (var fadePanel in fadePanels)
        {
            fadePanel.m_OnFadeInEnd += (obj) =>
          {
              LoadScene(name);
          };
            fadePanel.FadeIn();
        }
    }

    public void LoadSceneWithFade(int index)
    {
        FadeUI[] fadePanels = FindObjectsOfType<FadeUI>();
        foreach (var fadePanel in fadePanels)
        {
            fadePanel.m_OnFadeInEnd += (obj) =>
            {
                LoadScene(index);
            };
            fadePanel.FadeIn();
        }
    }

    public void ChangeSceneToMenu()
    {
        LoadScene("Menu");
    }

    public void ChangeSceneToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ChangeSceneToWaveGame()
    {
        SceneManager.LoadScene("Game_Wave");
    }
}
