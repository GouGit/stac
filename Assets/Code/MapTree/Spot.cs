using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spot : MonoBehaviour,  IPointerClickHandler
{
    public LineRenderer liner;
    [Tooltip("이 지점에서 길을 이을 다음 지점 입니다.")]
    public List<Spot> nextSpots;       // 다음으로 이어질 spot 입니다.

    public SceneOption sceneOption;

    [HideInInspector]
    public bool isTraversal = false;    // 맵 트리를 순회할때 중복순회를 막기위해 사용됩니다.
    [HideInInspector]
    public int ID;                      // 맵을 저장하고 불러올때 사용될 변수 입니다. 코드상에서 절대로 건들면 안됩니다.
    // [HideInInspector]
    public bool isClear = false;        // 맵의 진행도를 저장하고 불러올때 사용할 변수 입니다. 해당 Spot에 도달 했었는지를 나타냅니다.

    public List<Sprite> spriteList;

    public static Spot nowSpot;

    void Start()
    {
        liner.positionCount = nextSpots.Count * 2;
        
        int count = nextSpots.Count;

        for(int i = 0; i < count; i++)
        {
            int offset = (i + 1) * 2 - 1;
            liner.SetPosition(offset - 1, transform.position);
            liner.SetPosition(offset, nextSpots[i].transform.position);
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
        liner.positionCount = nextSpots.Count * 2;
        
        int count = nextSpots.Count;

        for(int i = 0; i < count; i++)
        {
            int offset = (i + 1) * 2 - 1;
            liner.SetPosition(offset - 1, transform.position);
            liner.SetPosition(offset, nextSpots[i].transform.position);
        }
    }

    public static Spot GetFirstSpot()
    {// Spot 컴포넌트를 가지는 모든 오브젝트를 조회하여 첫 Spot을 찾습니다.
        Spot[] spots = GameObject.FindObjectsOfType<Spot>();
        foreach(Spot spot in spots)
        {
            if(spot.ID == 0)
            {
                return spot;
            }
        }
        // 여기까지 왔다는 것은 맵 파일이 잘못 되었다는것을 의미합니다.
        return null;
    }

    public static Spot GetProgressSpot()
    {
        Spot firstSpot = GetFirstSpot();
        if(firstSpot == null)
        {// 맵 파일이 잘못 되었습니다.
            return null;
        }

        return GetProgressSpotRecursion(firstSpot);
    }

    private static Spot GetProgressSpotRecursion(Spot spot)
    {// isClear가 false인 spot을 리턴하기 위한 재귀함수 입니다.
        foreach(Spot nextSpot in spot.nextSpots)
        {
            if(nextSpot.isClear)
                return GetProgressSpotRecursion(nextSpot);
        }

        // 여기까지 왔다는 것은 현재 spot이 최종 진행도 라는 뜻 입니다.
        return spot;
    }

    public static void SetParent()
    {
        Spot[] spots = GameObject.FindObjectsOfType<Spot>();
        GameObject map = GameObject.Find("Map");
        foreach(Spot spot in spots)
        {
            spot.transform.SetParent(map.transform);
        }   
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isClear)
            return;

        isClear = true;

        nowSpot = this;

        GameDataHandler.SaveProgress(GetFirstSpot(), "Test");

        SceneLoader.LoadScene("BattleScene", sceneOption);
    }

    public static void ViewNextSpot()
    {
        // for(int i = 0; i < nowSpot.nextSpots.Count; i++)
        // {
        //     RoutePicker picker = Instantiate(routePicker);
        //     picker.SetOption(this, nowSpot.nextSpots[i]);
        //     picker.transform.SetParent(canvas.transform.GetChild(0).transform);
        // }
    }
}
