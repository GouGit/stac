using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// (대충 버튼 이벤트로 오브젝트 파괴하고싶을때 써도 되는 스크립트)
// DestroyItself()는 자신 오브젝트를 파괴하는거고 Hierarchy는 그 오브젝트의 루트 오브젝트를 파괴
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
                DestroyHierarchy(m_DestroyDelay);
        }
    }

    public void DestroyItself()
    {
        Destroy(this.gameObject);
    }

    public void DestroyItself(float delay)
    {
        Destroy(this.gameObject, delay);
    }

    public void DestroyHierarchy()
    {
        // (? 는 null인지 확인함. ?? 뒤에는 null일경우 기본값을 지정)
        Destroy(this.transform.root?.gameObject ?? this.gameObject);
        //if (transform.root != null)
        //    Destroy(transform.root.gameObject);
        //else
        //    Destroy(this.gameObject);
    }

    public void DestroyHierarchy(float delay)
    {
        Destroy(this.transform.root?.gameObject ?? this.gameObject, delay);
        //if (transform.parent != null)
        //    Destroy(transform.parent.gameObject, delay);
        //else
        //    Destroy(this.gameObject, delay);
    }
}
