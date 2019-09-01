using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class Traveler : MonoBehaviour
{


    // private GameObject canvas;

    // public void ChangeSpot(Spot spot)
    // {
    //     if(isMoving)
    //         return;
            
    //     if(nowSpot != spot)
    //     {
    //         lastSpot = nowSpot;
    //         nowSpot = spot;
    //         if(OnSpotChange != null)
    //             OnSpotChange.Invoke();
    //         StartCoroutine("Move");
    //     }
    // }

    // IEnumerator Move()
    // {
    //     isMoving = true;

    //     if(OnMoveStart != null)
    //         OnMoveStart.Invoke();

    //     if(teleport)
    //         transform.position = nowSpot.transform.position;

    //     while(Mathf.Abs(transform.position.x - nowSpot.transform.position.x) > 0.01f && Mathf.Abs(transform.position.y - nowSpot.transform.position.y) > 0.01f)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, nowSpot.transform.position, moveSpeed * Time.deltaTime);
    //         yield return null;
    //     }

    //     isMoving = false;

    //     if(OnMoveEnd != null)
    //         OnMoveEnd.Invoke();
    // }

    // void Start()
    // {
    //     if(nowSpot == null)
    //     {
    //         Debug.LogError("현재 spot이 null 입니다. spot을 지정하여야 합니다.");
    //         return;
    //     }

    //     transform.position = nowSpot.transform.position;
    //     canvas = GameObject.Find("Canvas");
    //     ViewRoutes();
    // }

    // public void ViewRoutes()
    // {
    //     for(int i = 0; i < nowSpot.nextSpots.Count; i++)
    //     {
    //         RoutePicker picker = Instantiate(routePicker);
    //         picker.SetOption(this, nowSpot.nextSpots[i]);
    //         picker.transform.SetParent(canvas.transform.GetChild(0).transform);
    //     }
    // }
}
