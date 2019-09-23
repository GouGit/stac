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

    protected override void OnMouseDown()
    {
        transform.localScale = scale * 1.25f;
        origin = transform.position;
        BezierDrawer.Instance.gameObject.SetActive(true);
        BezierDrawer.Instance.startPosition = gameObject.transform.position;
    }

    protected override void OnMouseUp()
    {
        transform.localScale = scale;
        UseCard();
        BezierDrawer.Instance.gameObject.SetActive(false);
    }

    IEnumerator DoubleAttack(ShowMonster mon)
    {
        mon.LoseHp(attackPower);
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
        mon.LoseHp(attackPower);
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
                SoundManager.Instance.PlaySFX(SoundManager.SFXList.DOUBLE_KNIFE);
                    break;
            case SKILL.POWER_ATTACK:
                monster.LoseHp(attackPower);
                Knight.instance.LoseHp(3);
                gameObject.SetActive(false);
                break;
            case SKILL.DOUBLE_SWORD:
                monster.LoseHp((Knight.instance.defensPower+attackPower)*2);
                Knight.instance.defensPower = 0;
                gameObject.SetActive(false);
                break;
            case SKILL.ROLL:
                StartCoroutine(Roll(monster ,GameManager.instance.cost+bonusCount));
                GameManager.instance.cost -= GameManager.instance.cost;
                break;
            case SKILL.HOLY_SWORD:
                holyCnt++;
                if(holyCnt >= 3)
                {
                    monster.LoseHp(monster.hp);
                }
                else
                {
                    monster.LoseHp(attackPower);
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

    
}
