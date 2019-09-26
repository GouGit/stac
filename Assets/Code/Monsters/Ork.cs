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
        skillUI.SetActive(true);
    }

    protected override void ChangeState()
    {
        isAttack = true;
        action = ACTION.ATTACK;
    }

    protected override void EndTurn()
    {
        attackPower += addPower;
        temPower = attackPower;
        base.EndTurn();
    }

    
}
