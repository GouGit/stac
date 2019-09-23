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

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isPlayerTurn = true;
    public List<CardSet> AllCards = new List<CardSet>();
    public int cost = 3;
    public int savingCost = 0;

    public int goldCount = 0;
    public int topazCount = 0;
    public int rubyCount = 0;
    public int sapphireCount = 0;
    public int diamondCount = 0;

    public ResultWindow ResultWindowPrefab;
    public int holyCnt = 0;
    public int stage_count = 0;// 현재 층
    public string mapName;// 현재 맵의 이름 (나중에 사용할 예정)
    public bool isFirstStart = true;// 해당맵이 처음 시작되는것 인지 맵이 바뀔때 마다 true로 해주어야 합니다.

    public MonsterManager monsterOption;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameDataHandler.CheckFolder();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameDataHandler.LoadGemCount(out goldCount, out topazCount, out rubyCount, out sapphireCount, out diamondCount);
        var allcard = GameDataHandler.LoadCards();
        foreach (var card in allcard)
        {
            Debug.Log(card.showCard.level);
        }
        
        if (allcard != null)
        {
            AllCards = allcard;

        }
        //AllCards = GameDataHandler.LoadCards();
        stage_count = GameDataHandler.LoadStageCount();
        mapName = "Stage" + stage_count;
        // GameDataHandler.SaveCards(AllCards);
    }

    void Start()
    {
        monsterOption = GetComponent<MonsterManager>();
    }

    public int GetGem(Type.TYPE type)
    {
        int retval = 0;
        switch (type)
        {
            case Type.TYPE.DIAMOND:
                retval = diamondCount;
                break;

            case Type.TYPE.RUBY:
                retval = rubyCount;
                break;

            case Type.TYPE.SAPPHIRE:
                retval = sapphireCount;
                break;

            case Type.TYPE.TOPAZ:
                retval = topazCount;
                break;
        }

        return retval;
    }

    public void AddGem(Type.TYPE type, int add)
    {
        switch (type)
        {
            case Type.TYPE.DIAMOND:
                diamondCount += add;
                break;

            case Type.TYPE.RUBY:
                rubyCount += add;
                break;

            case Type.TYPE.SAPPHIRE:
                sapphireCount += add;
                break;

            case Type.TYPE.TOPAZ:
                topazCount += add;
                break;
        }

        GameDataHandler.SaveGemCount(goldCount, topazCount, rubyCount, sapphireCount, diamondCount);
        MainUIMnager.Instance.SetText();
    }

    public void AddGold(int gold)
    {
        goldCount += gold;

        GameDataHandler.SaveGemCount(goldCount, topazCount, rubyCount, sapphireCount, diamondCount);
        MainUIMnager.Instance.SetText();
    }

    public void OnGameEnd()
    {
        ResultWindow obj = Instantiate(ResultWindowPrefab);

        int goldCount = 100;
        int topazCount = 0;
        int rubyCount = 0;
        int sapphireCount = 0;
        int diamondCount = 0;

        foreach (GameObject gameObj in Spot.nowSpot.sceneOption.objectList)
        {
            ShowMonster monster = gameObj.GetComponent<ShowMonster>();
            if (monster != null)
            {
                switch (monster.mon.type)
                {
                    case Type.TYPE.RUBY:
                        rubyCount += Random.Range(1, 4);
                        break;

                    case Type.TYPE.SAPPHIRE:
                        sapphireCount += Random.Range(1, 4);
                        break;

                    case Type.TYPE.DIAMOND:
                        diamondCount += Random.Range(1, 4);
                        break;

                    case Type.TYPE.TOPAZ:
                        topazCount += Random.Range(1, 4);
                        break;
                }
            }
        }
        obj.SetOption(goldCount, topazCount, rubyCount, sapphireCount, diamondCount);
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
        if (instance == this)
        {
            SceneManager.sceneLoaded -= AutoReset;
        }
    }

    void AutoReset(Scene scene, LoadSceneMode mode)
    {
        GameDataHandler.SaveGemCount(goldCount, topazCount, rubyCount, sapphireCount, diamondCount);

        switch (scene.name)
        {
            case "Title":
                isFirstStart = true;
                GameDataHandler.LoadGemCount(out goldCount, out topazCount, out rubyCount, out sapphireCount, out diamondCount);
                MainUIMnager.Instance.SetText();
                break;
        }
    }

    public void OnStageClear()
    {
        stage_count++;
        GameDataHandler.SaveStageCount(stage_count);
        isFirstStart = true;
        mapName = "Stage" + stage_count;
    }

    public void SaveProgress()
    {
        GameDataHandler.SaveProgress(Spot.GetFirstSpot(), mapName);
    }

    // public void LoadGemCount()
    // {
    //     goldCount= PlayerPrefs.GetInt("goldCount");

    //     topazCount = PlayerPrefs.GetInt("topazCount");
    //     rubyCount = PlayerPrefs.GetInt("rubyCount");
    //     sapphireCount = PlayerPrefs.GetInt("sapphireCount");
    //     diamondCount = PlayerPrefs.GetInt("diamondCount");
    // }

    // public void SaveGemCount()
    // {
    //     Debug.Log("Save!");
    //     PlayerPrefs.SetInt("goldCount", goldCount);

    //     PlayerPrefs.SetInt("topazCount", topazCount);
    //     PlayerPrefs.SetInt("rubyCount", rubyCount);
    //     PlayerPrefs.SetInt("sapphireCount", sapphireCount);
    //     PlayerPrefs.SetInt("diamondCount", diamondCount);

    //     PlayerPrefs.Save();
    // }
}
