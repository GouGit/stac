using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : ShowMonster
{
    private int monsterCnt;
    private int addPower, addDefens, addHp;
    private int maxHp;

    protected override void Start()
    {
        base.Start();
        addPower = 3;
        addHp = 10;
        addDefens = 2;
        maxHp = hp;
        monsterCnt = GameManager.instance.monsterOption.AllMonsters.Count;
    }

    protected override void ChangeState()
    {
        if(monsterCnt != GameManager.instance.monsterOption.AllMonsters.Count)
        {
            attackPower += addPower;
            defensPower += addDefens;
            hp += addHp;
            maxHp += addHp;
            MonsterUI hpui = hpUI.GetComponentInChildren<MonsterUI>();
            hpui.maxHp = maxHp;
            monsterCnt = GameManager.instance.monsterOption.AllMonsters.Count;
        }
        
        if(isAttack)
        {
            action = ACTION.ATTACK;
        }
        else
        {
            action = ACTION.DEFENS;
        }
        isAttack = !isAttack;
    }

}
