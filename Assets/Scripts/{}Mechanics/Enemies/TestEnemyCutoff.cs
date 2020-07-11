﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyCutoff : Enemy
{
    void Awake()
    {
        player = GameObject.Find("[Player] Main"); // Change to recieve from spawner once spawner is implemented
        NodeGrid = GameObject.Find("[AI] Node Grid").GetComponentsInChildren<Node>(); // Change to recieve from spawner once spawner is implemented
        currentBehavior = EnemyBehavior.CHASE;
        currNode = FindClosestNode(transform.position);
        targetNode = currNode;
        aggressive = true;
        attackRange = 0.5f;
        moveSpeed = 4f;
        isFacingRight = false;
        canJump = true;
    }

    protected override void AttackBehavior()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChaseBehavior()
    {
        float xVelocity = moveSpeed;
        if (targetNode.transform.position.x < transform.position.x)
        {
            xVelocity = -xVelocity;
            if (isFacingRight)
            {
                TurnAround();
            }
        }
        else
        {
            if (!isFacingRight)
            {
                TurnAround();
            }
        }
        if (grounded && targetNode.transform.position.y > transform.position.y)
        {
            float jumpHeight = Mathf.Abs(targetNode.transform.position.y - transform.position.y) * 5f;
            jumpHeight = Mathf.Max(jumpHeight, 10);
            Jump(jumpHeight);
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

    protected override Vector2 GetSearchLocation()
    {
        float displacement = (player.transform.localScale.x == 1) ? player.transform.position.x + 3f : player.transform.position.x - 3f;

        return new Vector2(displacement, player.transform.position.y);
    }
}
