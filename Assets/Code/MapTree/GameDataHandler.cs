using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using LitJson;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;

public static class GameDataHandler
{
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


        File.WriteAllText("./Assets/Resources/MapFiles/" + fileName + ".json", stage.ToString());

        UnityEditor.AssetDatabase.Refresh();
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
        UnityEditor.AssetDatabase.Refresh();

        string jsonData = File.ReadAllText("./Assets/Resources/MapFiles/" + fileName + ".json");
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
                string name = prefabList[i].ToString();
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

        File.WriteAllText("./Assets/Resources/TemporaryFiles/" + fileName + "_progress.json", stage.ToString());

        UnityEditor.AssetDatabase.Refresh();
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
        UnityEditor.AssetDatabase.Refresh();

        string jsonData = File.ReadAllText("./Assets/Resources/TemporaryFiles/" + fileName + "_progress.json");
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

        File.WriteAllText("./Assets/Resources/TemporaryFiles/GemCount.json", gemCount.ToString());

        UnityEditor.AssetDatabase.Refresh();
    }

    public static void LoadGemCount(out int goldCount, out int topazCount, out int rubyCount, out int sapphireCount, out int diamondCount)
    {
        UnityEditor.AssetDatabase.Refresh();
        
        string jsonData;

        try
        {
            jsonData = File.ReadAllText("./Assets/Resources/TemporaryFiles/GemCount.json");
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

        File.WriteAllText("./Assets/Resources/TemporaryFiles/CardList.json", list.ToString());

        UnityEditor.AssetDatabase.Refresh();
    }

    public static List<CardSet> LoadCards()
    {
        UnityEditor.AssetDatabase.Refresh();
                
        List<CardSet> cardList = new List<CardSet>();

        string jsonData = File.ReadAllText("./Assets/Resources/TemporaryFiles/CardList.json");
        JObject list = JObject.Parse(jsonData);

        JArray cards = (JArray)list["CardList"];

        foreach(JObject card in cards)
        {
            ShowCard cardObj = (Resources.Load(card["name"].ToString()) as GameObject).GetComponent<ShowCard>();
            int upgradeLevel = card["upgradeLevel"].ToInt();

            cardList.Add(new CardSet(cardObj, upgradeLevel));
        }

        return cardList;
    }



    /////////////////////////////////////////////////////////////////
    // 아래 부터는 이전에 사용하던 XML코드임.  혹시몰라서 일단은 남겨둠 //
    /////////////////////////////////////////////////////////////////


    // private static Spot CreateMap(XmlDocument document, XmlNode root, List<Spot> spotList)
    // {
    //     int ID = root.SelectSingleNode("ID").InnerText.ToInt();
    //     Vector3 position = root.SelectSingleNode("position").InnerText.ToVector3();

    //     int type = root.SelectSingleNode("type").InnerText.ToInt();
    //     bool isClear = bool.Parse(root.SelectSingleNode("clear").InnerText);

    //     XmlNodeList nextSpotList = root.SelectNodes("nextSpot");

    //     Spot spot = (GameObject.Instantiate(Resources.Load("Spot"), position, Quaternion.identity) as GameObject).GetComponent<Spot>();
    //     spot.ID = ID;
    //     spot.sceneOption.type = (SceneOption.Type)type;
    //     spot.isClear = isClear;

    //     XmlNodeList prefabList = root.SelectNodes("prefabs");
    //     for (int i = 0; i < prefabList.Count; i++)
    //     {
    //         string name = prefabList[i].InnerText;
    //         if (!name.Equals(""))
    //             spot.sceneOption.objectList.Add(Resources.Load(name) as GameObject);

    //     }

    //     spotList.Add(spot);

    //     for (int i = 0; i < nextSpotList.Count; i++)
    //     {
    //         Debug.Log(nextSpotList[i].InnerText.ToInt());
    //         spot.nextSpots.Add(spotList[nextSpotList[i].InnerText.ToInt()]);
    //     }

    //     XmlNodeList list = root.SelectNodes("spot");
    //     int count = list.Count;
    //     for (int i = 0; i < count; i++)
    //     {
    //         spot.nextSpots.Add(CreateMap(document, list[i], spotList));
    //     }

    //     return spot;
    // }

    // public static Spot CreateMap(string filePath)
    // {
    //     TextAsset textAsset = (TextAsset)Resources.Load(filePath);
    //     XmlDocument document = new XmlDocument();

    //     document.LoadXml(textAsset.text);

    //     XmlNode spots = document.SelectSingleNode("Stage_Test/Spots");
    //     int count = (spots as XmlElement).GetAttribute("SpotCount").ToInt();

    //     List<Spot> spotList = new List<Spot>();

    //     XmlNode root = spots.SelectSingleNode("spot");
    //     return CreateMap(document, root, spotList);
    // }

    // private static void SaveMap(Spot spot, XmlDocument document, XmlNode root)
    // {
    //     if (spot.isTraversal)
    //     {// 이미 지나간 Spot 입니다. (트리의 노드가 이어지는 부분 입니다.)
    //         XmlElement nextSpot = document.CreateElement("nextSpot");
    //         nextSpot.InnerText = spot.ID.ToString();
    //         root.AppendChild(nextSpot);

    //         return;
    //     }

    //     XmlElement wow = document.CreateElement("spot");

    //     XmlElement IDcount = document.CreateElement("ID");
    //     IDcount.InnerText = spot.ID.ToString();
    //     wow.AppendChild(IDcount);

    //     XmlElement position = document.CreateElement("position");
    //     position.InnerText = spot.transform.position.ToString();
    //     wow.AppendChild(position);

    //     XmlElement type = document.CreateElement("type");
    //     type.InnerText = ((int)spot.sceneOption.type).ToString();
    //     wow.AppendChild(type);

    //     XmlElement clear = document.CreateElement("clear");
    //     clear.InnerText = spot.isClear.ToString().ToLower();
    //     wow.AppendChild(clear);

    //     XmlElement prefabs = document.CreateElement("prefabs");
    //     prefabs.SetAttribute("PrefabCount", spot.sceneOption.objectList.Count.ToString());

    //     foreach (GameObject prefabObject in spot.sceneOption.objectList)
    //     {
    //         XmlElement prefab = document.CreateElement("prefab");
    //         prefab.InnerText = prefabObject.name;
    //         prefabs.AppendChild(prefab);
    //     }

    //     wow.AppendChild(prefabs);

    //     spot.isTraversal = true;

    //     int count = spot.nextSpots.Count;
    //     for (int i = 0; i < count; i++)
    //     {
    //         SaveMap(spot.nextSpots[i], document, wow);
    //     }
    //     root.AppendChild(wow);
    // }

    // public static void SaveMap(Spot spot, string filePath)
    // {
    //     int spotCount = SetID(spot);

    //     XmlDocument document = new XmlDocument();
    //     document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", "yes"));

    //     XmlElement root = document.CreateElement("Stage_Test");
    //     document.AppendChild(root);

    //     XmlElement child = document.CreateElement("Spots");
    //     child.SetAttribute("SpotCount", spotCount.ToString());
    //     root.AppendChild(child);

    //     SaveMap(spot, document, child);

    //     ResetSpot(spot);

    //     document.Save(filePath);
    //     UnityEditor.AssetDatabase.Refresh();
    //     // document.
    // }
}
