using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public static class MonsterOption
{
    public static LinkedList<GameObject> AllMonsters = new LinkedList<GameObject>();
    private static List<Vector3> posistions =  new List<Vector3>();

    public static void SetPos()
    {
        posistions.Clear();
        switch (AllMonsters.Count)
        {
        case 1:
            posistions.Add(new Vector3(0,2.75f,0));
            break;
        case 2:
            posistions.Add(new Vector3(-2.25f,2.75f,0));
            posistions.Add(new Vector3(2.25f,2.75f,0));
            break;
        case 3:
            posistions.Add(new Vector3(-4.5f,2.75f,0));
            posistions.Add(new Vector3(0,2.75f,0));
            posistions.Add(new Vector3(4.5f,2.75f,0));
            break;
        }
    }

    public static Vector3 ReturnPos(int number)
    {
        return posistions[number];
    }

    public static void Remove()
    {
        for(var node = AllMonsters.First; node != null; node = node.Next)
        {
            if(!node.Value.activeSelf)
            {
                AllMonsters.Remove(node);
            }
        }
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
            SceneLoader.LoadSceneWithFadeStatic("MapTree");
            // SceneLoader.LoadSceneWithFadeStatic("");
        });
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AutoReset;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= AutoReset;
    }

    void AutoReset(Scene scene, LoadSceneMode mode)
    {
        if (string.Equals(scene.name, "Title"))
        {
            isFirstStart = true;
        }
    }
}
