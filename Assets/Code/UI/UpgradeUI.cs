using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public GridLayoutGroup layoutGroup;
    public CardPicker pickerPrefab;

    void Awake()
    {
        int cardCount = GameManager.instance.AllCards.Count;
        for(int i = 0; i < cardCount; i++)
        {
            CardSet cardSet = GameManager.instance.AllCards[i];
            
            CardPicker picker = Instantiate(pickerPrefab);
            picker.transform.SetParent(layoutGroup.transform);
            
            picker.SetOption(cardSet.showCard, (cardPicker) => {
                // Debug.Log("올림");
                cardSet.upgradeLevel++;
                GameDataHandler.SaveCards(GameManager.instance.AllCards);
            });
        }
    }
}