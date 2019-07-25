using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    private GameObject temp;

    void Awake()
    {
        temp = Instantiate(Resources.Load("Player") as GameObject, Vector3.zero, Quaternion.identity);    
    }

}
