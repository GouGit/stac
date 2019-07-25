using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public Canvas uiCanvas;
    private GameObject ui;
    protected GameObject hpUI, attackUI, defensUI, defensOnUI;
    protected Type.TYPE type;
    protected ACTION action;
    protected new string name;
    public int hp;
    public int attackPower;
    public int defensPower;
    public int ondefensPower = 0;
    protected bool isAttack;
    protected bool shaking = false;
    protected float shakePower;
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
        isAttack = true;

        uiCanvas.worldCamera = Camera.main;
        SetMonster set = uiCanvas.GetComponent<SetMonster>();
        set.Set(this);
        ui = Instantiate(uiCanvas.gameObject, Vector3.zero, Quaternion.identity);
        ui.SetActive(true);

        hpUI = ui.transform.GetChild(0).gameObject;
        hpUI.transform.position = transform.position + Vector3.down*2.5f;
        defensOnUI = ui.transform.GetChild(1).gameObject;
        defensOnUI.transform.position = transform.position + Vector3.down*2f + Vector3.left*1.5f;
        attackUI = ui.transform.GetChild(2).gameObject;
        attackUI.transform.position = transform.position + Vector3.down*2f + Vector3.right*1.5f;
        defensUI = ui.transform.GetChild(3).gameObject;
        defensUI.transform.position = transform.position + Vector3.down*2f + Vector3.right*1.5f;

        hpUI.SetActive(true);
        attackUI.SetActive(isAttack);
        defensUI.SetActive(!isAttack);

        OnMonsterDead.AddListener(GameManager.instance.OnGameEnd);
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }

    void MyTurn()
    {
        if(isAttack)
        {
            action = ACTION.ATTACK;
        }
        else
        {
            action = ACTION.DEFENS;
        }
        isAttack = !isAttack;
        
        attackUI.SetActive(isAttack);
        defensUI.SetActive(!isAttack);

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
            newpos.x = transform.position.x + newpos.x;
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
                shakePower = Mathf.Abs(ondefensPower);
                hp += ondefensPower;
                StartCoroutine(Shaking());
            }
        }
        else
        {
            shakePower = damage;
            hp -= damage;
            StartCoroutine(Shaking());
        }
        
        if(hp <= 0)
        {
            ui.gameObject.SetActive(false);
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
