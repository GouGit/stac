using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gagoil : ShowMonster
{
    protected override void Start()
    {
        base.Start();
        isAttack = false;
    }

    protected override void Attack()
    {
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Defens()
    {
        for(var node = MonsterOption.AllMonsters.First; node != null; node = node.Next)
        {
            ShowMonster monster = node.Value.GetComponent<ShowMonster>();
            monster.ondefensPower += defensPower;
        }
    }
}
