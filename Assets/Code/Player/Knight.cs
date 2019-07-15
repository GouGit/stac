using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public Player player;
    private  LinkedList<GameObject> MyCard = new LinkedList<GameObject>();
    private LinkedList<GameObject> HandCard = new LinkedList<GameObject>();
    private LinkedList<GameObject> TrashCard = new LinkedList<GameObject>();
    private GameObject showCard;
    private int hp;
    private int defensPower;
    private string playerName;
    private int cost;

    void Start()
    {
        MyCard.Clear();
        HandCard.Clear();
        TrashCard.Clear();
        for(int i = 0; i < GameManager.instance.AllCards.Count; i++)
        {
            MyCard.AddLast(GameManager.instance.AllCards[i]);
        }
        Shuffle();
        DrawCard();

        GetComponent<SpriteRenderer>().sprite = player.image;
        hp = player.hp;
        defensPower = player.defensPower;
        playerName = player.name;
        cost = player.cost;
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


    void Update()
    {
        if(!GameManager.instance.isPlayerTurn)
            return;
        else
        {
            player.MyTurn(transform.localScale);
        }
    }
}
