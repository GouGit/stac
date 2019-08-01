using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.UI;

public class CardPicker : MonoBehaviour
{
    public Image image;
    private ResultWindow window;
    public ShowCard card;

    public void SetOption(ShowCard card, UnityAction<CardPicker> action)
    {
        this.card = card;
        image.sprite = card.card.image;
        GetComponent<Button>().onClick.AddListener(delegate{action(this);});
    }
}
