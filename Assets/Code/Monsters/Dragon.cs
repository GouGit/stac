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
            if(Knight.instance.isReflect)
            {
                LoseHp(attackPower);
            }
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

}
