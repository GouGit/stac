using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public void LoadTitle()
    {
        SceneLoader.LoadSceneWithFadeStatic("Title");
    }

    public void LoadTitleDie()
    {
        SceneLoader.LoadSceneWithFadeStatic("Title");
        GameDataHandler.SaveStageCount(0);                
        Debug.Log("asdf");
    }

    public void LoadMapTree()
    {
        SceneLoader.LoadSceneWithFadeStatic("MapTree");
    }

    public void DestroyAll()
    {
        GameManager.instance.monsterOption.DestoyAll();
    }
}
