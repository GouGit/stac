using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : ShowMonster
{
    private bool isHalf = false;
    private int halfHp;

    protected override void Start()
    {
        base.Start();
        halfHp = hp/2;
    }

    public override void LoseHp(int damage)
    {
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
        
        if(halfHp >= hp && !isHalf)
        {
            isHalf = true;
            isAttack = true;
            isSkill = true;
            attackUI.SetActive(isAttack);
            defensUI.SetActive(!isAttack);
            skillUI.SetActive(true);
        }
        
        if(hp <= 0)
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

    protected override void Skill()
    {
        Knight.instance.isPetrification = true;
        skillUI.SetActive(false);
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
