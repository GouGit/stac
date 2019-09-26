using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succubus : ShowMonster
{
    private int actionCount = 0;

    protected override void Start()
    {
        base.Start();
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
    }

    protected override void Attack()
    {
        if(Knight.instance.isReflect)
        {
            LoseHp(attackPower);
        }
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Skill()
    {
        Knight.instance.fCnt++;
        isAttack = false;
    }

    protected override void Defens()
    {
        ondefensPower += defensPower;
        isAttack = true;
    }

}
