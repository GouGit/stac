using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frankenstein : ShowMonster
{
    private int actionCount = 0;
    private int maxHp;
    private int addPower, addHp;

    protected override void Start()
    {
        base.Start();
        maxHp = hp;
        addPower = 3;
        addHp = 10;
    }

    protected override void ChangeState()
    {
        actionCount++;
        if(actionCount == 3)
        {
            attackPower += addPower;
            hp += addHp;
            maxHp += addHp;
            MonsterUI hpui = hpUI.GetComponentInChildren<MonsterUI>();
            hpui.maxHp = maxHp;
            actionCount = 0;
        }
        base.ChangeState();
    }

}
