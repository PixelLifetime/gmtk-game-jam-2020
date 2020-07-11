using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Damageable
{
    public enum EnemyBehavior { IDLE, ROAM, CHASE, ATTACK }

    protected EnemyBehavior currentBehavior; // What the enemy is currently doing
    protected GameObject player;
    protected Node[] NodeGrid;
    protected float moveSpeed;
    protected float attackRange;
    protected bool aggressive; // An aggresive enemy will attack the player if it enter its range, whereas a non aggressive one will not.
    protected bool isFacingRight;
    protected Vector3 velocity = Vector3.zero;
    protected bool grounded;
    protected Node currNode;
    protected Node targetNode;
    protected bool canJump;
    protected bool canFly;

    private bool _attacking;
    private bool _jumping;

    private const float _groundedRadius = .2f;
    Vector3 _groundCheckOffset = new Vector3(0, -0.39f, 0);
    [SerializeField]
    private LayerMask _groundLayer;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<Node>() != null)
        {
            currNode = collider.gameObject.GetComponent<Node>();
        }
        
        targetNode = GetNextTargetNode();
        //Debug.Log("Switched target node to node " + targetNode.gameObject.name);
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        OnTriggerEnter2D(collider);
    }

    protected void TurnAround()
    {
        isFacingRight = (isFacingRight) ? false : true;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    protected void Jump(float jumpForce)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
        grounded = false;
    }

    void FixedUpdate()
    {
        if (aggressive && IsPlayerInAttackRange())
        {
            currentBehavior = EnemyBehavior.ATTACK;
        }
        UpdateGroundedState();
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
                if (_attacking)
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

    private void UpdateGroundedState()
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + _groundCheckOffset, _groundedRadius, _groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                return;
            }
        }
        grounded = false;
    }

    protected Node GetNextTargetNode()
    {
        // First we find which node the player is closest to
        Node playerNode = FindClosestNode(player.transform.position);

        // Then we apply bfs to find the shortest path to the target node
        Queue<Node> needToCheck = new Queue<Node>();
        needToCheck.Enqueue(playerNode);
        Dictionary<Node, int> distance = new Dictionary<Node, int>();
        distance[playerNode] = 0;

        while(needToCheck.Count > 0)
        {
            Node curr = needToCheck.Dequeue();
            AddAdjacentNodes(curr.GetAdjacentByFoot(), needToCheck, distance, curr);
            if (canJump || canFly)
            {
                AddAdjacentNodes(curr.GetAdjacentByJump(), needToCheck, distance, curr);
            }
            if (canFly)
            {
                AddAdjacentNodes(curr.GetAdjacentByFlight(), needToCheck, distance, curr);
            }
        }
        
        Node targetNode = GetMinimumNodeDistance(null, currNode.GetAdjacentByFoot(), distance);
        if(canJump || canFly)
        {
            targetNode = GetMinimumNodeDistance(targetNode, currNode.GetAdjacentByJump(), distance);
        }
        if (canFly)
        {
            targetNode = GetMinimumNodeDistance(targetNode, currNode.GetAdjacentByFlight(), distance);
        }

        return targetNode;
    }

    private Node GetMinimumNodeDistance(Node currMin, GameObject[] nodeObjects, Dictionary<Node, int> distance)
    {
        foreach (GameObject go in nodeObjects)
        {
            Node node = go.GetComponent<Node>();
            if (currMin == null || distance[node] < distance[currMin])
            {
                currMin = node;
            }
        }

        return currMin;
    }

    private void AddAdjacentNodes(GameObject[] nodeObjects, Queue<Node> needToCheck, Dictionary<Node, int> distance, Node curr)
    {
        foreach (GameObject go in nodeObjects)
        {
            Node adjacent = go.GetComponent<Node>();
            if (!distance.ContainsKey(adjacent))
            {
                needToCheck.Enqueue(adjacent);
                distance[adjacent] = 1 + distance[curr];
            }
        }
    }

    protected Node FindClosestNode(Vector2 position)
    {
        Node closest = NodeGrid[0];
        float closestDist = 100000000;
        
        foreach(Node node in NodeGrid)
        {
            float xDiff = Mathf.Abs(node.gameObject.transform.position.x - position.x);
            float yDiff = Mathf.Abs(node.gameObject.transform.position.y - position.y);
            if( (xDiff + yDiff) < closestDist)
            {
                closest = node;
                closestDist = xDiff + yDiff;
            }
        }

        return closest;
    }

    protected bool IsPlayerInAttackRange()
    {
        float xDiff = Mathf.Abs(player.transform.position.x - transform.position.x);
        float yDiff = Mathf.Abs(player.transform.position.y - transform.position.y);
        return (xDiff + yDiff) < attackRange;
    }

    private bool CollidedWithGround(int collisionLayer)
    {
        return collisionLayer == 8;
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
