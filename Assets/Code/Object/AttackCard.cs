using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : ShowCard
{
    public enum SKILL
    {
        DOUBLE_ATTACK,
        POWER_ATTACK,
        DOUBLE_SWORD,
        ROLL,
        HOLY_SWORD
    }
    public SKILL skill;
    private int doubleCnt;
    private int rollCnt;
   
    protected override void Start()
    {
        base.Start();
        doubleCnt = 2;
        rollCnt = card.upgradeExtra * level;
    }

    IEnumerator DoubleAttack(ShowMonster mon)
    {
        mon.LoseHp(cardValue);
        doubleCnt--;
        yield return new WaitForSeconds(0.25f);
        if(doubleCnt<=0)
        {
            StopCoroutine(DoubleAttack(mon));
            gameObject.SetActive(false);
            Knight.instance.Sort();
        }
        else
        {
            StartCoroutine(DoubleAttack(mon));
        }
    }

     IEnumerator Roll(ShowMonster mon,int num)
    {
        mon.LoseHp(cardValue);
        num--;
        yield return new WaitForSeconds(0.25f);
        if(num<=0)
        {
            StopCoroutine(Roll(mon,num));
            gameObject.SetActive(false);
            Knight.instance.Sort();
        }
        else
        {
            StartCoroutine(Roll(mon,num));
        }
    }

    protected override void Using(GameObject ob)
    {
        if(Knight.instance.bCnt > 0)
        {
            cardValue = cardValue/2;
        }
        if(Knight.instance.fCnt > 0)
        {
            Knight.instance.LoseHp(cardValue);
            gameObject.SetActive(false);
            return;
        }
        ShowMonster monster = ob.GetComponent<ShowMonster>();
        monsterType = monster.mon.type;
        AddPower();
        switch (skill)
        {
        case SKILL.DOUBLE_ATTACK:
            StartCoroutine(DoubleAttack(monster));
            SoundManager.Instance.PlaySFX(SoundManager.SFXList.DOUBLE_KNIFE);
            break;
        case SKILL.POWER_ATTACK:
            monster.LoseHp(cardValue);
            Knight.instance.LoseHp(3);
            gameObject.SetActive(false);
            break;
        case SKILL.DOUBLE_SWORD:
            monster.LoseHp((Knight.instance.defensPower+cardValue)*2);
            Knight.instance.defensPower = 0;
            gameObject.SetActive(false);
            break;
        case SKILL.ROLL:
            StartCoroutine(Roll(monster ,GameManager.instance.cost+rollCnt));
            GameManager.instance.cost -= GameManager.instance.cost;
            break;
        case SKILL.HOLY_SWORD:
            GameManager.instance.holyCnt++;
            if(GameManager.instance.holyCnt >= 2)
            {
                GameManager.instance.holyCnt = 0;
                monster.LoseHp(monster.hp);
            }
            else
            {
                monster.LoseHp(cardValue);
            }
            gameObject.SetActive(false);
            break;
        }

        Knight.instance.defensPower += defensPower;
        if(skill != SKILL.DOUBLE_ATTACK)
            SoundManager.Instance.PlaySFX(SoundManager.SFXList.KNIFE_1);
        Knight.instance.Sort();
    }

    
}
