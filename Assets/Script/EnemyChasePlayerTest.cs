using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//public class EnemyChasePlayerTest : MonoBehaviour
//{
//    public float speed = 10.0f;
//    public float diststop = 1.5f;

//    public Transform target;
//    private Rigidbody2D rb;
//    public float avoidanceRadius = 2.0f;
//    public float avoidanceForce;

//    private Vector2 Position
//    {
//        get
//        {
//            return transform.position;
//        }
//        set
//        {
//            transform.position = value;
//        }
//    }

//    private void Start()
//    {
//        target = GameObject.Find("Player").transform;
//        rb = GetComponent<Rigidbody2D>();
//    }

//    private void FixedUpdate()
//    {
//        float dist = Vector2.Distance(Position, target.position);
//        float step;
//        Vector2 avoidanceVector = AvoidOtherEnemies();

//        if (dist <= diststop)
//        {
//            step = 0;
//        }
//        else
//        {
//            step = speed * Time.deltaTime;
//        }
//        Position = Vector2.MoveTowards(Position, (Vector2)target.position + avoidanceVector, step);
//        rb.MovePosition(Position);
//    }
//    private Vector2 AvoidOtherEnemies()
//    {
//        Vector2 avoidanceVector = Vector2.zero;

//        Collider2D[] colliders = Physics2D.OverlapCircleAll(Position, avoidanceRadius);
//        foreach (Collider2D collider in colliders)
//        {
//            if (collider != null && collider.gameObject != gameObject && collider.CompareTag("Enemy"))
//            {
//                Vector2 avoidDirection = (Position - (Vector2)collider.transform.position).normalized * avoidanceForce;
//                avoidanceVector += avoidDirection;
//            }
//        }

//        return avoidanceVector.normalized;
//    }
//}

public class EnemyChasePlayerTest : MonoBehaviour
{
    [SerializeField] float transformRotationSpeed, followRange, attackRange;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deadAnimation;
    [SerializeField] Collider2D c2d;
    [SerializeField] bool rotateX, rotateY, rotateZ, homingLikeRotate;

    public float speed = 5f;
    public float avoidanceRadius = 2f;
    public float avoidanceForce = 2f;
    public Transform player;
    public Rigidbody2D rb;
    public Seeker seeker;
    public float nextWayPointDistance;

    Path path;
    int currentWayPoint;
    float currentSpeed;
    bool reachedEndOfPath;
    float distanceToPlayer;
    bool isDead;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        currentSpeed = speed;
        GameManager.spawnedMonsterList.Add(gameObject);
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && !isDead)
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void Update()
    {
        if (GameManager.endOfWave)
        {
            isDead = true;
            Dead();
        }
        if (!isDead)
        {
            LookAtPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            FollowPlayer();
        }
    }

    void LookAtPlayer()
    {
        if (homingLikeRotate)
        {
            Vector2 direction = (Vector2)player.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            rb.angularVelocity = -rotateAmount * transformRotationSpeed;
        }
        else
        {
            // Determine the direction from the enemy to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // Create the target rotation quaternion
            Quaternion targetQuaternion = Quaternion.Euler(
                rotateX ? (directionToPlayer.x > 0f) ? 0f : 180f : 0f,
                rotateY ? (directionToPlayer.x > 0f) ? 0f : 180f : 0f,
                rotateZ ? (directionToPlayer.x > 0f) ? 0f : 180f : 0f);

            // Rotate towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, transformRotationSpeed * Time.deltaTime);
        }

    }

    void FollowPlayer()
    {
        distanceToPlayer = Vector2.Distance(rb.position, player.position);
        if (distanceToPlayer > followRange)
        {
            currentSpeed = 0;
        }
        else
        {
            if (distanceToPlayer <= attackRange)
            {
                currentSpeed = 0;
            }
            else
            {
                currentSpeed = speed;
            }
        }

        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = currentSpeed * Time.deltaTime * direction;

        rb.AddForce(force);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(rb.position, avoidanceRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject && collider.CompareTag("Enemy"))
            {
                // Calculate the avoidance direction and adjust the movement
                Vector2 avoidDirection = (rb.position - (Vector2)collider.transform.position).normalized;
                Vector2 forceToAvoid = avoidDirection * avoidanceForce;

                // Apply the avoidance force using Rigidbody2D.AddForce
                rb.AddForce(forceToAvoid);
            }
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }
    }

    void Dead()
    {
        animator.Play(deadAnimation.name);
        c2d.enabled = false;
        rb.freezeRotation = true;
        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}






