using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : PlayObject
{
    enum MYSTATE
    {
        NONE,
        ATTACK,
        DEFENS,
        END
    }
    private MYSTATE my;
    protected bool isAttack; //공격여부 (플레이어 정의X)
    public ShowMotion attack, defens;

    void Awake()
    {
        GameObject StateUI = GameObject.Find("StateUI");
        attack = StateUI.transform.GetChild(2).GetComponent<ShowMotion>();
        defens = StateUI.transform.GetChild(3).GetComponent<ShowMotion>();
        DefensUI = StateUI.transform.GetChild(4).GetComponent<OnDefens>();

        StateUI.transform.GetChild(0).transform.GetChild(1).GetComponent<Hpbar>().myObject = this;

        hp = 40;
        attackPower = 5;
        isAttack = true;
        my = MYSTATE.NONE;
        type = TYPE.TOPAZ;
        
        attack.SetAttack(attackPower);
        defens.SetDefens(5);
    }

    void Update()
    {
        if(GameManager.instance.playerTurn)
            return;

        Action();
    }

    protected override void Action()
    {
        switch (my)
        {
        case MYSTATE.NONE:
            MyTurn();
            if(isAttack)
            {
                my = MYSTATE.ATTACK;
                isAttack = false;
                attack.gameObject.SetActive(false);
                defens.gameObject.SetActive(true);
            }
            else
            {
                my = MYSTATE.DEFENS;
                isAttack = true;
                attack.gameObject.SetActive(true);
                defens.gameObject.SetActive(false);
            }
            break;
        case MYSTATE.ATTACK:
            Player.inst.LoseHp(attackPower);
            my = MYSTATE.END;
            StartCoroutine(WaitTime());
            break;
        case MYSTATE.DEFENS:
            defensPower = 5;
            DefensUI.SetDefens(defensPower);
            DefensUI.gameObject.SetActive(true);
            my = MYSTATE.END;
            StartCoroutine(WaitTime());
            break;
        case MYSTATE.END:
            break;
        }
    }

    protected override void MyTurn()
    {
        base.MyTurn();
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }

    protected override void Dead()
    {
        Destroy(attack.gameObject);
        Destroy(defens.gameObject);
        Destroy(DefensUI.gameObject);
        Destroy(gameObject);
    }

    public override void EndTurn()
    {
        my = MYSTATE.NONE;
        GameManager.instance.playerTurn = true;
        GameManager.instance.cost = 3;
        Player.inst.DrawCard();
        Player.inst.defens = 0;
        Vector3 scale = transform.localScale;
        scale.x = 0.8f;
        scale.y = 0.8f;
        transform.localScale = scale;
    }

}
