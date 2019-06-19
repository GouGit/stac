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
            Debug.LogError("SceneOptionTransporter를 찾을 수 없습니다.");
            return;
        }

        CreateBattleScene(transporter.sceneOption);
        Player.inst.gameObject.SetActive(true);
        Player.inst.DefensUI = GameObject.Find("StateUI").transform.GetChild(5).GetComponent<OnDefens>();
        GameObject.Find("end").GetComponent<Button>().onClick.AddListener(Player.inst.EndTurn);

        GameObject StateUI = GameObject.Find("StateUI");
        StateUI.transform.GetChild(1).transform.GetChild(1).GetComponent<Hpbar>().myObject = Player.inst;
    }

    public void CreateBattleScene(SceneOption option)
    {
        for(int i = 0; i < option.objectList.Count; i++)
        {
            Instantiate(option.objectList[i]);
        }
    }

    void OnDestroy()
    {
        Player.inst.gameObject.SetActive(false);
        Player.inst.DefensUI = null;
    }
}