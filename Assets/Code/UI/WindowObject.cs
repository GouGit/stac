using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WindowObject : MonoBehaviour
{
    // 리스트 인덱스들 끼리 요소가 이어짐.
    public List<Button> ButtonList = new List<Button>();
    public List<string> ButtonTextList = new List<string>();
    public List<UnityAction> EventList = new List<UnityAction>();

    public void OnInitialized()
    {
        // 리스트의 인덱스들 중 하나라도 다르면 에러
        if(ButtonList.Count == ButtonTextList.Count && ButtonTextList.Count == EventList.Count && EventList.Count == ButtonList.Count)
        {
            for (int index = 0; index < ButtonList.Count; index++) 
            {
                var button = ButtonList[index];
                button.transform.GetChild(0).GetComponent<Text>().text = ButtonTextList[index];
                button.onClick.AddListener(EventList[index]);
            }
        }
        else
        {
            Debug.LogWarning("WindowObject에서 버튼 / 텍스트 / 이벤트 중 인덱스가 다릅니다.");
        }
    }
}
