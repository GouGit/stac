﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    public enum TYPE
    {
        HP,
        ATTACK,
        DEFENS,
        SHELID
    }
    public TYPE type;
    private ShowMonster mon;
    private SetMonster set;
    private Text text;
    private Image image;
    private float maxHp;

    void Start()
    {
        if(type == TYPE.HP)
        {
            set = transform.parent.parent.GetComponent<SetMonster>();
        }
        else
        {
            set = transform.parent.GetComponent<SetMonster>();
        }
        
        mon = set.monster;

        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();

        maxHp = mon.hp;    
    }

    void Update()
    {
        switch (type)
        {
        case TYPE.HP:
            image.fillAmount = mon.hp/maxHp;
            text.text = "Hp:"+mon.hp;
            break;
        case TYPE.ATTACK:
            text.text = ""+mon.attackPower;
            break;
        case TYPE.DEFENS:
            text.text = ""+mon.defensPower;
            break;
        case TYPE.SHELID:
            text.text = ""+mon.ondefensPower;
            break;
        }
    }
}