using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : CardManager
{

    void Start()
    {
        myBox = GetComponent<BoxCollider2D>();
        attackPower = 5;
        origin = transform.localScale;
        useCost = 1;
        type = CardTYPE.RUBY;
    }

    protected override void CardAction(GameObject monster)
    {
        mon = monster.GetComponent<PlayObject>();
        AddPower();
        mon.LoseHp(attackPower);
        gameObject.SetActive(false);
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
