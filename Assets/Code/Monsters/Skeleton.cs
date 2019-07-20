using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : ShowMonster
{

    protected override void Start()
    {
        base.Start();
        OnMonsterDead.AddListener(GameManager.instance.OnGameEnd);
    }

    protected override void Attack()
    {
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Defens()
    {
        ondefensPower += defensPower;
    }

}
