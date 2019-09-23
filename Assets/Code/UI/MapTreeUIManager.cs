using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTreeUIManager : MonoBehaviour
{
    // 플레이어의 HP를 표시할 이미지입니다.
    public Image m_PlayerHPImage;

    void Start()
    {
        if (m_PlayerHPImage == null)
            m_PlayerHPImage = GameObject.Find("HP_Image").GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (m_PlayerHPImage != null && Knight.instance != null)
            m_PlayerHPImage.fillAmount = (float)Knight.instance.HP / (float)Knight.instance.MaxHP;
    }
}
