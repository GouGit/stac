using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Traveler traveler;
    public SceneOption option = new SceneOption();


    void Awake()
    {
        string mapName = GameManager.instance.mapName;
        if(GameManager.instance.isFirstStart)
        {
            // traveler.nowSpot = MapDataHandler.CreateMap("Test");
            // MapDataHandler.SaveMapJson(traveler.nowSpot, "Test");
            Spot.nowSpot = GameDataHandler.LoadMap(mapName);
            GameManager.instance.isFirstStart = false;
            Spot.SetParent();
        }
        else
        {
            Spot firstSpot = GameDataHandler.LoadMap(mapName);
            GameDataHandler.LoadProgress(firstSpot, mapName);
            Spot.nowSpot = Spot.GetProgressSpot();
            Spot.SetParent();
        }
    }
}
