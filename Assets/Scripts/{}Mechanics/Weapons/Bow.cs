using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    public int chargedShotDamage;
    public float chargedShotCooldown;
    public Arrow arrowPrefab;
    protected override void DoAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            var arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
            var facingRight = transform.lossyScale.x > 0;
            arrow.transform.localScale = new Vector3(
                arrow.transform.localScale.x * (facingRight ? 1 : -1)
                , arrow.transform.localScale.y,
                arrow.transform.localScale.z);
            arrow.damage = damage;
            StartCoroutine(AttackCooldown());
        }
    }

    protected override void DoSecondaryAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            var arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
            var facingRight = transform.lossyScale.x > 0;
            arrow.transform.localScale = new Vector3(
                arrow.transform.localScale.x * (facingRight ? 1 : -1)
                , arrow.transform.localScale.y,
                arrow.transform.localScale.z);
            arrow.damage = chargedShotDamage;
            arrow.isCharged = true;
            StartCoroutine(AttackCooldown(chargedShotCooldown));
        }
    }
}
