using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public Image m_FadeObject;
    public bool m_isStartFadeIn = false;
    public bool m_isStartFadeOut = false;
    public float m_Duration = 1.0f;
    public float m_StartAlpha = 1.0f;

    public event Action<FadeUI> m_OnFadeInBegin = (o) => { };
    public event Action<FadeUI> m_OnFadeInEnd = (o) => { };
    public event Action<FadeUI> m_OnFadeOutBegin = (o) => { };
    public event Action<FadeUI> m_OnFadeoutEnd = (o) => { };

    private Coroutine m_fadeRoutine;

    private void Awake()
    {
        if (m_FadeObject == null)
            m_FadeObject = GetComponent<Image>();

        Color color = m_FadeObject.color;
        color.a = m_StartAlpha;
        m_FadeObject.color = color;

        if (m_isStartFadeIn)
            FadeIn(m_Duration);
        if (m_isStartFadeOut)
            FadeOut(m_Duration);
    }

    public void FadeIn()
    {
        if (m_fadeRoutine != null)
        {
            StopCoroutine(m_fadeRoutine);
            m_fadeRoutine = null;
        }
        m_fadeRoutine = StartCoroutine(CO_FadeIn(m_Duration));
    }

    public void FadeIn(float duration)
    {
        gameObject.SetActive(true);
        if (m_fadeRoutine != null)
        {
            StopCoroutine(m_fadeRoutine);
            m_fadeRoutine = null;
        }
        m_fadeRoutine = StartCoroutine(CO_FadeIn(duration));
    }

    IEnumerator CO_FadeIn(float duration)
    {
        m_OnFadeInBegin(this);
        m_FadeObject.raycastTarget = true;

        while (true)
        {
            Color objColor = m_FadeObject.color;
            objColor.a += Time.deltaTime / duration;
            if(objColor.a >= 1.0f)
            {
                objColor.a = 1.0f;
                m_FadeObject.color = objColor;
                break;
            }
            m_FadeObject.color = objColor;
            yield return null;
        }

        m_fadeRoutine = null;
        m_FadeObject.raycastTarget = false;
        m_OnFadeInEnd(this);
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);
        if (m_fadeRoutine != null)
        {
            StopCoroutine(m_fadeRoutine);
            m_fadeRoutine = null;
        }
        m_fadeRoutine = StartCoroutine(CO_FadeOut(m_Duration));
    }

    public void FadeOut(float duration)
    {
        if (m_fadeRoutine != null)
        {
            StopCoroutine(m_fadeRoutine);
            m_fadeRoutine = null;
        }
        m_fadeRoutine = StartCoroutine(CO_FadeOut(duration));
    }

    IEnumerator CO_FadeOut(float duration)
    {
        m_OnFadeOutBegin(this);
        m_FadeObject.raycastTarget = true;

        while (true)
        {
            Color objColor = m_FadeObject.color;
            objColor.a -= Time.deltaTime / duration;
            if (objColor.a <= 0.0f)
            {
                objColor.a = 0.0f;
                m_FadeObject.color = objColor;
                break;
            }
            m_FadeObject.color = objColor;
            yield return null;
        }

        m_fadeRoutine = null;
        m_FadeObject.raycastTarget = false;
        m_OnFadeoutEnd(this);
    }
}
