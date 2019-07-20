using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class Traveler : MonoBehaviour
{
    [Tooltip("현재 머물고 있는 spot 입니다.")]
    public Spot nowSpot;    // 현제 머물고 있는 spot 입니다.
    [Tooltip("spot이 변경 되었을 때 자동으로 호출될 콜백 입니다.")]
    public UnityEvent OnSpotChange;     // ChangeSpot 호출 성공시 즉시 호출됩니다.
    [Tooltip("spot이 변경되고 해당 spot으로 이동하기 시작할때 호출될 콜백 입니다.")]
    public UnityEvent OnMoveStart;      // 예상 사용처) spot이 바뀌었을때 캐릭터를 움직이거나 애니메이션을 출력
    [Tooltip("spot이 변경되고 해당 spot으로 이동이 끝났을때 호출될 콜백 입니다.")]
    public UnityEvent OnMoveEnd;        // 예상 사용쳐) OnMoveStart 에서 출력한 애니메이션을 끝낼때 사용

    public float moveSpeed = 10.0f;
    [Tooltip("true일때 ChangeSpot 호출 시 해당 spot으로 순간이동 합니다.")]
    public bool teleport = false;

    public RoutePicker routePicker;

    private bool isMoving = false;
    private Spot lastSpot = null;

    private GameObject canvas;

    public void ChangeSpot(Spot spot)
    {
        if(isMoving)
            return;
            
        if(nowSpot != spot)
        {
            lastSpot = nowSpot;
            nowSpot = spot;
            if(OnSpotChange != null)
                OnSpotChange.Invoke();
            StartCoroutine("Move");
        }
    }

    IEnumerator Move()
    {
        isMoving = true;

        if(OnMoveStart != null)
            OnMoveStart.Invoke();

        if(teleport)
            transform.position = nowSpot.transform.position;

        while(Mathf.Abs(transform.position.x - nowSpot.transform.position.x) > 0.01f && Mathf.Abs(transform.position.y - nowSpot.transform.position.y) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, nowSpot.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;

        if(OnMoveEnd != null)
            OnMoveEnd.Invoke();
        
        // nowSpot.ChangeScene();
    }

    void Start()
    {
        if(nowSpot == null)
        {
            Debug.LogError("현재 spot이 null 입니다. spot을 지정하여야 합니다.");
            return;
        }

        // ChangeSpot(nowSpot);

        transform.position = nowSpot.transform.position;
        canvas = GameObject.Find("Canvas");
        ViewRoutes();
    }

    public void ViewRoutes()
    {
        for(int i = 0; i < nowSpot.nextRoutes.Count; i++)
        {
            RoutePicker picker = Instantiate(routePicker);
            picker.SetOption(this, nowSpot.nextRoutes[i]);
            picker.transform.SetParent(canvas.transform.GetChild(0).transform);
        }
    }
}
