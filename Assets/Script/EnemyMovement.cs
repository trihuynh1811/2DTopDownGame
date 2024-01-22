using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour, ITakeDamage
{
    [SerializeField] GameObject player;

    [SerializeField] float health;
    public float avoidanceRadius = 1.5f;
    public float avoidanceStrength;
    Vector2 avoidanceForce;

    public LineRenderer line;
    public float lineLength;
    public LayerMask laserHitMask, enemyLayerMask;
    [SerializeField] Transform laserPoint;
    [SerializeField] float laserRotationSpeed;
    [SerializeField] float transformRotationSpeed;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip movingAnimation;
    [SerializeField] AnimationClip attackAnimation;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackRange;
    [SerializeField] float followRange;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] LayerMask wallMask;
    [SerializeField] Vector2 rotation;
    public Seeker seeker;
    public float nextWayPointDistance;

    Path path;
    int currentWayPoint;
    bool reachedEndOfPath;

    float initialYRotation = 180;
    float distanceToPlayer;
    float laserDistance;
    float currentMoveSpeed;
    Vector2 direction;
    RaycastHit2D hit;

    void Start()
    {
        player = GameObject.Find("Player");
        currentMoveSpeed = moveSpeed;

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
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

    void Update()
    {
        LookAtPlayer();
        LaserRotateTowardPlayer();
        //myRigidbody.velocity = new Vector2 (moveSpeed, 0f);
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    void LookAtPlayer()
    {
        //if(player.transform.position.x > transform.position.x)
        //{
        //    transform.localRotation = new Quaternion(0f, 0, 0f, 0f);
        //}
        //else if(player.transform.position.x < transform.position.x)
        //{
        //    transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
        //}

        // Determine the direction from the enemy to the player
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // Calculate the target rotation angle in degrees
        float targetRotation = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Create the target rotation quaternion
        Quaternion targetQuaternion = Quaternion.Euler(0f, (directionToPlayer.x > 0f) ? 0f : 180f, 0f);

        // Rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, transformRotationSpeed * Time.deltaTime);

    }

    void FlipEnemyFacing()
    {
        transform.localRotation = Quaternion.Euler(rotation);
    }

    void FollowPlayer()
    {
        distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);

        if (distanceToPlayer > followRange)
        {
            currentMoveSpeed = 0;
            animator.Play(idleAnimation.name);
        }
        else
        {
            if (distanceToPlayer <= attackRange)
            {
                currentMoveSpeed = 0;
                animator.Play(idleAnimation.name);
            }
            else
            {
                currentMoveSpeed = moveSpeed;
                animator.Play(movingAnimation.name);
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
        Vector2 force = currentMoveSpeed * Time.deltaTime * direction;

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




    void LaserRotateTowardPlayer()
    {
        Vector3 differance = player.transform.position - laserPoint.position;
        float rotZ = Mathf.Atan2(differance.y, differance.x) * Mathf.Rad2Deg;
        Quaternion targetQuaternion = Quaternion.Euler(0f, 0f, rotZ);

        // Rotate towards the target rotation
        laserPoint.rotation = Quaternion.RotateTowards(laserPoint.rotation, targetQuaternion, laserRotationSpeed * Time.deltaTime);
        hit = Physics2D.Raycast(laserPoint.position, laserPoint.right, lineLength, laserHitMask);
        if (hit)
        {
            if (hit.collider.CompareTag("Ground/Wall"))
            {
                distanceToPlayer = ((Vector2)hit.point - (Vector2)laserPoint.position).magnitude;
            }
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponentInParent<TopDownPlayerMovement>().TakeDamage();
            }
        }
        else
        {
            distanceToPlayer = lineLength;
        }
        line.SetPosition(1, new Vector2(distanceToPlayer, 0));

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
    void Patrol()
    {
        if (Physics2D.Raycast(transform.position, transform.right, .5f, wallMask))
        {
            moveSpeed *= -1;
            initialYRotation *= -1;
            rotation.y -= initialYRotation;
            FlipEnemyFacing();
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }
}
