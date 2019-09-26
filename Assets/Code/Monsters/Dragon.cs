using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : ShowMonster
{
    private bool isFly;
    private int actionCount;
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
        SoundManager.Instance.PlaySFX(SoundManager.SFXList.MONSTER_DAMAGE);

        if (hp <= 0)
        {
            if (Knight.instance.HP <= 0)
                return;
            StartCoroutine(CO_DISSOLVE(GetComponent<SpriteRenderer>(), "_Edges", 1));
            Destroy(lightingParticle);
            Destroy(fireParticle);
            Destroy(poisionParticle);
            ui.gameObject.SetActive(false);
        }
    }

    protected override void ChangeState()
    {
        if(!isSkill)
        {
            actionCount++;
            if(actionCount >= 2)
            {
                isSkill = true;
                isFly = true;
                tempPower = attackPower;
                attackPower = tempPower * 3;
                skillUI.SetActive(true);
                attackUI.SetActive(false);
                defensUI.SetActive(false);
            }
        }
        else
        {
            skillUI.SetActive(false);
            action = ACTION.SKILL;
            attackPower = tempPower;
            actionCount = 0;
            isFly = false;
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
        Knight.instance.LoseHp(attackPower);
    }

    protected override void Attack()
    {
        if(Knight.instance.isReflect)
        {
            LoseHp(attackPower);
        }
        Knight.instance.LoseHp(attackPower);
        Knight.instance.bCnt = 1;
    }

}
