using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    public float attackRadius;
    public int thrownDamage;
    public LayerMask enemiesLayer;
    public ThrownSpear spearPrefab;
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

    public void PickUp()
    {
        _canAttack = true;
    }

    protected override void DoSecondaryAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            var spear = Instantiate(spearPrefab, transform.position, transform.rotation);
            var facingRight = transform.lossyScale.x > 0;
            spear.transform.localScale = new Vector3(
                spear.transform.localScale.x * (facingRight ? 1 : -1)
                , spear.transform.localScale.y,
                spear.transform.localScale.z);
            spear.damage = thrownDamage;
            spear.spear = this;
        }
    }
}
