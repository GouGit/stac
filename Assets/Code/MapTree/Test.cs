using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Traveler traveler;
    public SceneOption option = new SceneOption();

    public void Change()
    {

    }

    public void GoStart()
    {

    }

    public void GoEnd()
    {
        Spot nowSpot = traveler.nowSpot;
        if(nowSpot.nextSpots.Count > 0)
        {
            traveler.ChangeSpot(nowSpot.nextSpots[0]);
        }
        else
        {
            
        }
    }

    void Awake()
    {
        if(GameManager.instance.isFirstStart)
        {
            // traveler.nowSpot = MapDataHandler.CreateMap("Test");
            // MapDataHandler.SaveMapJson(traveler.nowSpot, "Test");
            traveler.nowSpot = MapDataHandler.LoadMapJson("Test");
            GameManager.instance.isFirstStart = false;
            Spot.SetParent();
        }
        else
        {
            Spot firstSpot = MapDataHandler.CreateMap("Test_Progress");
            traveler.nowSpot = Spot.GetProgressSpot();
            Spot.SetParent();
        }
    }
}
