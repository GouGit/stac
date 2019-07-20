using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

public static class MapDataHandler
{
    private static void ResetSpot(Spot spot)
    {//  모든 Spot에 지나갔던 흔적을 지웁니다.
        if (!spot.isTraversal)
            return;

        spot.isTraversal = false;

        int count = spot.nextRoutes.Count;
        for (int i = 0; i < count; i++)
        {
            ResetSpot(spot.nextRoutes[i]);
        }
    }

    private static int SetID(Spot spot)
    {// 모든 Spot에 ID를 부여합니다. 그리고 모든 spot의 갯수를 리턴합니다.
        Debug.Log("각각 spot에 ID를 부여합니다.");
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

        int count = spot.nextRoutes.Count;
        for (int i = 0; i < count; i++)
        {
            SetID(spot.nextRoutes[i], ref IDcount);
        }
    }

    /// <summary>
    /// 맵을 저장합니다.
    /// </summary>
    /// <param name="spot">제일 첫 spot</param>
    /// <param name="filePath">XML파일의 위치</param>
    public static void SaveMap(Spot spot, string filePath)
    {
        Debug.Log("맵을 저장합니다.");

        int spotCount = SetID(spot);

        XmlDocument document = new XmlDocument();
        document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        XmlElement root = document.CreateElement("Stage_Test");
        document.AppendChild(root);

        XmlElement child = document.CreateElement("Spots");
        child.SetAttribute("SpotCount", spotCount.ToString());
        root.AppendChild(child);

        SaveMap(spot, document, child);

        ResetSpot(spot);

        document.Save(filePath);
    }

    private static void SaveMap(Spot spot, XmlDocument document, XmlNode root)
    {
        if (spot.isTraversal)
        {// 이미 지나간 Spot 입니다. (트리의 노드가 이어지는 부분 입니다.)
            XmlElement nextSpot = document.CreateElement("nextSpot");
            nextSpot.InnerText = spot.ID.ToString();
            root.AppendChild(nextSpot);

            return;
        }

        XmlElement wow = document.CreateElement("spot");

        XmlElement IDcount = document.CreateElement("ID");
        IDcount.InnerText = spot.ID.ToString();
        wow.AppendChild(IDcount);

        XmlElement position = document.CreateElement("position");
        position.InnerText = spot.transform.position.ToString();
        wow.AppendChild(position);

        XmlElement type = document.CreateElement("type");
        type.InnerText = ((int)spot.sceneOption.type).ToString();
        wow.AppendChild(type);

        XmlElement prefabs = document.CreateElement("prefabs");
        prefabs.SetAttribute("PrefabCount", spot.sceneOption.objectList.Count.ToString());

        foreach (GameObject prefabObject in spot.sceneOption.objectList)
        {
            XmlElement prefab = document.CreateElement("prefab");
            prefab.InnerText = prefabObject.name;
            prefabs.AppendChild(prefab);        
        }

        wow.AppendChild(prefabs);

        spot.isTraversal = true;

        int count = spot.nextRoutes.Count;
        for (int i = 0; i < count; i++)
        {
            SaveMap(spot.nextRoutes[i], document, wow);
        }
        root.AppendChild(wow);
    }


    /// <summary>
    /// 맵을 불러옵니다.
    /// </summary>
    /// <param name="spot">제일 첫 spot</param>
    /// <param name="filePath">XML파일의 위치</param>
    public static void LoadProgress(Spot spot, string filePath)
    {
        // Debug.Log("진행도를 불러옵니다.");
        TextAsset textAsset = (TextAsset)Resources.Load(filePath);
        // Debug.Log(textAsset);
        XmlDocument document = new XmlDocument();

        document.LoadXml(textAsset.text);
        // Debug.Log(document.InnerText);

        XmlNode root = document.SelectSingleNode("Stage_Test/Spots/spot");

        LoadProgress(spot, document, root);
    }

    private static void LoadProgress(Spot spot, XmlDocument document, XmlNode root)
    {
        // spot.transform.position = root.SelectSingleNode("position").InnerText.;

        if (!spot.isTraversal)
        {
            spot.isTraversal = true;
            XmlNodeList list = root.SelectNodes("spot");
            int count = spot.nextRoutes.Count;
            for (int i = 0; i < count; i++)
            {
                LoadProgress(spot.nextRoutes[i], document, list[i]);
            }
        }
    }

    /// <summary>
    /// XML파일을 이용하여 맵을 구성하고 제일 처음 spot을 리턴합니다.
    /// </summary>
    /// <param name="filePath">XML파일의 위치</param>
    public static Spot CreateMap(string filePath)
    {
        Debug.Log(filePath);
        TextAsset textAsset = (TextAsset)Resources.Load(filePath);
        Debug.Log(textAsset);
        XmlDocument document = new XmlDocument();

        document.LoadXml(textAsset.text);

        XmlNode spots = document.SelectSingleNode("Stage_Test/Spots");
        int count = (spots as XmlElement).GetAttribute("SpotCount").ToInt();

        List<Spot> spotList = new List<Spot>();

        XmlNode root = spots.SelectSingleNode("spot");
        Debug.Log(root.InnerText);
        return CreateMap(document, root, spotList);
    }

    private static Spot CreateMap(XmlDocument document, XmlNode root, List<Spot> spotList)
    {
        int ID = root.SelectSingleNode("ID").InnerText.ToInt();
        Vector3 position = root.SelectSingleNode("position").InnerText.ToVector3();
        // position.x += 10;
        int type = root.SelectSingleNode("type").InnerText.ToInt();
        XmlNodeList nextSpotList = root.SelectNodes("nextSpot");

        Spot spot = (GameObject.Instantiate(Resources.Load("Spot"), position, Quaternion.identity) as GameObject).GetComponent<Spot>();
        spot.ID = ID;
        spot.sceneOption.type = (SceneOption.Type)type;

        XmlNodeList prefabList = root.SelectNodes("prefabs");
        for(int i = 0; i < prefabList.Count; i++)
        {
            string name = prefabList[i].InnerText;
            if(!name.Equals(""))
                spot.sceneOption.objectList.Add(Resources.Load(prefabList[i].InnerText) as GameObject);

        }

        spotList.Add(spot);
        
        for(int i = 0; i < nextSpotList.Count;i++)
        {
            spot.nextRoutes.Add(spotList[nextSpotList[i].InnerText.ToInt()]);
        }

        XmlNodeList list = root.SelectNodes("spot");
        int count = list.Count;
        for(int i = 0; i < count; i++)
        {
            spot.nextRoutes.Add(CreateMap(document, list[i], spotList));
        }

        return spot;
    }
}
