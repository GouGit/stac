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
    private int holyCnt;
    private int doubleCnt;
   
    protected override void Start()
    {
        base.Start();
        holyCnt = 0;
        doubleCnt = 2;
    }

    IEnumerator DoubleAttack(ShowMonster mon)
    {
        mon.LoseHp(attackPower);
        doubleCnt--;
        yield return new WaitForSeconds(0.5f);
        if(doubleCnt<=0)
        {
            StopCoroutine(DoubleAttack(mon));
            Knight.instance.Sort();
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(DoubleAttack(mon));
        }
    }

     IEnumerator Roll(ShowMonster mon,int num)
    {
        mon.LoseHp(attackPower);
        num--;
        yield return new WaitForSeconds(0.5f);
        if(num<=0)
        {
            StopCoroutine(Roll(mon,num));
            Knight.instance.Sort();
            gameObject.SetActive(false);
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
            attackPower = attackPower/2;
        }
        if(GameManager.instance.cost >= cost)
        {
            GameManager.instance.cost -= cost;
            if(Knight.instance.fCnt > 0)
            {
                Knight.instance.LoseHp(attackPower);
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
                break;
            case SKILL.POWER_ATTACK:
                monster.LoseHp(attackPower);
                Knight.instance.LoseHp(3);
                gameObject.SetActive(false);
                break;
            case SKILL.DOUBLE_SWORD:
                monster.LoseHp(Knight.instance.defensPower*2);
                Knight.instance.defensPower = 0;
                break;
            case SKILL.ROLL:
                StartCoroutine(Roll(monster ,GameManager.instance.cost));
                GameManager.instance.cost -= GameManager.instance.cost;
                break;
            case SKILL.HOLY_SWORD:
                holyCnt++;
                if(holyCnt >= 3)
                {
                    monster.LoseHp(monster.hp);
                }
                gameObject.SetActive(false);
                break;
            }
            monster.LoseHp(attackPower);

            SoundManager.Instance.PlaySFX(SoundManager.SFXList.KNIFE_1);
            Knight.instance.defensPower += defensPower;
            Knight.instance.Sort();
        }
    }

    
}
