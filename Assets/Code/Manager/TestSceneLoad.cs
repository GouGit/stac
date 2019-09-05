using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    void Start()
    {
        // GameManager.instance.monsterOption.CreateMonster(Resources.Load("Skeleton") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Gagoil") as GameObject);
        GameManager.instance.monsterOption.CreateMonster(Resources.Load("MonsterPrefabs/Dragon") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Ork") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Frankenstein") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Succbus") as GameObject); 
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Vampire") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Reaper") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Rich") as GameObject);  
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Medusa") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("Kraken") as GameObject);
        //GameManager.instance.monsterOption.CreateMonster(Resources.Load("DeathWorm") as GameObject);
        GameManager.instance.monsterOption.SetMonsterPosition();
    }

}
