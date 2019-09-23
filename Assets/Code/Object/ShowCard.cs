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
    }
}

public class ShowCard : MonoBehaviour
{
    public Card card;
    private new string name;
    protected int cost;
    public int attackPower;
    public int defensPower;
    public int bonusCount;
    public int fire;
    public int poision;
    public int lighting;
    public int drawCount;
    public int plusCost;
    public int minCost;
    public int powerUp;
    protected Vector3 scale, origin;
    protected BoxCollider2D myBox;
    protected Type.TYPE type, monsterType;


    protected virtual void Start()
    {
        GetComponent<SpriteRenderer>().sprite = card.image;
        name = card.name;
        cost = card.cost;
        attackPower = card.attackPower;
        defensPower = card.defensPower;
        bonusCount = card.bonusCount;
        fire = card.fire;
        poision = card.poision;
        lighting = card.lighting;
        drawCount = card.drawCount;
        plusCost = card.plusCost;
        minCost = card.minCost;
        powerUp = card.powerUp;
        type = card.type;
        scale = transform.localScale;
        origin = transform.position;
        myBox = GetComponent<BoxCollider2D>();
    }

    protected virtual void AddPower()
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
            attackPower = attackPower + Knight.instance.attackPower;
            Using(hit.collider.gameObject);
        }
        myBox.enabled = true;
    }

    protected virtual void Using(GameObject ob)
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
                Knight.instance.Sort();
                return;
            }
            ShowMonster monster = ob.GetComponent<ShowMonster>();
            monsterType = monster.mon.type;
            AddPower();
            monster.LoseHp(attackPower);
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
        if(attackPower > 0)
        {
            BezierDrawer.Instance.gameObject.SetActive(true);
            BezierDrawer.Instance.startPosition = gameObject.transform.position; 
        }
    }

    protected virtual void OnMouseDrag()
    {
        if(attackPower <= 0)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            transform.position = pos;
        } 
    }

    protected virtual void OnMouseUp()
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
