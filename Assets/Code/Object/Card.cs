using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Templete", menuName = "Cards")]
public class Card : ScriptableObject
{
    public new string name;
    public string explain;
    public int cost;
    public int cardValue; //각 카드별로 사용되는 내용 ex) 공겨카드->공격력, 드로우->드로우 개수
    public int defensPower;
    public bool IsAttack;
    public Sprite image;
    public Type.TYPE type;
    
    public int upgradeCostPerLevel;
    public int upgradeGemPerLevel;

    public int upgradeValue;//강화시 오르는 수치
    public int upgradeExtra;//강화시 오르는 부가 가치 ex)방어도, 공격횟수 등
}
