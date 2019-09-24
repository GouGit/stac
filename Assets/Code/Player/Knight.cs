using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    public Player player;
    public static Knight instance = null;
    public Canvas playerUI;
    public int bCnt, fCnt, pCnt; //화상, 매혹, 석화
    public bool isPetrification = false, isReflect = false;
    public int defensPower, attackPower;
    public int usingCard, usedCard;
    private  LinkedList<CardSet> MyCard = new LinkedList<CardSet>();
    private LinkedList<CardSet> HandCard = new LinkedList<CardSet>();
    private LinkedList<CardSet> TrashCard = new LinkedList<CardSet>();
    private GameObject showCard, defensUI;
    private GameObject stateUI, deadUI;
    private Image hpbar;
    private int hp, maxhp;
    public int HP { get => hp; }
    public int MaxHP { get => maxhp; }
    private string playerName;
    private FadeUI hitUI;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        hp = player.hp;
        maxhp = player.maxHp;
        defensPower = player.defensPower;
        playerName = player.name;
        playerUI.worldCamera = Camera.main;
        GameObject temp = Instantiate(playerUI.gameObject, Vector3.zero, Quaternion.identity);
        temp.SetActive(true);
        hpbar = temp.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        defensUI = temp.transform.GetChild(1).gameObject;
        stateUI = temp.transform.GetChild(2).gameObject;
        hitUI = temp.transform.GetChild(4).gameObject.GetComponent<FadeUI>();
        deadUI = temp.transform.GetChild(5).gameObject;
        
        for(int i = 0; i < GameManager.instance.AllCards.Count; i++)
        {
            MyCard.AddLast(GameManager.instance.AllCards[i]);
        }
        Shuffle();
        DrawCard();
        MyTurn();

        GameManager.instance.cost = 3;
        GameManager.instance.isPlayerTurn = true;
        hpbar.fillAmount = (float)hp/maxhp;
    }

    void Show()
    {
        showCard = new GameObject("Card");
        int i = -HandCard.Count/2;
        for(var node = HandCard.First; node != null; node = node.Next)
        {
            GameObject temp;
            node.Value.showCard.level = node.Value.upgradeLevel;
            temp = Instantiate(node.Value.showCard.gameObject, new Vector3(i * 2, -3.0f, 0), Quaternion.identity);
            temp.gameObject.SetActive(true);
            temp.transform.position = temp.transform.position + new Vector3(0, 0, -i + 10);
            temp.transform.SetParent(showCard.transform);
            i += 1;
        }
    }

    public void Sort()
    {
        int num = 0;
        int onActive = 0;
        for(int i = 0; i<showCard.transform.childCount; i++)
        {
            if(showCard.transform.GetChild(i).gameObject.activeSelf)
            {
                num++;
            }
        }
        
        num = num*-1;
        for(int i = 0; i<showCard.transform.childCount; i++)
        {
            if(showCard.transform.GetChild(i).gameObject.activeSelf)
            {
                showCard.transform.GetChild(i).localPosition = new Vector3(num + 2*onActive, -3.0f, -i);
                onActive++;   
            }
        }
    }

    void Shuffle()
    {
        List<CardSet> result = new List<CardSet>();

        for(var node = MyCard.First; node != null; node = node.Next)
        {
            result.Add(node.Value);
        }

        for(int i= 0; i < result.Count; i++)
        {
            CardSet temp = result[i];
            int index = Random.Range(0,MyCard.Count);
            result[i] = result[index];
            result[index] = temp;
        }

        MyCard.Clear();
        for(int i = 0; i < result.Count; i++)
        {
            MyCard.AddLast(result[i]);
        }
    }

    public void Draw(int num)
    {
        for(int i = 1; i<=num; i++)
        {
            HandCard.AddFirst(MyCard.First.Value);

            GameObject temp;
            temp = Instantiate(MyCard.First.Value.showCard.gameObject, new Vector3(0, -3.0f, 0), Quaternion.identity);
            temp.SetActive(true);
            temp.transform.SetParent(showCard.transform);

            MyCard.RemoveFirst();

            if(MyCard.Count == 0)
                ReBulid();
        }
        usingCard = MyCard.Count;
        usedCard = TrashCard.Count;
    }

    public void DrawCard()
    {
        if(HandCard.Count > 0)
            return;
        for(int i = 0; i < 5; i++)
        {
            HandCard.AddFirst(MyCard.First.Value);
            MyCard.RemoveFirst();

            if(MyCard.Count == 0)
                ReBulid();
        }
        usingCard = MyCard.Count;
        usedCard = TrashCard.Count;
        Show();
    }

    void DropCard()
    {
        for(int i = 0; i < showCard.transform.childCount; i++)
        {
           TrashCard.AddFirst(HandCard.First.Value);
           HandCard.RemoveFirst();
        }
        HandCard.Clear();
        Destroy(showCard);
    }

    void ReBulid()
    {
        for(int i=0;i<TrashCard.Count;i++)
        {
            MyCard.AddFirst(TrashCard.First.Value);
            TrashCard.RemoveFirst();
        }
        Shuffle();   
    }

    private void StateCheck()
    {
        if(bCnt > 0)
        {
            LoseHp(bCnt);
            bCnt--;
        }
        if(fCnt > 0)
        {
            fCnt--;
        }
        if(isPetrification)
        {
            pCnt++;
            if(pCnt >= 3)
            {
                LoseHp(hp);
            }
        }
    }

    public void CheckDebuff()
    {
        if(bCnt > 0)
        {
            stateUI.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            stateUI.transform.GetChild(0).gameObject.SetActive(false);
        }

        if(fCnt > 0)
        {
            stateUI.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            stateUI.transform.GetChild(1).gameObject.SetActive(false);
        }

        if(isPetrification)
        {
            stateUI.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            stateUI.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void MyTurn()
    {
        if(!defensUI.activeSelf)
        {
            if(defensPower > 0)
            {
                defensUI.SetActive(true);
            }
        }
        if(defensPower <= 0)
        {
            defensUI.SetActive(false);
        }

        Vector3 scale = new Vector3(1,1,1);
        transform.localScale = scale;
    }

    public void EndTurn()
    {
        GameManager.instance.isPlayerTurn = false;
        DropCard();
        StateCheck();
        Vector3 scale = new Vector3(0.8f,0.8f,1);
        transform.localScale = scale;
    }

    public int ReturnHP()
    {
        return hp;
    }

    public void LoseHp(int damage)
    {
        if(defensPower > 0)
        {
            int defens = defensPower - damage;
            defensPower = defens;
            if(defensPower < 0)
            {
                Vibration.Instance.CreateOneShot(50, 255);
                defensUI.SetActive(false);
                hp += defensPower;
                hitUI.FadeOut(1.0f, new Color(1,1,1,(1 - hp/maxhp)));
                SoundManager.Instance.PlaySFX(SoundManager.SFXList.MONSTER_DAMAGE);
            }
        }
        else
        {
            hp -= damage;
            Vibration.Instance.CreateOneShot(50, 255);
            hitUI.FadeOut(1.0f, new Color(1,1,1,(1 - hp/maxhp)));
            SoundManager.Instance.PlaySFX(SoundManager.SFXList.MONSTER_DAMAGE);
        }

        if(hp <= 0)
        {
            hp = 0;
            GameManager.instance.monsterOption.AllMonsters.Clear();
            deadUI.SetActive(true);
        }

        hpbar.fillAmount = (float)hp/maxhp;
    }

    public void AddHp(int hp)
    {
        hp += hp;
        if (hp > maxhp)
            hp = maxhp;
    }

    void FixedUpdate()
    {
        if(!GameManager.instance.isPlayerTurn)
            return;
        else
        {
            MyTurn();
        }
    }
}
