using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Damageable
{
    #region Variables
    public enum EnemyBehavior { IDLE, ROAM, CHASE, ATTACK }

    protected EnemyBehavior currentBehavior; // What the enemy is currently doing
    protected GameObject player;
    protected Node[] nodeGrid;
    [SerializeField]
    protected float moveSpeed = 4.0f;
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
    private Vector2 _lastGroundedPosition;


    private const float _groundedRadius = .2f;
    Vector3 _groundCheckOffset = new Vector3(0, -0.39f, 0);

    [SerializeField]
    private LayerMask _groundLayer;
    #endregion

    public void InitializeEnemyData(Node[] nodeGrid, GameObject player, EnemyBehavior enemyBehavior)
    {
        this.nodeGrid = nodeGrid;
        this.player = player;
        currentBehavior = enemyBehavior;
        currNode = FindClosestNode(transform.position);
        targetNode = currNode;
    }

    #region Abstract Methods
    protected abstract Vector2 GetSearchLocation();
    protected abstract void IdleBehavior();
    protected abstract void RoamBehavior();
    protected abstract void ChaseBehavior();
    protected abstract void AttackBehavior();

    #endregion

    #region Movement Methods
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Only update if the trigger hit a node
        if(collider.gameObject.GetComponent<Node>() != null)
        {
            currNode = collider.gameObject.GetComponent<Node>();
        }
        
        targetNode = GetNextTargetNode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Lose();
        }
    }

    protected void TurnAround()
    {
        isFacingRight = (isFacingRight) ? false : true;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
    }

    protected void Jump(float jumpForce)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
        grounded = false;
    }

    void FixedUpdate()
    {
        UpdateGroundedState();
        if (grounded)
        {
            _lastGroundedPosition = transform.position;
        }
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
                if (_attacking)
                {
                    return;
                }
                AttackBehavior();
                break;
        }
    }
    #endregion

    #region Target Acquisition
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
        Dictionary<Node, float> distance = new Dictionary<Node, float>();
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

    private Node GetMinimumNodeDistance(Node currMin, GameObject[] nodeObjects, Dictionary<Node, float> distance)
    {
        foreach (GameObject go in nodeObjects)
        {
            Node node = go.GetComponent<Node>();
            if (currMin == null || distance[node] < distance[currMin])
            {
                currMin = node;
            }
            else if(distance[node] == distance[currMin] && Random.Range(0, 1) == 1)
            {
                currMin = node;
            }
            
        }

        return currMin;
    }

    private void AddAdjacentNodes(GameObject[] nodeObjects, Queue<Node> needToCheck, Dictionary<Node, float> distance, Node curr)
    {
        foreach (GameObject go in nodeObjects)
        {
            Node adjacent = go.GetComponent<Node>();
            if (!distance.ContainsKey(adjacent))
            {
                needToCheck.Enqueue(adjacent);
                distance[adjacent] = GetDistanceBetweenTwoVectors(adjacent.transform.position, curr.transform.position) + distance[curr];
            }
        }
    }

    private float GetDistanceBetweenTwoVectors(Vector2 v1, Vector2 v2)
    {
        float xDiff = Mathf.Abs(v1.x - v2.x);
        float yDiff = Mathf.Abs(v1.y - v2.y);

        return xDiff + yDiff;
    }

    protected Node FindClosestNode(Vector2 position)
    {
        Node closest = nodeGrid[0];
        float closestDist = 100000000;
        
        foreach(Node node in nodeGrid)
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
    #endregion

    void OnMouseDown()
    {
        Debug.Log("Hey I'm " + gameObject.name + ". I'm currently on node " + currNode.gameObject.name + " and I'm headed to node " + targetNode.gameObject.name);
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
