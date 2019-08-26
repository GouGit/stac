using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ShowMonster : MonoBehaviour
{
    public Monster mon;
    protected enum ACTION
    {
        NONE,
        ATTACK,
        DEFENS,
        END
    }
    public Canvas uiCanvas;
    public GameObject ui;
    protected GameObject hpUI, attackUI, defensUI, defensOnUI;
    public Type.TYPE type;
    protected ACTION action;
    protected new string name;
    public int hp;
    public int attackPower;
    public int defensPower, tempDefens;
    public int ondefensPower = 0;
    public int fire, poision, lighting;
    public bool isDont = false;
    protected bool isAttack;
    protected bool shaking = false;
    protected float shakePower;
    public UnityEvent OnMonsterDead;
    private Vector3 origin;
    protected GameObject hitParticle, hitTemp;
    protected int temPower;

    protected virtual void Start()
    {
        hitParticle = Resources.Load("HitParticleSystem") as GameObject;
        GetComponent<SpriteRenderer>().sprite = mon.image;
        name = mon.name;
        hp = mon.hp;
        attackPower = temPower = mon.attackPower;
        defensPower = tempDefens = mon.defensPower;
        type = mon.type;
        action = ACTION.NONE;
        isAttack = true;
        origin = transform.position;

        uiCanvas.worldCamera = Camera.main;
        SetMonster set = uiCanvas.GetComponent<SetMonster>();
        set.Set(this);
        ui = Instantiate(uiCanvas.gameObject, Vector3.zero, Quaternion.identity);
        ui.SetActive(true);

        hpUI = ui.transform.GetChild(0).gameObject;
        hpUI.transform.position = transform.position + Vector3.down*2.5f;
        defensOnUI = ui.transform.GetChild(1).gameObject;
        defensOnUI.transform.position = transform.position + Vector3.down*2f + Vector3.left*1.5f;
        attackUI = ui.transform.GetChild(2).gameObject;
        attackUI.transform.position = transform.position + Vector3.down*2f + Vector3.right*1.5f;
        defensUI = ui.transform.GetChild(3).gameObject;
        defensUI.transform.position = transform.position + Vector3.down*2f + Vector3.right*1.5f;

        hpUI.SetActive(true);
        attackUI.SetActive(isAttack);
        defensUI.SetActive(!isAttack);

        Vector3 scale = new Vector3(0.8f,0.8f,1);
        transform.localScale = scale;

        hitTemp = Instantiate(hitParticle, transform.position, Quaternion.identity);
        hitTemp.SetActive(false);

        OnMonsterDead.AddListener(GameManager.instance.OnGameEnd);
    }

    protected IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }

    void MyTurn()
    {
        ChangeState();
        attackUI.SetActive(isAttack);
        defensUI.SetActive(!isAttack);

        ondefensPower = 0;
        Vector3 scale = new Vector3(1,1,1);
        transform.localScale = scale;
        if(lighting != 0)
        {
            if(Random.Range(0,10) < 5)
            {
                StartCoroutine(WaitTime());
                action = ACTION.END;
            }
            lighting--;
        }
        if(fire!=0)
        {
            attackPower = temPower/2;
        }
        else
        {
            attackPower = temPower;
        }

        if(isDont)
        {
            defensPower = 0;
        }
    }

    protected virtual void EndTurn()
    {
        if(fire != 0)
        {
            LoseHp(fire);
            fire--;
        }
        if(poision != 0)
        {
            int defens = ondefensPower;
            ondefensPower = 0;
            LoseHp(poision);
            poision--;
            ondefensPower = defens;
        }
        action = ACTION.NONE;
        isDont = false;
        defensPower = tempDefens;
        GameManager.instance.isPlayerTurn = true;
        GameManager.instance.cost = 3 + GameManager.instance.savingCost;
        GameManager.instance.savingCost = 0;
        Knight.instance.defensPower = 0;
        Knight.instance.isReflect = false;
        Knight.instance.DrawCard();
        Vector3 scale = new Vector3(0.8f,0.8f,1);
        transform.localScale = scale;
    }

    protected void Shake()
    {
        if(shaking)
        {
            Vector3 newpos = Random.insideUnitSphere * (Time.deltaTime * shakePower);
            newpos.x = transform.position.x + newpos.x;
            newpos.y = transform.position.y;
            newpos.z = transform.position.z;
            transform.position = newpos;
        }

        if(ondefensPower > 0)
        {
            defensOnUI.SetActive(true);
        }
        else
        {
            defensOnUI.SetActive(false);
        }
    }

    protected IEnumerator Shaking()
    {
        if(!shaking)
        {
            shaking = true;
        }
        hitTemp.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        shaking = false;
        hitTemp.SetActive(false);
        transform.position = origin;
    }

    public virtual void LoseHp(int damage)
    {
        if(ondefensPower > 0)
        {
            int defens = ondefensPower - damage;
            ondefensPower = defens;
            if(ondefensPower < 0)
            {
                shakePower = Mathf.Abs(ondefensPower);
                hp += ondefensPower;
                StartCoroutine(Shaking());
            }
        }
        else
        {
            shakePower = damage;
            hp -= damage;
            StartCoroutine(Shaking());
        }
        SoundManager.Instance.PlaySFX(SoundManager.SFXList.MONSTER_DAMAGE);
        
        if(hp <= 0)
        {
            ui.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        shaking = false;
    }

    void OnDisable()
    {
        if(GameManager.instance.monsterOption.IsEnd())
        {
            GameManager.instance.monsterOption.AllMonsters.Clear();
            OnMonsterDead?.Invoke();
        }
    }

    protected virtual void ChangeState()
    {
        if(isAttack)
        {
            action = ACTION.ATTACK;
        }
        else
        {
            action = ACTION.DEFENS;
        }
        isAttack = !isAttack;
    }

    protected virtual void Attack()
    {
        if(Knight.instance.isReflect)
        {
            LoseHp(attackPower);
        }
        Knight.instance.LoseHp(attackPower);
    }

    protected virtual void Defens()
    {
        ondefensPower += defensPower;
    }

    void Update()
    {
        Shake();

        if(GameManager.instance.isPlayerTurn)
            return;
        
        switch (action)
        {
        case ACTION.NONE:
            MyTurn();
            break;
        case ACTION.ATTACK:
            Attack();
            action = ACTION.END;
            StartCoroutine(WaitTime());
            break;
        case ACTION.DEFENS:
            Defens();
            action = ACTION.END;
            StartCoroutine(WaitTime());
            break;
        case ACTION.END:
            break;
        }
    }
}
