using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    private GameObject temp;

    void Awake()
    {
        temp = Instantiate(Resources.Load("Skeleton") as GameObject, Vector3.zero, Quaternion.identity);
        MonsterOption.AllMonsters.AddLast(temp);
        temp = Instantiate(Resources.Load("Gagoil") as GameObject, Vector3.zero, Quaternion.identity);
        MonsterOption.AllMonsters.AddLast(temp);
        MonsterOption.SetPos();
        int i = 0;
        for(var node = MonsterOption.AllMonsters.First; node != null; node = node.Next)
        {
            node.Value.transform.position = MonsterOption.ReturnPos(i);
            ++i;
        }
    }

}
