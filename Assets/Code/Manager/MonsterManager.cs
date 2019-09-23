using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public Material DissolveMat;
    public LinkedList<GameObject> AllMonsters = new LinkedList<GameObject>();
    private List<Vector3> posistions =  new List<Vector3>();
    public void CreateMonster(GameObject monster)
    {
        GameObject temp = Instantiate(monster, Vector3.zero, Quaternion.identity);

        // 몬스터 및 카드 파괴시 효과 부분 ******************************************************************************************

        // 몬스터들의 마테리얼을 바꿔준다. (클래스는 기본적으로 얕은 복사가 일어나므로 새로 생성한다. 그렇지 않으면 모든 마테리얼의 프로퍼티에 영향을 끼친다.)
        temp.GetComponent<SpriteRenderer>().material = new Material(DissolveMat);

        // 아래 코루틴은 예시이므로 나중에 실행지점을 바꿀것. 
        // 코루틴 종료 지점에 카드 파괴 파티클을 생성하거나 키워드 매개변수로 _Level을 넘겨주는 코루틴을 다시 실행하면 좋은 효과가 나옴.
        //StartCoroutine(CO_DISSOLVE(temp.GetComponent<SpriteRenderer>(), "_Edges", 1));
        // **********************************************************************************************************************

        AllMonsters.AddLast(temp);
    }

    private IEnumerator CO_DISSOLVE(SpriteRenderer renderer, string keyword, float time)
    {
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        float value = 0.0f;
        while(value < 1.0f)
        {
            value += Time.deltaTime / time;
            renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(keyword, value);
            renderer.SetPropertyBlock(_propBlock);
            yield return null;
        }
        renderer.material.SetFloat(keyword, 1);

        // 이곳에서 카드 파괴 파티클 생성하거나 아래 구문을 실행하면 됨. (아래 구문 그대로 놔두면 코루틴 무한으로 재귀되니까 if문으로 키워드 확인)
        if(keyword.CompareTo("_Level") != 0)
            StartCoroutine(CO_DISSOLVE(renderer, "_Level", 1));
    }

    public void SetMonsterPosition()
    {
        SetPos();
        int i = 0;
        for(var node = AllMonsters.First; node != null; node = node.Next)
        {
            node.Value.transform.position = ReturnPos(i);
            ++i;
        }
    }

    private void SetPos()
    {
        posistions.Clear();
        switch (AllMonsters.Count)
        {
        case 1:
            posistions.Add(new Vector3(0, 2.75f, 1));
            break;
        case 2:
            posistions.Add(new Vector3(-2.25f, 2.75f, 1));
            posistions.Add(new Vector3(2.25f, 2.75f, 1));
            break;
        case 3:
            posistions.Add(new Vector3(-4.5f, 2.75f, 1));
            posistions.Add(new Vector3(0, 2.75f, 1));
            posistions.Add(new Vector3(4.5f, 2.75f, 1));
            break;
        }
    }

    public void DestoyAll()
    {
        AllMonsters.Clear();
    }

    private Vector3 ReturnPos(int number)
    {
        return posistions[number];
    }

    public bool IsEnd()
    {
        int deadCnt = 0;
        for(var node = AllMonsters.First; node != null; node = node.Next)
        {
            if(!node.Value.activeSelf)
            {
                deadCnt++;
            }
        }
        if(deadCnt == AllMonsters.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
