using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCount : MonoBehaviour
{
    public bool isUsed;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();    
    }

    void Update()
    {
        if(!isUsed)
        {
            text.text = "" + Player.inst.cardsCount;
        }
        else
        {
            text.text = "" + Player.inst.usedCount;
        }
    }
}
