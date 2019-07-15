using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isPlayerTurn = true;
    public List<GameObject> AllCards = new List<GameObject>();
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
