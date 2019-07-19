using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public Player player;
    public static Knight instance = null;
    public int defensPower;
    public int usingCard, usedCard;
    private  LinkedList<GameObject> MyCard = new LinkedList<GameObject>();
    private LinkedList<GameObject> HandCard = new LinkedList<GameObject>();
    private LinkedList<GameObject> TrashCard = new LinkedList<GameObject>();
    private GameObject showCard;
    private CameraManager cam;
    private int hp;
    private string playerName;

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
        for(int i = 0; i < GameManager.instance.AllCards.Count; i++)
        {
            MyCard.AddLast(GameManager.instance.AllCards[i]);
        }
        Shuffle();
        DrawCard();

        cam = Camera.main.GetComponent<CameraManager>();
        GetComponent<SpriteRenderer>().sprite = player.image;
        hp = player.hp;
        defensPower = player.defensPower;
        playerName = player.name;
    }

    void Show()
    {
        showCard = new GameObject("Card");
        int i = -2;
        for(var node = HandCard.First; node != null; node = node.Next)
        {
            GameObject temp;
            temp = Instantiate(node.Value,new Vector3(i*2,-3.0f,0),Quaternion.identity);
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
        for(int i=0;i<showCard.transform.childCount;i++)
        {
           TrashCard.AddFirst(HandCard.First.Value);
           HandCard.RemoveFirst();
        }
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

    private void MyTurn()
    {
        Vector3 scale = new Vector3(1,1,1);
        transform.localScale = scale;
    }

    public void EndTurn()
    {
        GameManager.instance.isPlayerTurn = false;
        DropCard();
        Vector3 scale = new Vector3(0.8f,0.8f,1);
        transform.localScale = scale;
    }

    public void LoseHp(int damage)
    {
        if(defensPower > 0)
        {
            int defens = defensPower - damage;
            defensPower = defens;
            if(defensPower < 0)
            {
                hp -= defensPower;
            }
        }
        else
        {
            hp -= damage;
            cam.Shake(15);
        }

        if(hp <= 0)
        {
            Debug.Log("die");
        }
    }

    void Update()
    {
        if(!GameManager.instance.isPlayerTurn)
            return;
        else
        {
            MyTurn();
        }
    }
}
