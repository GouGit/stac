using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

using UnityEngine.UI;

// 이 컴포넌트는 씬에 하나만 존재하여야 하며, SceneOption을 기반으로 배틀씬을 구성합니다.
public class SceneOptionHandler : MonoBehaviour
{
    void Awake()
    {
        SceneOptionTransporter transporter = GameObject.FindObjectOfType<SceneOptionTransporter>(); 
        if(transporter == null)
        {
            SceneOption option = new SceneOption();
            option.type = SceneOption.Type.Battle;
            option.objectList.Add(Resources.Load("Skeleton") as GameObject);

            Debug.Log("SceneOptionTransporter를 찾을 수 없습니다.\n테스트를 위하여 Skeleton을 소환합니다.");
            return;
        }
        else
        {
            CreateBattleScene(transporter.sceneOption);
        }
    }

    public void CreateBattleScene(SceneOption option)
    {
        for(int i = 0; i < option.objectList.Count; i++)
        {
            GameManager.instance.monsterOption.CreateMonster(option.objectList[i]);
        }
        
        GameManager.instance.monsterOption.SetMonsterPosition();
    }
}