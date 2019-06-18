using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMotion : MonoBehaviour
{
    public bool isAttack;
    private Text text;
    private int attack, defens;

    void Start()
    {
        text = transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    public void SetAttack(int power)
    {
        attack = power;
    }

    public void SetDefens(int power)
    {
        defens = power;
    }

    void Update()
    {
        switch (isAttack)
        {    
        case true:
            text.text = "" + attack;
            break;
        case false:
            text.text = "" + defens;
            break;
        }
    }

}
