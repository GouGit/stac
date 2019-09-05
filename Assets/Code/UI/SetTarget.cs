using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarget : MonoBehaviour
{
    public ShowMonster monster;

    public virtual void Set(ShowMonster mon)
    {
        monster = mon;
    }

}
