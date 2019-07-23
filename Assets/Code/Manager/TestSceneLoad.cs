using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    private GameObject temp;

    void Start()
    {
        temp = Instantiate(Resources.Load("Player") as GameObject, Vector3.zero, Quaternion.identity);    
    }

}
