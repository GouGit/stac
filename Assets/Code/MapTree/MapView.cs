using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    public GameObject map;
    public Vector3 mapStartPosition;
    public Vector3 mapEndPosition;

    public GameObject routePickerLayout;
    public Vector3 pickerStartPosition;
    public Vector3 pickerEndPosition;

    [Range(0.0f, 10.0f)]
    public float lerp;

    private Vector3 mapTargetPosition;
    private Vector3 pickerTargetPosition;

    bool pickerUp = false;

    void Awake()
    {
        map = GameObject.Find("Map");
        routePickerLayout = GameObject.Find("GridLayout");

        pickerStartPosition = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f - Screen.height * 0.065f);
        pickerEndPosition = pickerStartPosition;
        pickerEndPosition.y = -2000.0f;

        Change();
        Change();
    }

    public void Change()
    {
        pickerUp = !pickerUp;

        if(pickerUp)
        {
            mapTargetPosition = mapEndPosition;
            pickerTargetPosition = pickerStartPosition;
        }
        else
        {
            mapTargetPosition = mapStartPosition;
            pickerTargetPosition = pickerEndPosition;
        }
    }

    void Update()
    {
        map.transform.position = Vector3.Lerp(map.transform.position, mapTargetPosition, Time.deltaTime * lerp);
        routePickerLayout.transform.position = Vector3.Lerp(routePickerLayout.transform.position, pickerTargetPosition, Time.deltaTime * lerp);
    }
}
