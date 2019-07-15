using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Templete", menuName = "Monsters")]
public class Monster : ScriptableObject
{
    public new string name;
    public int hp;
    public int attackPower;
    public int defensPower;
    public Sprite image;
}
