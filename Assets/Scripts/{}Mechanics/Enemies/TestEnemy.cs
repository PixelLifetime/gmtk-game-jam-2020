using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{

    void Awake()
    {
        player = GameObject.Find("[Player] Main"); // Change to recieve from spawner once spawner is implemented
        currentBehavior = EnemyBehavior.CHASE;
        aggressive = true;
        attackRange = 0.5f;
        moveSpeed = 4f;
        isFacingRight = false;
    }

    protected override void AttackBehavior()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChaseBehavior()
    {
        float xVelocity = moveSpeed;
        if (player.transform.position.x < transform.position.x)
        {
            xVelocity = -xVelocity;
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = new Vector2(xVelocity, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.3f);
    }

    protected override void IdleBehavior()
    {
        return;
    }

    protected override void RoamBehavior()
    {
        float xVelocity = moveSpeed;
        if (!isFacingRight)
        {
            xVelocity = -xVelocity;
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = new Vector2(xVelocity, rb.velocity.y);
        
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.3f);
    }
}
