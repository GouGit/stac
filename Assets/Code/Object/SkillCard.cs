using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard : ShowCard
{
    public enum SkILL
    {
        FIRE,
        LIGHTING,
        POISION,
        DEBUFF
    }
    public SkILL skill;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnMouseDown()
    {
        transform.localScale = scale * 1.25f;
        BezierDrawer.Instance.gameObject.SetActive(true);
        BezierDrawer.Instance.startPosition = gameObject.transform.position; 
    }

    protected override void OnMouseDrag()
    {
        return;
    }

    protected override void OnMouseUp()
    {
        transform.localScale = scale;
        UseCard();
        BezierDrawer.Instance.gameObject.SetActive(false);
    }

    protected override void Using(GameObject ob)
    {
        ShowMonster monster = ob.GetComponent<ShowMonster>();
        switch (skill)
        {
        case SkILL.FIRE:
            monster.fire = 2;
            break;
        case SkILL.POISION:
            monster.poision = 4;
            break;
        case SkILL.LIGHTING:
            monster.lighting = 2;
            break;
        case SkILL.DEBUFF:
            monster.fire *= 2;
            monster.lighting *= 2;
            monster.poision *= 2;
            break;
        }
        gameObject.SetActive(false);
    }
}
