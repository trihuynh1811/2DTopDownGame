using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class EnemyChasePlayerTest : MonoBehaviour
{
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] BossAttack bossAttack;
    [SerializeField] EnemyTakeDmg takeDmg;

    public float transformRotationSpeed, followRange, attackRange;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deadAnimation, idleAnimation, walkAnimation;
    public Collider2D c2d;
    [SerializeField] bool rotateX, rotateY, rotateZ, homingLikeRotate, explodeWhenDie, instantRotate, pushWhenHitWall;

    public float speed = 5f;
    public float avoidanceRadius = 2f;
    public float avoidanceForce = 2f;
    public Transform player;
    public Rigidbody2D rb;
    public Seeker seeker;
    public float nextWayPointDistance;
    [SerializeField] int numberOfItemToSpawn;
    [SerializeField] List<GameObject> itemList;
    [SerializeField] Vector2 randomItemPos;

    Path path;
    int currentWayPoint;
    float currentSpeed;
    bool reachedEndOfPath;
    float distanceToPlayer;
    bool isDead;

    private void Awake()
    {
    }

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
        if (GameManager.endOfWave || takeDmg.health <= 0)
        {
            isDead = true;
            Dead();
        }
        if (!isDead)
        {
            LookAtPlayer();
        }
        if (takeDmg.healtCanvas != null)
        {
            takeDmg.HealthCanvasFollow();
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
            if (instantRotate)
            {
                if (player.transform.position.x > transform.position.x)
                {
                    transform.localRotation = new Quaternion(0f, 0, 0f, 0f);
                }
                else if (player.transform.position.x < transform.position.x)
                {
                    transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
                }
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

    }

    void FollowPlayer()
    {
        distanceToPlayer = Vector2.Distance(rb.position, player.position);
        if (distanceToPlayer > followRange)
        {
            currentSpeed = 0;
            if (idleAnimation != null)
                animator.Play(idleAnimation.name);
        }
        else
        {
            if (distanceToPlayer <= attackRange)
            {
                currentSpeed = 0;
                if (idleAnimation != null)
                    animator.Play(idleAnimation.name);
                if (enemyAttack != null)
                    enemyAttack.Attack();
            }
            else
            {
                if (idleAnimation != null)
                    animator.Play(walkAnimation.name);
                if (enemyAttack != null)
                    enemyAttack.DisableAttack();
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
            if ((collider != null && collider.gameObject != gameObject && collider.CompareTag("Enemy")))
            {
                // Calculate the avoidance direction and adjust the movement
                Vector2 avoidDirection = (rb.position - (Vector2)collider.transform.position).normalized;
                Vector2 forceToAvoid = avoidDirection * avoidanceForce;

                // Apply the avoidance force using Rigidbody2D.AddForce
                rb.AddForce(forceToAvoid);
            }
            if(pushWhenHitWall && collider.gameObject != gameObject && collider.CompareTag("Ground/Wall")){
                // Calculate the avoidance direction and adjust the movement
                Vector2 avoidDirection = (rb.position - (Vector2)collider.transform.position).normalized;
                Vector2 forceToAvoid = avoidDirection * 10000;

                // Apply the avoidance force using Rigidbody2D.AddForce
                rb.AddForce(-forceToAvoid);
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
        if(takeDmg.health <= 0)
        {
            TopDownPlayerMovement.instance.monsterKill++;
        }
        if(itemList.Where(x => x.GetComponent<ItemStat>().dropOneTime).Any())
        {
            List<GameObject> oneItemList = itemList.Where(x => x.GetComponent<ItemStat>().dropOneTime).ToList();
            for (int i = 0; i < oneItemList.Count; i++)
            {
                Vector2 randomPos = new(transform.position.x + Random.Range(-randomItemPos.x, randomItemPos.x), transform.position.y + Random.Range(-randomItemPos.y, randomItemPos.y));
                GameObject item = Instantiate(oneItemList[i], randomPos, oneItemList[i].transform.rotation);
                Debug.Log(itemList.RemoveAll(item => item.GetComponent<ItemStat>().dropOneTime));
            }
        }
        if (bossAttack != null && bossAttack.bossType == BossAttack.Boss.Death)
        {
            bossAttack.leftEyeTrail.SetActive(false);
            bossAttack.rightEyeTrail.SetActive(false);
            GameManager.instance.time = 0;
            GameManager.instance.timerText.text = GameManager.instance.time.ToString();
            GameManager.instance.timerRunning = false;
            GameManager.instance.triggerNewWaveObject.SetActive(false);
            GameManager.endOfWave = true;
            TopDownPlayerMovement.instance.Win();
        }
        if (animator != null)
        {
            if (deadAnimation != null)
            {
                animator.Play(deadAnimation.name);
            }
        }
        if (explodeWhenDie)
        {
            if(enemyAttack != null)
            {
                enemyAttack.Explode();
            }
            if(bossAttack != null)
            {
                bossAttack.Explode();
            }
        }
        if(GameManager.instance.time > 0)
        {
            int randomAmount = Random.Range(1, numberOfItemToSpawn + 1);
            for (int i = 0; i < randomAmount; i++)
            {
                int randomIndex = Random.Range(0, itemList.Count);
                Vector2 randomPos = new(transform.position.x + Random.Range(-randomItemPos.x, randomItemPos.x), transform.position.y + Random.Range(-randomItemPos.y, randomItemPos.y));
                GameObject item = Instantiate(itemList[randomIndex], randomPos, Quaternion.identity);
                GameManager.itemList.Add(item);
            }
        }
        if(bossAttack != null)
        {
            bossAttack.enabled = false;
        }
        if(enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
        if(takeDmg.healtCanvas != null)
        {
            takeDmg.healtCanvas.SetActive(false);
        }

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






