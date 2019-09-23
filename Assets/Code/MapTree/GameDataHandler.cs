using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using LitJson;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;

public static class GameDataHandler
{
    public static void CheckFolder()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/TemporaryFiles"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/TemporaryFiles");
        }
    }

    private static void ResetSpot(Spot spot)
    {//  모든 Spot에 지나갔던 흔적을 지웁니다.
        if (!spot.isTraversal)
            return;

        spot.isTraversal = false;

        int count = spot.nextSpots.Count;
        for (int i = 0; i < count; i++)
        {
            ResetSpot(spot.nextSpots[i]);
        }
    }

    private static int SetID(Spot spot)
    {// 모든 Spot에 ID를 부여합니다. 그리고 모든 spot의 갯수를 리턴합니다.
        int IDcount = 0;
        SetID(spot, ref IDcount);
        ResetSpot(spot);

        return IDcount;
    }

    private static void SetID(Spot spot, ref int IDcount)
    {// 트리의 노드를 순회하며 ID를 부여하는 재귀함수 입니다.
        if (spot.isTraversal)
            return;

        spot.isTraversal = true;

        spot.ID = IDcount++;

        int count = spot.nextSpots.Count;
        for (int i = 0; i < count; i++)
        {
            SetID(spot.nextSpots[i], ref IDcount);
        }
    }

    /// <summary>
    /// 맵을 저장합니다.
    /// </summary>
    /// <param name="spot">저장할 맵의 제일 첫 spot</param>
    /// <param name="fileName">JSON 파일의 이름</param>

    public static void SaveMap(Spot spot, string fileName)
    {
        int spotCount = SetID(spot);

        JObject stage = new JObject();
        JObject spots = new JObject();
        JArray jarray = new JArray();

        SaveMap(spot, jarray);

        spots.Add("spots", jarray);

        stage.Add(fileName, spots);

        ResetSpot(spot);

        File.WriteAllText(Application.persistentDataPath + "/MapFiles/" + fileName + ".json", stage.ToString());

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private static void SaveMap(Spot spot, JArray spotArray)
    {
        JObject childSpot = new JObject();

        childSpot.Add("ID", spot.ID);
        childSpot.Add("position", spot.transform.position.ToString());
        childSpot.Add("type", (int)spot.sceneOption.type);
        // 무언가 저장할 데이터가 늘었다면 여기서 추가 하면 됩니다.
        // 주의 : 이곳에서는 spot의 속성만 저장하여야 합니다.

        if (spot.sceneOption.objectList.Count > 0)
        {
            JArray prefabs = new JArray();

            foreach (GameObject prefabObject in spot.sceneOption.objectList)
                prefabs.Add(prefabObject.name);

            childSpot.Add("prefabs", prefabs);
        }

        spot.isTraversal = true;
        int count = spot.nextSpots.Count;
        if (count != 0)
        {
            JArray jarray = new JArray();
            for (int i = 0; i < count; i++)
            {
                Spot next = spot.nextSpots[i];

                if (next.isTraversal)
                    childSpot.Add("nextSpot", next.ID);
                else
                    SaveMap(next, jarray);
            }
            childSpot.Add("spots", jarray);
        }

        spotArray.Add(childSpot);
    }


    /// <summary>
    /// JSON파일을 이용하여 맵을 구성하고 제일 처음 spot을 리턴합니다.
    /// </summary>
    /// <param name="fileName">JSON 파일의 이름</param>
    public static Spot LoadMap(string fileName)
    {
        Debug.Log(fileName);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        string jsonData = (Resources.Load("MapFiles/" + fileName) as TextAsset).text;
        JObject stage = (JObject)JObject.Parse(jsonData)[fileName];
        JArray spots = (JArray)stage["spots"];

        List<Spot> spotList = new List<Spot>();

        return LoadMap((JObject)spots[0], spotList);
    }


    private static Spot LoadMap(JObject root, List<Spot> spotList)
    {
        int ID = root["ID"].ToInt();
        Vector3 position = root["position"].ToVector3();

        SceneOption.Type type = (SceneOption.Type)root["type"].ToInt();

        Spot spot = (GameObject.Instantiate(Resources.Load("Spot"), position, Quaternion.identity) as GameObject).GetComponent<Spot>();
        spot.ID = ID;
        spot.sceneOption.type = type;

        JToken prefabsValue;
        if (root.TryGetValue("prefabs", out prefabsValue))
        {
            JArray prefabList = (JArray)prefabsValue;

            for (int i = 0; i < prefabList.Count; i++)
            {
                string name = "MonsterPrefabs/" + prefabList[i].ToString();
                if (!name.Equals(""))
                    spot.sceneOption.objectList.Add(Resources.Load(name) as GameObject);
            }
        }

        spotList.Add(spot);

        JToken nextSpotValue;
        if (root.TryGetValue("nextSpot", out nextSpotValue))
        {
            spot.nextSpots.Add(spotList[nextSpotValue.ToInt()]);
        }

        JToken spotsValue;
        if (root.TryGetValue("spots", out spotsValue))
        {
            JArray list = (JArray)spotsValue;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                spot.nextSpots.Add(LoadMap((JObject)list[i], spotList));
            }
        }
        return spot;
    }

    /// <summary>
    /// 맵의 진행도를 저장합니다.
    /// </summary>
    /// <param name="spot">진행도를 저장할 맵의 제일 첫 spot</param>
    /// <param name="fileName">저장할 JSON파일의 이름</param>
    public static void SaveProgress(Spot spot, string fileName)
    {
        JObject stage = new JObject();
        JObject spots = new JObject();
        JArray jarray = new JArray();

        SaveProgress(spot, jarray);

        spots.Add("spots", jarray);

        stage.Add(fileName, spots);

        ResetSpot(spot);

        File.WriteAllText(Application.persistentDataPath + "/TemporaryFiles/" + fileName + "_progress.json", stage.ToString());
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private static void SaveProgress(Spot spot, JArray spotArray)
    {
        JObject childSpot = new JObject();

        childSpot.Add("ID", spot.ID);
        childSpot.Add("clear", spot.isClear.ToString().ToLower());

        // 무언가 저장할 데이터가 늘었다면 여기서 추가 하면 됩니다.
        // 주의 : 이곳에서는 spot의 속성만 저장하여야 합니다.

        spot.isTraversal = true;
        int count = spot.nextSpots.Count;
        if (count != 0)
        {
            JArray jarray = new JArray();
            for (int i = 0; i < count; i++)
            {
                Spot next = spot.nextSpots[i];

                if(next.isClear)
                {
                    if (next.isTraversal)
                        childSpot.Add("nextSpot", next.ID);
                    else
                        SaveProgress(next, jarray);
                }
            }
            childSpot.Add("spots", jarray);
        }
        spotArray.Add(childSpot);
    }

    /// <summary>
    /// 맵의 진행도를 불러옵니다.
    /// </summary>
    /// <param name="spot">진행도를 불러올 맵의 첫spot</param>
    /// <param name="fileName">불러올 JSON파일의 이름</param>
    public static void LoadProgress(Spot spot, string fileName)
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        string jsonData = File.ReadAllText(Application.persistentDataPath + "/TemporaryFiles/" + fileName + "_progress.json");
        JObject stage = (JObject)JObject.Parse(jsonData)[fileName];
        JArray spots = (JArray)stage["spots"];

        List<Spot> spotList = new List<Spot>();

        LoadProgress(spot ,(JObject)spots[0]);
    }

    private static void LoadProgress(Spot spot, JObject root)
    {
        int ID = root["ID"].ToInt();

        bool isClear = root["clear"].ToBool();

        if(spot.ID == ID)
        {
            spot.isClear = isClear;
        }
        else
        {
            // 저장파일 훼손 가능성 있음
        }

        JToken spotsValue;
        if (root.TryGetValue("spots", out spotsValue))
        {
            JArray list = (JArray)spotsValue;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                LoadProgress(spot.nextSpots[i], (JObject)list[i]);
            }
        }
    }

    public static void SaveGemCount(int goldCount, int topazCount, int rubyCount, int sapphireCount, int diamondCount)
    {
        JObject gemCount = new JObject();

        gemCount.Add("goldCount", goldCount);
        gemCount.Add("topazCount", topazCount);
        gemCount.Add("rubyCount", rubyCount);
        gemCount.Add("sapphireCount", sapphireCount);
        gemCount.Add("diamondCount", diamondCount);

        File.WriteAllText(Application.persistentDataPath + "/TemporaryFiles/GemCount.json", gemCount.ToString());
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static void LoadGemCount(out int goldCount, out int topazCount, out int rubyCount, out int sapphireCount, out int diamondCount)
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        string jsonData;

        try
        {
            jsonData = File.ReadAllText(Application.persistentDataPath + "/TemporaryFiles/GemCount.json");
        }
        catch (FileNotFoundException)
        {
            goldCount = 0;
            topazCount = 0;
            rubyCount = 0;
            sapphireCount = 0;
            diamondCount = 0;
            return;
        }


        JObject getCount = (JObject)JObject.Parse(jsonData);

        goldCount = getCount["goldCount"].ToInt();
        topazCount = getCount["topazCount"].ToInt();
        rubyCount = getCount["rubyCount"].ToInt();
        sapphireCount = getCount["sapphireCount"].ToInt();
        diamondCount = getCount["diamondCount"].ToInt();
    }

    public static void SaveCards(List<CardSet> cardList)
    {
        JObject list = new JObject();

        JArray cards = new JArray();
        foreach(CardSet set in cardList)
        {
            JObject card = new JObject();
            
            ShowCard cardObj = set.showCard;

            card.Add("name", cardObj.name);
            card.Add("upgradeLevel",set.upgradeLevel);

            cards.Add(card);
        }

        list.Add("CardList", cards);

        File.WriteAllText(Application.persistentDataPath + "/TemporaryFiles/CardList.json", list.ToString());
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static List<CardSet> LoadCards()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif        
        List<CardSet> cardList = new List<CardSet>();
        string jsonData = null;
        
        try
        {
            jsonData = File.ReadAllText(Application.persistentDataPath + "/TemporaryFiles/CardList.json");
        }
        catch (FileNotFoundException)
        {
            SaveCards(GameManager.instance.AllCards);
            return null;
        }
        JObject list = JObject.Parse(jsonData);

        JArray cards = (JArray)list["CardList"];

        if (cards.Count == 0)
        {
            File.Delete(Application.persistentDataPath + "/TemporaryFiles/CardList.json");
            return LoadCards();
        }

        foreach (JObject card in cards)
        {
            ShowCard cardObj = (Resources.Load("CardsPrefabs/" + card["name"].ToString()) as GameObject).GetComponent<ShowCard>();
            int upgradeLevel = card["upgradeLevel"].ToInt();

            cardList.Add(new CardSet(cardObj, upgradeLevel));
        }

        return cardList;
    }

    public static void SaveStageCount(int count)
    {
        JObject stageCount = new JObject();

        stageCount.Add("stage_count", count);

        File.WriteAllText(Application.persistentDataPath + "/TemporaryFiles/StageCount.json", stageCount.ToString());
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static int LoadStageCount()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        int count = 0;
        string jsonData = "";
        try
        {
            jsonData = File.ReadAllText(Application.persistentDataPath + "/TemporaryFiles/StageCount.json");

            JObject jObject = JObject.Parse(jsonData);
            count = jObject["stage_count"].ToInt();
        }
        catch (FileNotFoundException)
        {
            count = 0;
        }

        return count;
    }
}
