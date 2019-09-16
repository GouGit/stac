using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterState : SetTarget
{
    public enum STATE
    {
        FIRE,
        LIGHTING,
        POISION
    }
    public STATE state;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        base.monster = transform.parent.parent.GetComponent<SetTarget>().monster;
    }

    void ShowState()
    {
        switch (state)
        {
        case STATE.FIRE:
            text.text = "" + monster.Fire;
            break;
        case STATE.LIGHTING:
            text.text = "" + monster.Lighting;
            break;
        case STATE.POISION:
            text.text = "" + monster.Poision;
            break;
        }
    }

    void FixedUpdate()
    {
        ShowState();
    }

}
