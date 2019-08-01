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
        foreach(CardSet cardSet in GameManager.instance.AllCards)
        {
            CardPicker picker = Instantiate(pickerPrefab);
            picker.SetOption(cardSet.showCard, (cardPicker) => {
                Debug.Log("업그레이드 했다!!");
            });
            picker.transform.SetParent(layoutGroup.transform);
        }
    }
}