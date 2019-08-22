﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardSet
{
    public ShowCard showCard;
    public int upgradeLevel;

    public CardSet(ShowCard showCard, int upgradeLevel)
    {
        this.showCard = showCard;
        this.upgradeLevel = upgradeLevel;
    }
}

public class ShowCard : MonoBehaviour
{
    public Card card;
    private new string name;
    private int cost;
    public int attackPower;
    public int defensPower;
    private Vector3 scale, origin;
    private BoxCollider2D myBox;
    private Type.TYPE type, monsterType;


    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = card.image;
        name = card.name;
        cost = card.cost;
        attackPower = card.attackPower;
        defensPower = card.defensPower;
        scale = transform.localScale;
        type = card.type;
        origin = transform.position;
        myBox = GetComponent<BoxCollider2D>();
    }

    private void AddPower()
    {
        if(type == Type.TYPE.DIAMOND)
        {
            attackPower *= 2;
            return;
        }
        
        switch (monsterType)
        {
        case Type.TYPE.RUBY:
            if(type == Type.TYPE.SAPPHIRE)
            {
                attackPower *= 2;
            }
            else if(type == Type.TYPE.TOPAZ)
            {
                attackPower /=2;
            }
            break;
        case Type.TYPE.SAPPHIRE:
            if(type == Type.TYPE.TOPAZ)
            {
                attackPower *= 2;
            }
            else if(type == Type.TYPE.RUBY)
            {
                attackPower /=2;
            }
            break;
        case Type.TYPE.TOPAZ:
            if(type == Type.TYPE.RUBY)
            {
                attackPower *= 2;
            }
            else if(type == Type.TYPE.SAPPHIRE)
            {
                attackPower /=2;
            }
            break;
        case Type.TYPE.DIAMOND:
            attackPower *= 2;
            break;
        default:
            break;
        }       
    }

    private void OnlyDefens()
    {
        if(origin.y + 2 <= transform.position.y)
        {
            if(GameManager.instance.cost >= cost)
            {
                GameManager.instance.cost -= cost;
                Knight.instance.defensPower += defensPower;
                gameObject.SetActive(false);
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

    private void UseCard()
    {
        myBox.enabled = false;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 0.0f, 1<<8);
        if(hit.collider != null)
        {
            Using(hit.collider.gameObject);
        }
        myBox.enabled = true;
    }

    void Using(GameObject ob)
    {
        if(Knight.instance.bCnt > 0)
        {
            attackPower = attackPower/2;
        }
        if(GameManager.instance.cost >= cost)
        {
            GameManager.instance.cost -= cost;
            if(Knight.instance.fCnt > 0)
            {
                Knight.instance.LoseHp(attackPower);
                gameObject.SetActive(false);
                return;
            }
            ShowMonster monster = ob.GetComponent<ShowMonster>();
            monsterType = monster.mon.type;
            AddPower();
            monster.LoseHp(attackPower);
            SoundManager.Instance.PlaySFX(SoundManager.SFXList.KNIFE_1);
            Knight.instance.defensPower += defensPower;
            gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        transform.localScale = scale * 1.25f;
        if(attackPower > 0)
        {
            BezierDrawer.Instance.gameObject.SetActive(true);
            BezierDrawer.Instance.startPosition = gameObject.transform.position; 
        }
    }

    void OnMouseDrag()
    {
        if(attackPower <= 0)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            transform.position = pos;
        } 
    }

    void OnMouseUp()
    {
        transform.localScale = scale;
        if(attackPower > 0)
        {
            UseCard();
            BezierDrawer.Instance.gameObject.SetActive(false);
        }
        else
        {
            OnlyDefens();
        }
    }
}
