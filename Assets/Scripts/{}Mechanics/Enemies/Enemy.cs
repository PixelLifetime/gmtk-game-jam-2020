using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Damageable
{
    public enum EnemyBehavior { IDLE, ROAM, CHASE, ATTACK }

    protected EnemyBehavior currentBehavior; // What the enemy is currently doing
    protected GameObject player;
    protected float moveSpeed;
    protected float attackRange;
    protected bool aggressive; // An aggresive enemy will attack the player if it enter its range, whereas a non aggressive one will not.
    protected bool isFacingRight;
    protected Vector3 velocity = Vector3.zero;

    private bool _isAttacking;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(CollidedWithEdge(collider.gameObject.layer) && currentBehavior == EnemyBehavior.ROAM)
        {
            isFacingRight = (isFacingRight) ? false : true;
        }
    }

    void FixedUpdate()
    {
        if (aggressive && IsPlayerInAttackRange())
        {
            currentBehavior = EnemyBehavior.ATTACK;
        }

        switch (currentBehavior)
        {
            case EnemyBehavior.IDLE:
                IdleBehavior();
                break;
            case EnemyBehavior.ROAM:
                RoamBehavior();
                break;
            case EnemyBehavior.CHASE:
                ChaseBehavior();
                break;
            case EnemyBehavior.ATTACK:
                if (_isAttacking)
                {
                    return;
                }
                AttackBehavior();
                break;
        }
    }

    protected abstract void IdleBehavior();
    protected abstract void RoamBehavior();
    protected abstract void ChaseBehavior();
    protected abstract void AttackBehavior();
        
    protected bool IsPlayerInAttackRange()
    {
        float xDiff = Mathf.Abs(player.transform.position.x - transform.position.x);
        float yDiff = Mathf.Abs(player.transform.position.y - transform.position.y);
        return (xDiff + yDiff) < attackRange;
    }

    private bool CollidedWithEdge(int collisionLayer)
    {
        return collisionLayer == 9;
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
