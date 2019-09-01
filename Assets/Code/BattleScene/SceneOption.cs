using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SceneOption
{
    public enum Type
    { 
        Boss, Battle, Event, Rest, Start, MAX
    };

    [Tooltip("다음씬의 타입")]
    public Type type;

    [Tooltip("다음씬에서 소환할 객체들의 프리팹을 넣습니다.")]
    public List<GameObject> objectList;

    public SceneOption(Type type)
    {
        this.type = type;
        objectList = new List<GameObject>();
    }
}
