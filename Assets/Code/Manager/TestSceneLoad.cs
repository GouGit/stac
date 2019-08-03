using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    void Start()
    {
        // GameManager.instance.monsterOption.CreateMonster(Resources.Load("Skeleton") as GameObject);
        // GameManager.instance.monsterOption.CreateMonster(Resources.Load("Gagoil") as GameObject);
        GameManager.instance.monsterOption.CreateMonster(Resources.Load("Dragon") as GameObject);
        GameManager.instance.monsterOption.SetMonsterPosition();
    }

}
