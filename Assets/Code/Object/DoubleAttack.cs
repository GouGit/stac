using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : CardManager
{
    private int cnt;

    void Start()
    {
        myBox = GetComponent<BoxCollider2D>();
        attackPower = 3;
        origin = transform.localScale;
        useCost = 1;
        cnt = 0;
        type = CardTYPE.SAPPHIRE;
    }

    protected override void CardAction(GameObject monster)
    {
        mon = monster.GetComponent<PlayObject>();
        AddPower();
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        ++cnt;    
        mon.LoseHp(attackPower);
        yield return new WaitForSeconds(0.2f);
        if(cnt < 2)
        {
            StartCoroutine(Attack());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        SelectCard();
    }

    void OnMouseUp()
    {
        DropCrad();
    }

}
