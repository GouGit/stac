using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCount : MonoBehaviour
{
    public bool isUsed;
    private Text text;
    Player player;

    void Start()
    {
        text = GetComponent<Text>();
        player = Player.inst;    
    }

    void Update()
    {
        Debug.Log("asdf");
        if(!isUsed)
        {
            Debug.Log("??? : " + player.cardsCount);
            text.text = "" + player.cardsCount;
        }
        else
        {
            
            Debug.Log("used : " + player.usedCount);
            text.text = "" + player.usedCount;
        }
    }
}
