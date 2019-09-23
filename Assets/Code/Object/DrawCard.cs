using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCard : ShowCard
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnlyDefens()
    {
        if(origin.y + 2 <= transform.position.y)
        {
            if(GameManager.instance.cost >= cost)
            {
                GameManager.instance.cost -= cost;
                Knight.instance.Draw(drawCount);
                Knight.instance.defensPower += defensPower;
                gameObject.SetActive(false);
                Knight.instance.Sort();
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
