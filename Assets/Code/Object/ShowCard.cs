using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCard : MonoBehaviour
{
    public Card card;
    private new string name;
    private int cost;
    private int attackPower;
    private int defensPower;
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
        switch (type)
        {
        case Type.TYPE.RUBY:
            if(monsterType == Type.TYPE.SAPPHIRE) // 불리
            {
                attackPower /= 2;
            }
            else if(monsterType == Type.TYPE.TOPAZ) // 유리
            {
                attackPower *=2;
            }
            break;
        case Type.TYPE.SAPPHIRE:
            if(monsterType == Type.TYPE.TOPAZ)
            {
                attackPower /= 2;
            }
            else if(monsterType == Type.TYPE.RUBY)
            {
                attackPower *=2;
            }
            break;
        case Type.TYPE.TOPAZ:
            if(monsterType == Type.TYPE.RUBY)
            {
                attackPower /= 2;
            }
            else if(monsterType == Type.TYPE.SAPPHIRE)
            {
                attackPower *=2;
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
            if(GameManager.instance.cost >= cost)
            {
                GameManager.instance.cost -= cost;
                ShowMonster monster = hit.collider.gameObject.GetComponent<ShowMonster>();
                monsterType = monster.mon.type;
                AddPower();
                monster.LoseHp(attackPower);
                Knight.instance.defensPower += defensPower;
                gameObject.SetActive(false);
            }
        }
        myBox.enabled = true;
    }

    void OnMouseDown()
    {
        transform.localScale = scale * 1.25f;
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
        }
        else
        {
            OnlyDefens();
        }
    }
}
