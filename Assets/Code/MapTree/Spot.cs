using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spot : MonoBehaviour,  IPointerClickHandler
{
    public LineRenderer liner;
    [Tooltip("이 지점에서 길을 이을 다음 지점 입니다.")]
    public List<Spot> nextRoutes;       // 다음으로 이어질 spot 입니다.

    public SceneOption sceneOption;

    [HideInInspector]
    public bool isTraversal = false;    // 맵 트리를 순회할때 중복순회를 막기위해 사용됩니다.
    [HideInInspector]
    public int ID;                      // 맵을 저장하고 불러올때 사용될 변수 입니다. 코드상에서 절대로 건들면 안됩니다.
    // [HideInInspector]
    private bool isClear = false;        // 맵의 진행도를 저장하고 불러올때 사용할 변수 입니다. 해당 Spot에 도달 했었는지를 나타냅니다.

    public List<Sprite> spriteList;

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
        
        
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        render.sprite = spriteList[(int)sceneOption.type];

        if(isClear)
        {
            Color color = render.color;
            color.a = 0.3f;
            render.color = color;
        }
    }

    public void ChangeScene()
    {
        SceneLoader.LoadScene("BattleScene", sceneOption);
    }

    void LateUpdate()
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

    public void OnPointerClick(PointerEventData eventData)
    {
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            Destroy(transform.parent.GetChild(i).gameObject);
        }

        SceneLoader.LoadScene("TestScene", sceneOption);

        // traveler.ChangeSpot(spot);    
    }
}
