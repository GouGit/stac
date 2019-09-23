using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Templete", menuName = "Cards")]
public class Card : ScriptableObject
{
    public new string name;
    public int cost;
    public int attackPower;
    public int defensPower;
    public int bonusCount;
    public int fire;
    public int poision;
    public int lighting;
    public int drawCount;
    public int plusCost;
    public int minCost;
    public int powerUp;
    public Sprite image;
    public Type.TYPE type;
    
    public int upgradeCostPerLevel;
    public int upgradeGemPerLevel;
}
