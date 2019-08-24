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
   
    protected override void Start()
    {
        base.Start();
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
            monster.LoseHp(attackPower);
            
            SoundManager.Instance.PlaySFX(SoundManager.SFXList.KNIFE_1);
            Knight.instance.defensPower += defensPower;
            gameObject.SetActive(false);
        }
    }

    
}
