using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    public enum UIType
    {
        USING,        
        USED,
        COST
    }
    public UIType type;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();    
    }

    void Update()
    {
        switch (type)
        {
            case UIType.USING:
            text.text = "" + Knight.instance.usingCard;
            break;
            case UIType.USED:
            text.text = "" + Knight.instance.usedCard;
            break;
            case UIType.COST:
            text.text = "" + GameManager.instance.cost;
            break;
        }
    }
}
