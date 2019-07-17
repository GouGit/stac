using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMonster : MonoBehaviour
{
    private enum ACTION
    {
        NONE,
        ATTACK,
        DEFENS,
        END
    }
    public Monster mon;
    private new string name;
    private int hp;
    private int attackPower;
    private int defensPower;
    private Type.TYPE type;
    private ACTION action;
    private bool isAttack = true;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = mon.image;
        name = mon.name;
        hp = mon.hp;
        attackPower = mon.attackPower;
        defensPower = mon.defensPower;
        type = mon.type;
        action = ACTION.NONE;
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }

    void MyTurn()
    {
        Vector3 scale = new Vector3(1,1,1);
        transform.localScale = scale;
    }

    void EndTurn()
    {
        action = ACTION.NONE;
        GameManager.instance.isPlayerTurn = true;
        GameManager.instance.cost = 3;
        Knight.instance.defensPower = 0;
        Knight.instance.DrawCard();
        Vector3 scale = new Vector3(0.8f,0.8f,1);
        transform.localScale = scale;
    }

    public void LoseHp(int damage)
    {
        if(action == ACTION.DEFENS)
        {
            int defens = defensPower - damage;
            defensPower = defens;
            if(defensPower < 0)
            {
                hp -= defensPower;
            }
        }
        else
        {
            hp -= damage;
        }

        Debug.Log(hp);
        
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(GameManager.instance.isPlayerTurn)
            return;
        
        switch (action)
        {
        case ACTION.NONE:
            MyTurn();
            if(isAttack)
            {
                action = ACTION.ATTACK;
                isAttack = false;
            }
            else
            {
                action = ACTION.DEFENS;
                isAttack = true;
            }
            break;
        case ACTION.ATTACK:
            Knight.instance.LoseHp(attackPower);
            action = ACTION.END;
            break;
        case ACTION.DEFENS:
            action = ACTION.END;
            break;
        case ACTION.END:
            StartCoroutine(WaitTime());
            break;
        }
    }
}
