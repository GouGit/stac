using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultWindow : MonoBehaviour
{
    public Button TitleButton;

    void Start()
    {
        OnInitialized();
    }

    protected void OnInitialized()
    {
        // TitleButton.onClick.AddListener(OnClickTitleButton);
    }

    protected void OnClickTitleButton()
    {
        // SceneLoader.LoadSceneWithFadeStatic("Title");
    }
}
