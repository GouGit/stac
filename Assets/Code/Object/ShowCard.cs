using System.Collections;
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

        this.showCard.level = upgradeLevel;
    }
}

public class ShowCard : MonoBehaviour
{
    public Card card;
    public int level;
    private new string name;
    protected bool IsAttack;
    protected int cost;
    public int cardValue;
    public int defensPower;
    public int powerUp;
    protected Vector3 scale, origin;
    protected BoxCollider2D myBox;
    protected Type.TYPE type, monsterType;


    protected virtual void Start()
    {
        GetComponent<SpriteRenderer>().sprite = card.image;
        name = card.name;
        cost = card.cost;
        IsAttack = card.IsAttack;
        cardValue = card.cardValue;
        defensPower = card.defensPower;
        type = card.type;
        scale = transform.localScale;
        origin = transform.position;
        myBox = GetComponent<BoxCollider2D>();
        CardUpgrade();
    }

    protected virtual void AddPower()
    {
        if(type == Type.TYPE.DIAMOND)
        {
            cardValue *= 2;
            return;
        }
        
        switch (monsterType)
        {
        case Type.TYPE.RUBY:
            if(type == Type.TYPE.SAPPHIRE)
            {
                cardValue *= 2;
            }
            else if(type == Type.TYPE.TOPAZ)
            {
                cardValue /=2;
            }
            break;
        case Type.TYPE.SAPPHIRE:
            if(type == Type.TYPE.TOPAZ)
            {
                cardValue *= 2;
            }
            else if(type == Type.TYPE.RUBY)
            {
                cardValue /=2;
            }
            break;
        case Type.TYPE.TOPAZ:
            if(type == Type.TYPE.RUBY)
            {
                cardValue *= 2;
            }
            else if(type == Type.TYPE.SAPPHIRE)
            {
                cardValue /=2;
            }
            break;
        case Type.TYPE.DIAMOND:
            cardValue *= 2;
            break;
        default:
            break;
        }       
    }

    protected virtual void CardUpgrade()
    {
        cardValue += card.upgradeValue * level;
        defensPower += card.upgradeExtra* level;
    }

    protected virtual void OnlyDefens()
    {
        if(origin.y + 2 <= transform.position.y)
        {
            if(GameManager.instance.cost >= cost)
            {
                GameManager.instance.cost -= cost;
                Knight.instance.defensPower += defensPower;
                gameObject.SetActive(false);
                Knight.instance.Sort();
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

    protected virtual void UseCard()
    {
        myBox.enabled = false;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 0.0f, 1<<8);
        if(hit.collider != null)
        {
            cardValue = cardValue + Knight.instance.attackPower;
            Using(hit.collider.gameObject);
        }
        myBox.enabled = true;
    }

    protected virtual void Using(GameObject ob)
    {
        if(Knight.instance.bCnt > 0)
        {
            cardValue = cardValue/2;
        }
        if(GameManager.instance.cost >= cost)
        {
            GameManager.instance.cost -= cost;
            if(Knight.instance.fCnt > 0)
            {
                Knight.instance.LoseHp(cardValue);
                gameObject.SetActive(false);
                Knight.instance.Sort();
                return;
            }
            ShowMonster monster = ob.GetComponent<ShowMonster>();
            monsterType = monster.mon.type;
            AddPower();
            monster.LoseHp(cardValue);
            SoundManager.Instance.PlaySFX(SoundManager.SFXList.KNIFE_1);
            Knight.instance.defensPower += defensPower;
            gameObject.SetActive(false);
            Knight.instance.Sort();
        }
    }

    protected virtual void OnMouseDown()
    {
        transform.localScale = scale * 1.25f;
        origin = transform.position;
        if(IsAttack)
        {
            BezierDrawer.Instance.gameObject.SetActive(true);
            BezierDrawer.Instance.startPosition = gameObject.transform.position; 
        }
    }

    protected virtual void OnMouseDrag()
    {
        if(!IsAttack)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            transform.position = pos;
        } 
    }

    protected virtual void OnMouseUp()
    {
        transform.localScale = scale;
        if(IsAttack)
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
