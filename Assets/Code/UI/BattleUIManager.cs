using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public void LoadTitle()
    {
        SceneLoader.Instance.LoadSceneWithFade("Title");
    }

    public void LoadTitleDie()
    {
        SceneLoader.Instance.LoadSceneWithFade("Title");
        GameDataHandler.SaveStageCount(0);                
        Debug.Log("asdf");
    }

    public void LoadMapTree()
    {
        SceneLoader.Instance.LoadSceneWithFade("MapTree");
    }

    public void DestroyAll()
    {
        GameManager.instance.monsterOption.DestoyAll();
    }
}
