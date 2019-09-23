using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTreeUIManager : MonoBehaviour
{
    // 플레이어의 HP를 표시할 이미지입니다.
    public Image m_PlayerHPImage;
    public Player m_Player;

    void Start()
    {
        if (m_PlayerHPImage == null)
            m_PlayerHPImage = GameObject.Find("HP_Image").GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (m_PlayerHPImage != null && m_Player != null)
            //m_PlayerHPImage.fillAmount = (float)Knight.instance.HP / Knight.instance.MaxHP;
            m_PlayerHPImage.fillAmount = (float)m_Player.hp / (float)m_Player.maxHp;
    }
}
