using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rich : ShowMonster
{

    private int actionCount = 0;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ChangeState()
    {
        actionCount++;
        if(actionCount == 5)
        {
            for(var node = GameManager.instance.monsterOption.AllMonsters.First; node != null; node = node.Next)
            {
                if(!node.Value.activeSelf)
                {
                    ShowMonster monster = node.Value.GetComponent<ShowMonster>();
                    monster.hp = hp/2;
                    monster.gameObject.SetActive(true);
                    monster.ui.SetActive(true);
                    actionCount = 0;
                    action = ACTION.END;
                    StartCoroutine(WaitTime());
                    return;
                }
            }
        }
        if(isAttack)
        {
            action = ACTION.ATTACK;
        }
        else
        {
            action = ACTION.DEFENS;
        }
        isAttack = !isAttack;
    }


}
