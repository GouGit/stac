using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Traveler traveler;
    public SceneOption option = new SceneOption();


    void Awake()
    {
        if(GameManager.instance.isFirstStart)
        {
            // traveler.nowSpot = MapDataHandler.CreateMap("Test");
            // MapDataHandler.SaveMapJson(traveler.nowSpot, "Test");
            Spot.nowSpot = GameDataHandler.LoadMap("test2");
            GameManager.instance.isFirstStart = false;
            Spot.SetParent();
        }
        else
        {
            Spot firstSpot = GameDataHandler.LoadMap("test2");
            GameDataHandler.LoadProgress(firstSpot, "test2");
            Spot.nowSpot = Spot.GetProgressSpot();
            Spot.SetParent();
        }
    }
}
