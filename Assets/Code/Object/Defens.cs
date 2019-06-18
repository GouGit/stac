using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defens : CardManager
{
    private Vector2 originPos, usePos;
    
    void Start()
    {
        myBox = GetComponent<BoxCollider2D>();
        defensPower = 5;
        origin = transform.localScale;
        originPos = transform.position;
        useCost = 1;
    }

    void OnMouseDown()
    {
        SelectCard();
    }

    void OnMouseDrag()
    {
        usePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = usePos;
    }

    protected override void UseCard()
    {
        if(Mathf.Abs(originPos.y - transform.position.y) >= 2.0f && GameManager.instance.cost >= useCost)
        {
            GameManager.instance.cost -= useCost;
            Player.inst.defens += defensPower;
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = originPos;
            transform.localScale = origin;
        }
    }

    void OnMouseUp()
    {
        UseCard();
    }

}
