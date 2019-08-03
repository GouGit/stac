using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : ShowMonster
{
    private bool isFly;
    private int actionCnt;
    private int tempPower;
    
    protected override void Start()
    {
        base.Start();
        attackUI.SetActive(isAttack);
        defensUI.SetActive(!isAttack);
    }

    public override void LoseHp(int damage)
    {
        if(isFly)
            return;
        
        if(ondefensPower > 0)
        {
            int defens = ondefensPower - damage;
            ondefensPower = defens;
            if(ondefensPower < 0)
            {
                shakePower = Mathf.Abs(ondefensPower);
                hp += ondefensPower;
                StartCoroutine(Shaking());
            }
        }
        else
        {
            shakePower = damage;
            hp -= damage;
            StartCoroutine(Shaking());
        }
        
        if(hp <= 0)
        {
            ui.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    protected override void Attack()
    {
        if(isFly)
        {
            Knight.instance.LoseHp(attackPower);
            actionCnt = 0;
            isFly = false;
            attackPower = tempPower;
        }
        else
        {    
            Knight.instance.LoseHp(attackPower);
            Knight.instance.bCnt = 1;
            actionCnt++;
            if(actionCnt >= 3)
            {
                isFly = true;
                tempPower = attackPower;
                attackPower = tempPower * 3;
            }
        }
    }

    protected override void Defens()
    {
        ondefensPower += defensPower;
    }

}
