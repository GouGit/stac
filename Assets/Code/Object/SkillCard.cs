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
        DEBUFF,
        DONT
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
        if(GameManager.instance.cost >= cost)
        {
            GameManager.instance.cost -= cost;
            ShowMonster monster = ob.GetComponent<ShowMonster>();
            switch (skill)
            {
            case SkILL.FIRE:
                monster.Fire = 2;
                    monster.fireParticle = Instantiate(Resources.Load("Particles/Fire Particle System") as GameObject).GetComponent<ParticleSystem>();
                    monster.fireParticle.transform.position = monster.transform.position;
                    break;
            case SkILL.POISION:
                monster.Poision = 4;
                    monster.poisionParticle = Instantiate(Resources.Load("Particles/Toxin Particle System") as GameObject).GetComponent<ParticleSystem>();
                    monster.poisionParticle.transform.position = monster.transform.position;
                    break;
            case SkILL.LIGHTING:
                monster.Lighting = 2;
                    monster.lightingParticle = Instantiate(Resources.Load("Particles/Lighting Particle System") as GameObject).GetComponent<ParticleSystem>();
                    monster.lightingParticle.transform.position = monster.transform.position;
                    break;
            case SkILL.DEBUFF:
                monster.Fire *= 2;
                monster.Lighting *= 2;
                monster.Poision *= 2;
                break;
            case SkILL.DONT:
                monster.isDont = true;
                break;
            }
            gameObject.SetActive(false);   
            Knight.instance.Sort();
        }
    }
}
