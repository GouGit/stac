using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUI : MonoBehaviour
{
    protected Text bodyText;

    public virtual void Awake()
    {
        bodyText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
    }

    public void SetWindow(string bodyTextStr = "")
    {
        bodyText.text = bodyTextStr;
    }
}
