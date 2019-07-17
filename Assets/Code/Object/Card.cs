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
    public Sprite image;
    public Type.TYPE type;
}
