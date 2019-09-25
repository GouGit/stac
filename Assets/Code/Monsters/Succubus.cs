using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succubus : ShowMonster
{
    private int actionCount = 1;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ChangeState()
    {
        if(isAttack)
        {
            action = ACTION.ATTACK;
        }
        else
        {
            action = ACTION.DEFENS;
        }

        if(actionCount == 2)
        {
            Knight.instance.fCnt++;
            actionCount = 0;
            isAttack = false;
        }
        else
        {
            actionCount++;
            isAttack = true;
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

    protected override void Defens()
    {
        ondefensPower += defensPower;
        isAttack = true;
    }

}
