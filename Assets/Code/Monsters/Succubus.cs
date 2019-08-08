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
        if(isAttack)
        {
            action = ACTION.ATTACK;
        }
        else
        {
            action = ACTION.DEFENS;
        }
        actionCount++;
    }

    protected override void Attack()
    {
        if(actionCount == 2)
        {
            Knight.instance.fCnt++;
            actionCount = 0;
            isAttack = false;
        }
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Defens()
    {
        ondefensPower += defensPower;
        isAttack = true;
    }

}
