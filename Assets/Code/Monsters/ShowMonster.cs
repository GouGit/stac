using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShowMonster : MonoBehaviour
{
    public Monster mon;
    protected enum ACTION
    {
        NONE,
        ATTACK,
        DEFENS,
        END
    }
    protected Type.TYPE type;
    protected ACTION action;
    protected new string name;
    protected int hp;
    protected int attackPower;
    protected int defensPower;
    protected bool isAttack = true;

    protected virtual void Start()
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
        
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Defens()
    {

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
            Attack();
            action = ACTION.END;
            StartCoroutine(WaitTime());
            break;
        case ACTION.DEFENS:
            Defens();
            action = ACTION.END;
            StartCoroutine(WaitTime());
            break;
        case ACTION.END:
            break;
        }
    }
}
