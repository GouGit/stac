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
        // 마우스 입력 좌표를 베지어 곡선의 시작 지점으로 설정
        Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputWorldPosition.z = 0;

        // 베지어 곡선 텍스쳐 타일링
        lineRenderer.material.mainTextureOffset = new Vector2(Time.timeSinceLevelLoad * flowSpeed * lineRenderer.material.mainTextureScale.x, 1); ;

        // 2차 베지어 곡선 그리므로 4번째 지점은 설정 안해도 됨 (3차 베지어 곡선 사용시 수정 필요)
        points[3] = points[2] = inputWorldPosition;

        points[1].x = points[0].x;
        points[1].y = points[2].y;

        // 베지어 곡선 좌표 계산
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

    /// <summary>
    /// 베지어 곡선을 계산하여 좌표를 반환합니다.
    /// </summary>
    /// <param name="cp">각 꼭짓점의 좌표 (3개 필요)</param>
    /// <param name="numberOfPoints">총 정점들의 개수입니다. 얼마나 부드럽게 곡선을 만들지 결정합니다.</param>
    /// <param name="curve">최종적으로 계산된 베지어의 정점들이 저장될 배열입니다.</param>
    void ComputeBezier(ref Vector3[] cp, int numberOfPoints, ref Vector3[] curve)
    {
        // 입력된 총 정점들의 개수만큼 좌표를 계산하여 레퍼런스에 반환합니다.
        float dt;
        int i;

        dt = 1.0f / (numberOfPoints - 1);

        for (i = 0; i < numberOfPoints; i++)
            curve[i] = CalculateQuadraticBezierPoint(i * dt, ref cp[0], ref cp[1], ref cp[2]);

    }

    /// <summary>
    /// 2차 베지어 곡선 정점 계산
    /// </summary>
    /// <param name="t">0 ~ 1의 값 사이에서 베지어 곡선 사이의 지점을 입력합니다.</param>
    /// <param name="p0">베지어곡선 시작점</param>
    /// <param name="p1">베지어곡선 중간지점</param>
    /// <param name="p2">베지어곡선 끝지점</param>
    /// <returns>t 지점의 정점 좌표 반환</returns>
    private Vector3 CalculateQuadraticBezierPoint(float t, ref Vector3 p0, ref Vector3 p1, ref Vector3 p2)
    {
        // 2차 베지어곡선 계산 공식
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
}
