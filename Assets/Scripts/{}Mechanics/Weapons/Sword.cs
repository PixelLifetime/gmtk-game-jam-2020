using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public int heavyDamage;
    public float heavyCooldown;
    public float attackRadius;
    public LayerMask enemiesLayer;

    protected override void DoAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            var enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemiesLayer);
            foreach (var enemy in enemies)
            {
                var damageable = enemy.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.Damage(damage, transform);
                }
            }
            StartCoroutine(AttackCooldown());
        }
    }

    protected override void DoSecondaryAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            var enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemiesLayer);
            foreach (var enemy in enemies)
            {
                var damageable = enemy.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.Damage(heavyDamage, transform);
                }
            }
            StartCoroutine(AttackCooldown(heavyCooldown));
        }
    }
}
