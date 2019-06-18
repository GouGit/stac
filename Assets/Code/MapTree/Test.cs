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
        if(nowSpot.nextRoutes.Count > 0)
        {
            traveler.ChangeSpot(nowSpot.nextRoutes[0]);
        }
        else
        {
            
        }
    }

    void Start()
    {
        // MapDataHandler.SetID(traveler.nowSpot);
        MapDataHandler.SaveMap(traveler.nowSpot, "./Assets/Resources/Test.xml");
        // traveler.nowSpot = MapDataHandler.CreateMap("Test");
        // MapDataHandler.LoadMap(traveler.nowSpot, "Test");
        // traveler.ChangeSpot(traveler.nowSpot.nextRoutes[0]);
    }
}
