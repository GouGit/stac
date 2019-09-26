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
        if(!isSkill)
        {
            actionCount++;
            if(actionCount >= 2)
            {
                isSkill = true;
                skillUI.SetActive(true);
                attackUI.SetActive(false);
                defensUI.SetActive(false);
            }
        }
        else
        {
            skillUI.SetActive(false);
            action = ACTION.SKILL;
            actionCount = 0;
            isSkill = false;
            return;
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

    protected override void Skill()
    {
        attackPower += addPower;
        temPower = attackPower;
        hp += addHp;
        maxHp += addHp;
        MonsterUI hpui = hpUI.GetComponentInChildren<MonsterUI>();
        hpui.maxHp = maxHp;
    }

}
