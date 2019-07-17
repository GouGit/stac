using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Templete", menuName = "Players")]
public class Player : ScriptableObject
{
    public new string name;
    public int hp;
    public int defensPower;
    public int cost;
    public Sprite image;
}
