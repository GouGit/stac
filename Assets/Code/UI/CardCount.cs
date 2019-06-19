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
        if(!isUsed)
        {
            text.text = "" + player.cardsCount;
        }
        else
        {
            
            text.text = "" + player.usedCount;
        }
    }
}
