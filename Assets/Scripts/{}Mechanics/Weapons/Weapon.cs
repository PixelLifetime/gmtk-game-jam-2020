using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float cooldown;
    public int damage;

    protected bool _canAttack;

    private void Start()
    {
        _canAttack = true;
    }
    public void Attack()
    {
        DoAttack();

    }
    public void SecondaryAttack()
    {
        DoSecondaryAttack();
    }

    protected abstract void DoAttack();
    protected abstract void DoSecondaryAttack();

    protected IEnumerator AttackCooldown(float? cooldown = null)
    {
        var usedCooldown = cooldown!=null? cooldown.Value : this.cooldown;
        yield return new WaitForSeconds(usedCooldown);
        _canAttack = true;
    }
}
