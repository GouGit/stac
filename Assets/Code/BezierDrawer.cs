using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierDrawer : MonoBehaviour
{
    private static BezierDrawer instance = null;
    public static BezierDrawer Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("BezierDrawer").AddComponent<BezierDrawer>();
            }
            return instance;
        }
    }
    private LineRenderer lineRenderer;
    private Vector3[] points = new Vector3[4];
    private Vector3[] curve;
    public SpriteRenderer arrowSprite;
    public int curveNum = 10;
    public float flowSpeed = -4f;

    public Vector3 startPosition;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            instance.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = curveNum;
        curve = new Vector3[curveNum];
    }

    void OnEnable()
    {
        startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // startPosition = transform.position;
        startPosition.z = 0;
        points[0] = startPosition;
        if (arrowSprite != null)
            arrowSprite.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        if(arrowSprite != null)
            arrowSprite.gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputWorldPosition.z = 0;


        lineRenderer.material.mainTextureOffset = new Vector2(Time.timeSinceLevelLoad * flowSpeed * lineRenderer.material.mainTextureScale.x, 1); ;


        // 2차 베지어 곡선 그리므로 4번째 지점은 설정 안해도 됨
        points[3] = points[2] = inputWorldPosition;

        points[1].x = points[0].x;
        points[1].y = points[2].y;

        ComputeBezier(ref points, curveNum, ref curve);

        lineRenderer.SetPositions(curve);

        if (arrowSprite != null)
        {
            arrowSprite.transform.position = inputWorldPosition;
            arrowSprite.transform.up = (inputWorldPosition - lineRenderer.GetPosition(lineRenderer.positionCount - 2)).normalized;
        }
        else
            Debug.LogWarning(string.Format("{0} : 화살표 Sprite가 Null입니다.", name));
    }

    void ComputeBezier(ref Vector3[] cp, int numberOfPoints, ref Vector3[] curve)
    {
        float dt;
        int i;

        dt = 1.0f / (numberOfPoints - 1);

        for (i = 0; i < numberOfPoints; i++)
            curve[i] = CalculateQuadraticBezierPoint(i * dt, ref cp[0], ref cp[1], ref cp[2]);

    }

    // 3차 베지어 곡선
    private Vector3 CalculateCubicBezierPoint(float t, ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3)
    {
        // return = (1 - t)3 P0 + 3(1 - t)2 tP1 + 3(1 - t) t2 P2 + t3 P3
        //             uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    // 2차 베지어 곡선
    private Vector3 CalculateQuadraticBezierPoint(float t, ref Vector3 p0, ref Vector3 p1, ref Vector3 p2)
    {
        //B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}
