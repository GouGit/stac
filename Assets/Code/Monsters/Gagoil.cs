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
        isAttack = !isAttack;
    }

    protected override void Defens()
    {
        for(int i = 0; i < GameManager.instance.AllMonsters.Count; i++)
        {
            ShowMonster monster = GameManager.instance.AllMonsters[i].GetComponent<ShowMonster>();
            monster.ondefensPower += defensPower;
        }
        isAttack = !isAttack;
    }
}
