using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    [HideInInspector]
    public bool playerTurn;
    public int cost = 3;

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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerTurn = true;
    }
}