using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCard : ShowCard
{
    public enum SKILL
    {
        POWERUP,
        GAMBLE,
        PILL,
        SAVING,
        REFLECT
    }
    public SKILL skill;
    protected override void Start()
    {
        base.Start();
    }

    protected override void CardUpgrade()
    {
        switch (card.name)
        {
        case "이자":
            for(int i = 0; i < level; i++)
            {
                if(level%2 == 0)
                    cardValue += card.upgradeValue;
                else
                    defensPower += card.upgradeExtra;
            }
            break;
        case "겜블":
            for(int i = 0; i < level; i++)
            {
                if(level%2 == 0)
                    cardValue += card.upgradeValue;
                else
                    defensPower += card.upgradeExtra;
            }
            break;
        default:
            cardValue += card.upgradeValue * level;
            defensPower += card.upgradeExtra* level;
            break;
        }
    }

    protected  override void OnlyDefens()
    {
        if(origin.y + 2 <= transform.position.y)
        {
            if(GameManager.instance.cost >= cost)
            {
                GameManager.instance.cost -= cost;
                switch (skill)
                {
                case SKILL.GAMBLE:
                    GameManager.instance.cost += Random.Range(cardValue,5);
                    Knight.instance.defensPower += defensPower;
                    break;
                case SKILL.POWERUP:
                    Knight.instance.attackPower += cardValue;
                    break;
                case SKILL.PILL:
                    Knight.instance.bCnt = 0;
                    Knight.instance.fCnt = 0;
                    Knight.instance.pCnt = 0;
                    Knight.instance.defensPower += defensPower;
                    break;
                case SKILL.SAVING:
                    GameManager.instance.savingCost += cardValue;
                    Knight.instance.defensPower += defensPower;
                    break;
                case SKILL.REFLECT:
                    Knight.instance.isReflect = true;
                    Knight.instance.defensPower += defensPower;
                    break;
                }
                gameObject.SetActive(false);
                Knight.instance.Sort();
            }
            else
            {
                LowerCost();
                transform.position = origin;
            }
        }
    }

}
