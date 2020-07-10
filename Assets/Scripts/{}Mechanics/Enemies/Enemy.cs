using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
