using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCard : ShowCard
{
    public enum SKILL
    {
        DRAW,
        DEFENS_DRAW,

    }
    public SKILL skill;
    private int drawNum;

    protected override void Start()
    {
        base.Start();
        if(skill == SKILL.DRAW)
        {
            drawNum = 2;
        }
        else
        {
            drawNum = 1;
        }
    }

    protected override void OnlyDefens()
    {
        if(origin.y + 2 <= transform.position.y)
        {
            if(GameManager.instance.cost >= cost)
            {
                GameManager.instance.cost -= cost;
                Knight.instance.Draw(drawNum);
                Knight.instance.defensPower += defensPower;
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
