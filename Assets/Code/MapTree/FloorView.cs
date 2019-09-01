using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorView : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> floorList;
    void Start()
    {
        int floor = GameManager.instance.stage_count + 1;

        Color blue = new Color(0.0f, 0.0f, 1.0f);

        for(int i = 0; i < floor; i++)
        {
            floorList[i].GetComponent<SpriteRenderer>().color = blue;
        }
    }
}
