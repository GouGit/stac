using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWorm : ShowMonster
{ 
    private int randomAction;
    private int actionCount = 0;

    protected override void Start()
    {
        base.Start();
        randomAction = Random.Range(2,4);
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
        isAttack = !isAttack;
        actionCount++;
    }

    protected override void Attack()
    {
        if(actionCount >= randomAction)
        {
            actionCount = 0;
            randomAction = Random.Range(2,4);
            int addPower = Knight.instance.defensPower;
            Knight.instance.LoseHp(attackPower + addPower);
        }
        else
        {
            Knight.instance.LoseHp(attackPower);
        }
    }
    
}
