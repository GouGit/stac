using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultWindow : MonoBehaviour
{
    public Button TitleButton;

    public CardPicker cardPicker;

    [SerializeField]    
    Text GoldText;
    [SerializeField]
    Text TopazText;
    [SerializeField]
    Text RubyText;
    [SerializeField]
    Text SapphireText;
    [SerializeField]
    Text DiamondText;
    
    [SerializeField]
    HorizontalLayoutGroup cardLayout;
    
    public List<ShowCard> cardPrefabList;

    public bool isSelected = false;


    void Start()
    {
        OnInitialized();
    }

    public void SetOption(int goldCount, int topazCount, int rubyCount, int sapphireCount, int diamondCount)
    {
        GoldText.text = goldCount.ToString();
        TopazText.text = topazCount.ToString();
        RubyText.text = rubyCount.ToString();
        SapphireText.text = sapphireCount.ToString();
        DiamondText.text = diamondCount.ToString();

        GameManager.instance.goldCount += goldCount;
        GameManager.instance.topazCount += topazCount;
        GameManager.instance.rubyCount += rubyCount;
        GameManager.instance.sapphireCount += sapphireCount;
        GameManager.instance.diamondCount += diamondCount;

        if(cardLayout == null)
        {
            Debug.LogError("cardLayout이 없어 보상창에 추가카드를 표시할 수 없습니다.");
            return;
        }

        if(cardPrefabList.Count == 0)
        {
            Debug.LogError("추가로 제공할 카드가 없습니다.");
            return;
        }

        // 일단은 추가카드 선택은 랜덤으로 해뒀음 나중에 수정할 예정
        for(int i = 0; i < 3 ; i++)
        {
            int randomIndex = Random.Range(0, cardPrefabList.Count);

            ShowCard additionalCard = cardPrefabList[randomIndex];
            CardPicker picker = Instantiate(cardPicker); 

            picker.SetOption(additionalCard, (cardpicker) => {
                if(!this.isSelected)
                {
                    GameManager.instance.AllCards.Add(new CardSet(additionalCard, 0));
                    cardpicker.image.sprite = null;
                    this.isSelected = true;
                    GameDataHandler.SaveCards(GameManager.instance.AllCards);
                }
            });
            
            picker.transform.SetParent(cardLayout.transform);
        }
    }

    protected void OnInitialized()
    {
        TitleButton.onClick.AddListener(OnClickTitleButton);
    }

    protected void OnClickTitleButton()
    {
        SceneLoader.LoadSceneWithFadeStatic("MapTree");
        Destroy(gameObject);
    }
}
