using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : ShowMonster
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Defens()
    {
        
    }

}
