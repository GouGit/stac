using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{

    public PlayObject myObject;
    private float hp, maxHp;
    private Vector2 scale;
    private Image hpbar;
    private Text text;

    void Start()
    {
        myObject = myObject.GetComponent<PlayObject>();
        hp = maxHp = myObject.hp;
        hpbar = GetComponent<Image>();
        text = transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = "Hp:"+ hp;
    }

    void Losing()
    {
        if(hp <= 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            hpbar.fillAmount = hp/maxHp;
            text.text = "Hp:"+ hp;
        }
    }

    void Update()
    {
        if(hp != myObject.hp)
        {
            scale = transform.localScale;
            hp = myObject.hp;
            Losing();
        }
    }
}