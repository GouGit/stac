using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviour
{
    public enum STATE
    {
        FIRE,
        LIGHTING,
        POISION,
        MEDUSA,
        SUCCUBUS
    }
    public STATE state;
    private Text text;

    void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }

    void FixedUpdate()
    {
        switch (state)
        {
        case STATE.FIRE:
            text.text = "" + Knight.instance.bCnt;
            break;
        case STATE.LIGHTING:
            break;
        case STATE.POISION:
            break;
        case STATE.MEDUSA:
            text.text = "" + Knight.instance.pCnt;
            break;
        case STATE.SUCCUBUS:
            text.text = "" + Knight.instance.fCnt;
            break;
        }
    }
}
