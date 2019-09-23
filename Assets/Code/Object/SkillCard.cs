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
        origin = transform.position;
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
                monster.Fire += fire;
                    if (monster.fireParticle == null)
                    {
                        monster.fireParticle = Instantiate(Resources.Load("Particles/Fire Particle System") as GameObject).GetComponent<ParticleSystem>();
                        monster.fireParticle.transform.position = monster.transform.position;
                    }
                    SoundManager.Instance.PlaySFX(SoundManager.SFXList.BURNING);
                    break;
            case SkILL.POISION:
                monster.Poision += poision;
                    if (monster.poisionParticle == null)
                    {
                        monster.poisionParticle = Instantiate(Resources.Load("Particles/Toxin Particle System") as GameObject).GetComponent<ParticleSystem>();
                        monster.poisionParticle.transform.position = monster.transform.position;
                    }
                    SoundManager.Instance.PlaySFX(SoundManager.SFXList.GLASS_BREAK);
                    break;
            case SkILL.LIGHTING:
                monster.Lighting += lighting;
                    if (monster.lightingParticle == null)
                    {
                        monster.lightingParticle = Instantiate(Resources.Load("Particles/Lighting Particle System") as GameObject).GetComponent<ParticleSystem>();
                        monster.lightingParticle.transform.position = monster.transform.position;
                    }
                    SoundManager.Instance.PlaySFX(SoundManager.SFXList.THUNDER);
                    break;
            case SkILL.DEBUFF:
                monster.Fire = (monster.Fire+poision) * 2;
                monster.Lighting = (monster.Lighting+lighting) * 2;
                monster.Poision = (monster.Poision+poision) * 2;
                break;
            case SkILL.DONT:
                monster.isDont = true;
                Knight.instance.defensPower += defensPower;
                break;
            }
            gameObject.SetActive(false);   
            Knight.instance.Sort();
        }
    }
}
