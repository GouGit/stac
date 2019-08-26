using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : ShowMonster
{
    private int maxHp;

    protected override void Start()
    {
        base.Start();
        maxHp = hp;
    }

    protected override void Attack()
    {
        if(Knight.instance.isReflect)
        {
            LoseHp(attackPower);
        }
        int beforeHp = Knight.instance.ReturnHP();
        Knight.instance.LoseHp(attackPower);
        int playerHp = beforeHp - Knight.instance.ReturnHP();
        hp += playerHp;
        if(hp > maxHp)
        {
            hp = maxHp;
        }
    }


}
