﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{

    void Awake()
    {
        currentBehavior = EnemyBehavior.ROAM;
        aggressive = true;
        attackRange = 1.5f;
        moveSpeed = 4f;
        isFacingRight = false;
        canJump = true;
        DOTween.ToAlpha(() => GetComponentInChildren<SpriteRenderer>().color, x => GetComponentInChildren<SpriteRenderer>().color = x, 1, 1);
    }

    protected override void AttackBehavior()
    {
        if (!IsPlayerInAttackRange())
        {
            currentBehavior = EnemyBehavior.CHASE;
        }
        float xVelocity = moveSpeed;
        if (player.transform.position.x < transform.position.x)
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
        if (grounded && player.transform.position.y > transform.position.y)
        {
            float jumpHeight = Mathf.Abs(targetNode.transform.position.y - transform.position.y) * 5f;
            jumpHeight = Mathf.Max(jumpHeight, 12);
            Jump(jumpHeight);
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 targetVelocity = new Vector2(xVelocity, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.3f);
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
        if(grounded && targetNode.transform.position.y > transform.position.y)
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
        return player.transform.position;
    }
}
