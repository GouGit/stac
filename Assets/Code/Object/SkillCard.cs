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
    private int maxLevel = 5;

    protected override void Start()
    {
        base.Start();
    }

    protected override void CardUpgrade()
    {
        switch (card.name)
        {
        case "상처벌리기":
            for(int i = 0; i < level; i++)
            {
                if(maxLevel == level)
                    cost -= 1;
                else
                {
                    cardValue += card.upgradeValue;
                }
            }
            break;
        case "금지지령":
            for(int i = 0; i < level; i++)
            {
                if(maxLevel == level)
                    cost -= 1;
                else
                {
                    defensPower += card.upgradeExtra;
                }   
            }
            break;
        default:
            cardValue += card.upgradeValue * level;
            defensPower += card.upgradeExtra* level;
            break;
        }
    }

    protected override void Using(GameObject ob)
    {
        ShowMonster monster = ob.GetComponent<ShowMonster>();
        switch (skill)
        {
        case SkILL.FIRE:
            monster.Fire += cardValue;
            monster.PowerDown();
                if (monster.fireParticle == null)
                {
                    monster.fireParticle = Instantiate(Resources.Load("Particles/Fire Particle System") as GameObject).GetComponent<ParticleSystem>();
                    monster.fireParticle.transform.position = monster.transform.position;
                }
                SoundManager.Instance.PlaySFX(SoundManager.SFXList.BURNING);
                break;
        case SkILL.POISION:
            monster.Poision += cardValue;
                if (monster.poisionParticle == null)
                {
                    monster.poisionParticle = Instantiate(Resources.Load("Particles/Toxin Particle System") as GameObject).GetComponent<ParticleSystem>();
                    monster.poisionParticle.transform.position = monster.transform.position;
                }
                SoundManager.Instance.PlaySFX(SoundManager.SFXList.GLASS_BREAK);
                break;
        case SkILL.LIGHTING:
            monster.Lighting += cardValue;
                if (monster.lightingParticle == null)
                {
                    monster.lightingParticle = Instantiate(Resources.Load("Particles/Lighting Particle System") as GameObject).GetComponent<ParticleSystem>();
                    monster.lightingParticle.transform.position = monster.transform.position;
                }
                SoundManager.Instance.PlaySFX(SoundManager.SFXList.THUNDER);
                break;
        case SkILL.DEBUFF:
            monster.Fire = (monster.Fire+cardValue) * 2;
            monster.Lighting = (monster.Lighting+cardValue) * 2;
            monster.Poision = (monster.Poision+cardValue) * 2;
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
