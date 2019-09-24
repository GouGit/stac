using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ork : ShowMonster
{

    private int addPower;
    
    protected override void Start()
    {
        base.Start();
        addPower = 2;
    }

    protected override void ChangeState()
    {
        isAttack = true;
        action = ACTION.ATTACK;
    }

    protected override void EndTurn()
    {
        base.attackPower += addPower;
        base.EndTurn();
    }

    
}
