using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    public float m_DestroyDelay = -1;
    public bool m_isDestroySelf = true;

    void Start()
    {
        if (m_DestroyDelay >= 0)
        {
            if (m_isDestroySelf)
                DestroyItself(m_DestroyDelay);
            else
                DestroyParent(m_DestroyDelay);
        }
    }

    public void DestroyItself()
    {
        Destroy(this.gameObject);
    }

    public void DestroyItself(float delay)
    {
        StartCoroutine(CO_DestroyTimer(delay));
    }

    IEnumerator CO_DestroyTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyItself();
    }

    public void DestroyParent()
    {
        Destroy(this.transform.parent?.gameObject ?? this.gameObject);
    }

    public void DestroyParent(float delay)
    {
        StartCoroutine(CO_DestroyTimer(delay));
    }

    IEnumerator CO_DestroyParentTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyParent();
    }
}
