using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public GameObject hpUI, attackUI, defensUI, defensOnUI;
    protected Type.TYPE type;
    protected ACTION action;
    protected new string name;
    public int hp;
    public int attackPower;
    public int defensPower;
    public int ondefensPower = 0;
    protected bool isAttack = true;
    protected bool shaking = false;
    protected float shakePower = 5;
    public UnityEvent OnMonsterDead;

    protected virtual void Start()
    {
        GetComponent<SpriteRenderer>().sprite = mon.image;
        name = mon.name;
        hp = mon.hp;
        attackPower = mon.attackPower;
        defensPower = mon.defensPower;
        type = mon.type;
        action = ACTION.NONE;

        hpUI.SetActive(true);
        if(isAttack)
        {
            attackUI.SetActive(true);
            defensUI.SetActive(false);
        }
        else
        {
            attackUI.SetActive(false);
            defensUI.SetActive(true);
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }

    void MyTurn()
    {
        if(!isAttack)
        {
            attackUI.SetActive(true);
            defensUI.SetActive(false);
        }
        else
        {
            attackUI.SetActive(false);
            defensUI.SetActive(true);
        }
        ondefensPower = 0;
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

    void Shake()
    {
        if(shaking)
        {
            Vector3 newpos = Random.insideUnitSphere * (Time.deltaTime * shakePower);
            newpos.y = transform.position.y;
            newpos.z = transform.position.z;
            transform.position = newpos;
        }

        if(ondefensPower > 0)
        {
            defensOnUI.SetActive(true);
        }
        else
        {
            defensOnUI.SetActive(false);
        }
    }

    IEnumerator Shaking()
    {
        Vector3 origin = transform.position;

        if(!shaking)
        {
            shaking = true;
        }

        yield return new WaitForSeconds(0.5f);

        shaking = false;
        transform.position = origin;
    }

    public void LoseHp(int damage)
    {
        if(ondefensPower > 0)
        {
            int defens = ondefensPower - damage;
            ondefensPower = defens;
            if(ondefensPower < 0)
            {
                hp -= ondefensPower;
                StartCoroutine(Shaking());
            }
        }
        else
        {
            hp -= damage;
            StartCoroutine(Shaking());
        }
        
        if(hp <= 0)
        {
            hpUI.SetActive(false);
            attackUI.SetActive(false);
            defensOnUI.SetActive(false);
            defensUI.SetActive(false);
            gameObject.SetActive(false);
            OnMonsterDead?.Invoke();
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
        Shake();

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
