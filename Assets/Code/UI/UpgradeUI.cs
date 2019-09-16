using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public GridLayoutGroup layoutGroup;
    public CardPicker pickerPrefab;
    public GameObject upParticle;

    void Awake()
    {
        upParticle = Resources.Load("Upgrade Particle System") as GameObject;
        int cardCount = GameManager.instance.AllCards.Count;
        for(int i = 0; i < cardCount; i++)
        {
            CardSet cardSet = GameManager.instance.AllCards[i];
            
            CardPicker picker = Instantiate(pickerPrefab);
            picker.transform.SetParent(layoutGroup.transform);
            
            picker.SetOption(cardSet.showCard, (cardPicker) => {
                // Debug.Log("올림");
                WindowUI win = Instantiate(MainUIMnager.Instance.window);
                win.GetComponent<ChooseWindowUI>().SetWindow("카드 강화", "계속 하시겠습니까?\n\n" + 
                    string.Format("{0} -> {1}", cardSet.upgradeLevel, cardSet.upgradeLevel + 1), "네", "아니요", () =>
                {
                    cardSet.upgradeLevel++;
                    GameObject temp = Instantiate(upParticle, cardPicker.transform.position, Quaternion.identity);
                    Destroy(temp, 3.0f);
                    GameDataHandler.SaveCards(GameManager.instance.AllCards);
                    Destroy(win.gameObject);
                }, () => Destroy(win.gameObject));
                
            });
        }
        
        Canvas cam = transform.GetChild(0).GetComponent<Canvas>();
        cam.worldCamera = Camera.main;
    }
}