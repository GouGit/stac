using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDefens : MonoBehaviour
{
    private Text text;
    private int defens;

    void Start()
    {
        text = transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    public void SetDefens(int power)
    {
        defens = power;
    }

    void Update()
    {
        text.text = "" + defens;
    }
}
