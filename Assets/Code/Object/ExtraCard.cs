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
        SAVING
    }
    public SKILL skill;
    protected override void Start()
    {
        base.Start();
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
                    GameManager.instance.cost = Random.Range(1,5);
                    break;
                case SKILL.POWERUP:
                    Knight.instance.attackPower += 2;
                    break;
                case SKILL.PILL:
                    Knight.instance.bCnt = 0;
                    Knight.instance.fCnt = 0;
                    Knight.instance.pCnt = 0;
                    break;
                case SKILL.SAVING:
                    GameManager.instance.savingCost = 2;
                    break;
                }
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = origin;
            }
        }
        else
        {
            transform.position = origin;
        }
    }



}
