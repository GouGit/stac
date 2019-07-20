using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type
{
    public enum TYPE
    {
        NONE,
        TOPAZ,
        RUBY,
        SAPPHIRE,
        DIAMOND
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isPlayerTurn = true;
    public List<GameObject> AllCards = new List<GameObject>();
    public int cost = 3;

    public ResultWindow ResultWindowPrefab;
    public string mapName;// 현재 맵의 이름 (나중에 사용할 예정)
    public bool isFirstStart = true;// 해당맵이 처음 시작되는것 인지 맵이 바뀔때 마다 true로 해주어야 합니다.

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnGameEnd()
    {
        ResultWindow obj = Instantiate(ResultWindowPrefab);
        obj.TitleButton.onClick.AddListener(() =>
        {
            // SceneLoader.LoadSceneWithFadeStatic("Title");
            SceneLoader.LoadSceneWithFadeStatic("");
        });
    }
}
