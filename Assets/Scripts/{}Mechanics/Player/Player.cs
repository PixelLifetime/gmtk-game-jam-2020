using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable
{
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
