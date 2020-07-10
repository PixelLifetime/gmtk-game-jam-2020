using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Damageable : MonoBehaviour
{
    public int maxHealth;
    public float recoveryTime;
    public float invincibilityTime;
    public UnityEvent OnHit; 
    public UnityEvent OnHitRecovery;
    private Rigidbody2D _rigidbody2D;
    private bool isInvincible;
    private int _health;
    private void Start()
    {
        _health = maxHealth;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (OnHit == null)
            OnHit = new UnityEvent();
        if (OnHitRecovery == null)
            OnHitRecovery = new UnityEvent();
    }

    public void Heal(int health)
    {
        _health += health;
        _health = Mathf.Min(_health, maxHealth);
    }
    public void Damage(int damage, Transform damager = null)
    {
        if (!isInvincible)
        {
            if (_rigidbody2D != null)
            {
                var direction = Vector3.right * Mathf.Sign(transform.position.x - damager.position.x);
                _rigidbody2D.AddForce(direction * damage / 5f + transform.up * 5f, ForceMode2D.Impulse);
            }
            OnHit.Invoke();
            _health -= damage;
            if (_health <= 0)
            {
                OnDeath();
            } 
            else
            {
                StartCoroutine(HitCooldown());
                StartCoroutine(RecoveryCooldown());
            }
        }
    }
    private IEnumerator RecoveryCooldown()
    {
        yield return new WaitForSeconds(recoveryTime);
        OnHitRecovery.Invoke();
    }

    private IEnumerator HitCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
    protected abstract void OnDeath();
}
