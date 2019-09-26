using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCard : ShowCard
{
    int maxLevel = 5;

    protected override void Start()
    {
        base.Start();
    }

    protected override void CardUpgrade()
    {
        switch (card.name)
        {
        case "준비된자세":
            for(int i = 0; i < level; i++)
            {
                if(level%2 != 0)
                    cardValue += card.upgradeValue;
                else
                    defensPower += card.upgradeExtra;
            }
            break;
        case "드로우":
            for(int i = 0; i < level; i++)
            {
                if(maxLevel == level)
                {
                    cost -= 1;
                    break;
                }
                if(level%2 != 0)
                    cardValue += card.upgradeValue;
                else
                    defensPower += card.upgradeExtra;
            }
            break;
        }
    }

    protected override void OnlyDefens()
    {
        if(origin.y + 2 <= transform.position.y)
        {
            if(GameManager.instance.cost >= cost)
            {
                GameManager.instance.cost -= cost;
                Knight.instance.Draw(cardValue);
                Knight.instance.defensPower += defensPower;
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
