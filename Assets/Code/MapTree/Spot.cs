using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public LineRenderer liner;
    [Tooltip("이 지점로 오는 길은 되돌아 갈 수 없는 길이 됩니다.")]
    public bool isOneWay;               // true 일시 이spot으로 오는 길은 뒤로 돌아갈 수 없는 길이 됩니다.
    [Tooltip("이 지점에서 길을 이을 다음 지점 입니다.")]
    public List<Spot> nextRoutes;       // 다음으로 이어질 spot 입니다.

    public SceneOption sceneOption;

    [HideInInspector]
    public bool isTraversal = false;    // 맵 트리를 순회할때 중복순회를 막기위해 사용됩니다.
    [HideInInspector]
    public int ID;                      // 맵을 저장하고 불러올때 사용될 변수 입니다. 코드상에서 절대로 건들면 안됩니다.

    public void test()
    {
        Debug.Log("와!!!");
    }

    void Start()
    {
        liner.positionCount = nextRoutes.Count * 2;
        
        int count = nextRoutes.Count;

        for(int i = 0; i < count; i++)
        {
            int offset = (i + 1) * 2 - 1;
            liner.SetPosition(offset - 1, transform.position);
            liner.SetPosition(offset, nextRoutes[i].transform.position);
        }
    }

    public void ChangeScene()
    {
        SceneLoader.LoadScene("BattleScene", sceneOption);
    }
}
