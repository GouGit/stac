using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public LinkedList<GameObject> AllMonsters = new LinkedList<GameObject>();
    private List<Vector3> posistions =  new List<Vector3>();

    public void CreateMonster(GameObject monster)
    {
        GameObject temp = Instantiate(monster, Vector3.zero, Quaternion.identity);
        AllMonsters.AddLast(temp);
    }

    public void SetMonsterPosition()
    {
        SetPos();
        int i = 0;
        for(var node = AllMonsters.First; node != null; node = node.Next)
        {
            node.Value.transform.position = ReturnPos(i);
            ++i;
        }
    }

    private void SetPos()
    {
        posistions.Clear();
        switch (AllMonsters.Count)
        {
        case 1:
            posistions.Add(new Vector3(0,2.75f,0));
            break;
        case 2:
            posistions.Add(new Vector3(-2.25f,2.75f,0));
            posistions.Add(new Vector3(2.25f,2.75f,0));
            break;
        case 3:
            posistions.Add(new Vector3(-4.5f,2.75f,0));
            posistions.Add(new Vector3(0,2.75f,0));
            posistions.Add(new Vector3(4.5f,2.75f,0));
            break;
        }
    }

    private Vector3 ReturnPos(int number)
    {
        return posistions[number];
    }

    public void Remove()
    {
        for(var node = AllMonsters.First; node != null; node = node.Next)
        {
            if(!node.Value.activeSelf)
            {
                Debug.Log("check");
                AllMonsters.Remove(node);
            }
        }
    }
}
