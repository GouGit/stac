using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUI : MonoBehaviour
{
    protected Text headText;
    protected Text bodyText;

    public virtual void Awake()
    {
        bodyText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        headText = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
    }

    public void SetWindow(string headTextStr = "", string bodyTextStr = "")
    {
        headText.text = headTextStr;
        bodyText.text = bodyTextStr;
    }
}
