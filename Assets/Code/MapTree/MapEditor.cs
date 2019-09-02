using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public Spot spot = null;
    public string map_name;
    public bool save = false;

    void Update()
    {
        if (save && spot != null)
        {
            GameDataHandler.SaveMap(spot, map_name);
            save = false;
        }        
    }
}
