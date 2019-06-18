using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 클래스는 씬전환 시 SceneOptionHandler에게 SceneOption을 전달하는 역할을 합니다.

public class SceneOptionTransporter : MonoBehaviour
{
    public SceneOption sceneOption;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
   }
}