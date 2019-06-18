using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayObject
{
    private  LinkedList<GameObject> MyCard = new LinkedList<GameObject>();
    private LinkedList<GameObject> HandCard = new LinkedList<GameObject>();
    private LinkedList<GameObject> TrashCard = new LinkedList<GameObject>();

    private GameObject showCard;

    private static Player instance = null;
    public static Player inst;
    public int cardsCount, usedCount;
    public int defens;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            inst = instance;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(inst);
    } 

    void Start()
    {
        MyCard.Clear();
        HandCard.Clear();
        TrashCard.Clear();
        for(int i=0;i<GameManager.instance.AllCards.Count;i++)
        {
            MyCard.AddLast(GameManager.instance.AllCards[i]);
        }
        Shuffle();
        DrawCard();

        hp = 80;
    }

    void Update()
    {
        if(!GameManager.instance.playerTurn)
            return;
        
        MyTurn();
    }

    void Show()
    {
        showCard = new GameObject("Card");
        int i = -2;
        for(var node = HandCard.First; node != null; node = node.Next)
        {
            GameObject temp;
            temp = Instantiate(node.Value,new Vector3(i*2,-2.5f,0),Quaternion.identity);
            temp.gameObject.SetActive(true);
            temp.transform.position = temp.transform.position + new Vector3(0,0,-i);
            temp.transform.SetParent(showCard.transform);
            i += 1;
        }
    }

    void Shuffle()
    {
        List<GameObject> result = new List<GameObject>();
        
        for(var node = MyCard.First; node != null; node = node.Next)
        {
            result.Add(node.Value);
        }

        for(int i= 0; i<result.Count; i++)
        {
            GameObject temp = result[i];
            int index = Random.Range(0,MyCard.Count);
            result[i] = result[index];
            result[index] = temp;
        }

        MyCard.Clear();
        for(int i=0;i<result.Count;i++)
        {
            MyCard.AddLast(result[i]);
        }
    }

    public void DrawCard()
    {
        HandCard.Clear();
        for(int i=0; i<5; i++)
        {
            HandCard.AddLast(MyCard.Last.Value);
            MyCard.RemoveLast();

            if(MyCard.Count == 0)
                ReBulid();
        }
        Show();
    }

    void DropCard()
    {
        Destroy(showCard);
        for(int i=0;i<5;i++)
        {
           TrashCard.AddFirst(HandCard.First.Value);
           HandCard.RemoveFirst();
        }
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

    protected override void MyTurn()
    {
        base.MyTurn();
        cardsCount = MyCard.Count;
        usedCount = TrashCard.Count;
        if(defens > 0)
        {
            defensPower = defens;
            DefensUI.SetDefens(defensPower);
            DefensUI.gameObject.SetActive(true);
        }
        else
        {
            DefensUI.gameObject.SetActive(false);
        }
    }

    public override void EndTurn()
    {
        GameManager.instance.playerTurn = false;
        DropCard();
        Vector3 scale = transform.localScale;
        scale.x = 0.8f;
        scale.y = 0.8f;
        transform.localScale = scale;
    }

}