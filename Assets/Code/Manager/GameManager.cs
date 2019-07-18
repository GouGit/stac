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
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
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
            SceneLoader.LoadSceneWithFadeStatic("Title");
        });
    }
}
