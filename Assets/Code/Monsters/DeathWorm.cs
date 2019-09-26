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
        if(!isSkill)
        {
            actionCount++;
            if(actionCount >= randomAction-1)
            {
                isSkill = true;
                skillUI.SetActive(true);
                attackUI.SetActive(false);
                defensUI.SetActive(false);
            }
        }
        else
        {
            skillUI.SetActive(false);
            action = ACTION.SKILL;
            actionCount = 0;
            isSkill = false;
            return;
        }

        if(isAttack)
        {
            action = ACTION.ATTACK;
        }
        else
        {
            action = ACTION.DEFENS;
        }
        isAttack = !isAttack;
    }

    protected override void Skill()
    {
        randomAction = Random.Range(2,4);
        Knight.instance.defensPower = 0;
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Attack()
    {
        if(Knight.instance.isReflect)
        {
            LoseHp(attackPower);
        }
        Knight.instance.LoseHp(attackPower);
    }
    
}
