﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Made By : 안규원
// Comment : 메인 화면의 UI들을 관리 (애니메이션 및 기능들)할 스크립트입니다.

public class MainUIMnager : MonoBehaviour
{
    private MainUIMnager instance;
    public MainUIMnager Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogWarning(typeof(MainUIMnager).FullName + " Instance is null.");
                return null;
            }
            return instance;
        }
    }

    public float SlideObjectSpeed = 5.0f;
    // Volume
    public RectTransform VolumeController;
    private bool isVolumeControl = false;
    private Coroutine VolumeCoroutine = null;

    // Vibration
    public RectTransform VibrationController;
    private bool isVibrationControl = false;
    private Coroutine VibrationCoroutine = null;

    // Text
    public Text GoldText;
    public Text RubyText;
    public Text SapphireText;
    public Text TopazText;
    public Text DiamondText;

    private void Awake()
    {
        instance = this;
    }

    public void VolumeControl()
    {
        isVolumeControl = !isVolumeControl;

        if(VolumeCoroutine != null)
        {
            StopCoroutine(VolumeCoroutine);
            VolumeCoroutine = null;
        }
        VolumeCoroutine = StartCoroutine(CO_SlideObject(VolumeController, isVolumeControl ? new Vector3(555, 130) : new Vector3(1210, 130)));
    }

    public void VibrationControl()
    {
        isVibrationControl = !isVibrationControl;

        if (VibrationCoroutine != null)
        {
            StopCoroutine(VibrationCoroutine);
            VibrationCoroutine = null;
        }
        VibrationCoroutine = StartCoroutine(CO_SlideObject(VibrationController, isVibrationControl ? new Vector3(555, -130) : new Vector3(1210, -130)));
    }

    IEnumerator CO_SlideObject(RectTransform obj, Vector3 pos)
    {
        while(Vector3.Distance(obj.localPosition, pos) > 50.0f)
        {
            obj.localPosition += (pos - obj.localPosition).normalized * SlideObjectSpeed * Time.deltaTime;
            yield return null;
        }
        obj.localPosition = pos;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }
}
