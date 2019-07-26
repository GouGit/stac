using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gagoil : ShowMonster
{
    protected override void Start()
    {
        base.Start();
        isAttack = false;
        attackUI.SetActive(isAttack);
        defensUI.SetActive(!isAttack);
    }

    protected override void Attack()
    {
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Defens()
    {
        for(var node = GameManager.instance.monsterOption.AllMonsters.First; node != null; node = node.Next)
        {
            ShowMonster monster = node.Value.GetComponent<ShowMonster>();
            monster.ondefensPower += defensPower;
        }
    }
}
