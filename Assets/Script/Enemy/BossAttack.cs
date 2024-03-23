using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public enum Boss
    {
        Crab,
        MechaGolem,
        Death
    }
    public Boss bossType;

    [SerializeField] EnemyTakeDmg enemyTakeDmg;

    [SerializeField] Transform firePoint, gunHolderPos;
    [SerializeField] GameObject bullet, gatling, flamethrower, missleLauncher;
    [SerializeField] int bulletDamage, flameDamage;
    [SerializeField] float gunHolderRotateSpeed, fireRate, flameFireRate, spreadAngle, numberOfBullet, maxBulletSpeed, minBulletSpeed;
    [SerializeField] List<ParticleSystem> flameParticle;
    [SerializeField] FlameDetectPlayer flameDetectPlayer;
    [SerializeField] float minXpos, maxXpos, minYpos, maxYpos, missleSpeed, timeBtwLaunchingMissile;
    [SerializeField] int maxNumberMissle, minNumberMissle;
    [SerializeField] GameObject missleIndicator, missle, missleSpawnPoint;
    float currentFireRate, spread, currentBulletSpeed;
    Vector2 rotateDirection;

    [SerializeField] float rotation, numberOfShootPoint, radiusMultiplier;
    [SerializeField] List<Transform> shootPointList;

    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attackClip;
    [SerializeField] GameObject ring;
    [SerializeField] List<GameObject> drone, laserLines, lasers;
    [SerializeField] float ringRotationSpeed, minRotateTime, maxRotateTime, laserLength, damageRate, randomLaserTime, shootFireBallRate;
    [SerializeField] int laserDmg;
    [SerializeField] LayerMask hitMask;
    float currentRotateTime, currentDamageRate, currentRandomLaserTime;
    Transform player;

    [SerializeField] EnemyChasePlayerTest enemyChasePlayerTest;
    [SerializeField] SpriteRenderer deathSprite;
    [SerializeField] AnimationClip disappearClip, attack_2, attack_3;
    [SerializeField] SpriteMask deathMask;
    [SerializeField] float attackRate, blackHoleRotateSpeed, disappearRate, appearRate, shootRate, splashSpeed;
    public GameObject blackHole, leftEyeTrail, rightEyeTrail, splash, shootPointListObject;
    float currentAttackRate, currentDisappearRate, currentAppearRate, currentSpeed, currentShootRate, currentTransformRotateSpeed;
    bool disappear;

    [SerializeField] bool useEffectWhenDie;
    [SerializeField] GameObject deathEffect;
    [SerializeField] int explosionDmg;
    [SerializeField] float explosionForce, explosionTime, splashRadius;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentRotateTime = Random.Range(minRotateTime, maxRotateTime);
        currentRandomLaserTime = randomLaserTime;
        currentDisappearRate = disappearRate;
        if (bossType == Boss.Death)
        {
            currentSpeed = enemyChasePlayerTest.speed;
            currentTransformRotateSpeed = enemyChasePlayerTest.transformRotationSpeed;
        }
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyTakeDmg.health <= 0)
        {
            enemyTakeDmg.health = 0;
            this.enabled = false;
        }

        switch (bossType)
        {

            case Boss.MechaGolem:
                GolemAttack();
                if (currentDamageRate > 0) currentDamageRate -= Time.deltaTime;
                currentRandomLaserTime -= Time.deltaTime;
                if (currentRandomLaserTime <= 0)
                {
                    currentRandomLaserTime = 0;
                }
                break;
            case Boss.Crab:
                RotateGunTowardPlayer();
                ShootPlayer();
                if (currentFireRate > 0) currentFireRate -= Time.deltaTime;
                break;
            case Boss.Death:
                DeathAttack();
                if (currentAttackRate > 0) currentAttackRate -= Time.deltaTime;

                break;

        }

    }

    void RotateRing()
    {
        if (currentRotateTime > 0)
        {
            ring.transform.Rotate(0, 0, ringRotationSpeed * Time.deltaTime);
            currentRotateTime -= Time.deltaTime;
        }
        else
        {
            ring.transform.Rotate(0, 0, 0);
            StartCoroutine(ResetRandomRotateTime());
        }
    }

    IEnumerator ResetRandomRotateTime()
    {
        ActivateLaser();
        yield return new WaitForSeconds(1f);
        currentRotateTime = Random.Range(minRotateTime, maxRotateTime);
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
        currentDamageRate = 0;
    }

    IEnumerator ActivateSingleLaser(int index)
    {
        shootPointList[index].transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(.25f);
        shootPointList[index].transform.GetChild(1).gameObject.SetActive(true);
        RaycastHit2D hit = Physics2D.Raycast(shootPointList[index].transform.GetChild(1).gameObject.transform.position,
            shootPointList[index].transform.GetChild(1).gameObject.transform.right, laserLength, hitMask);

        if (hit)
        {
            hit.collider.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage(laserDmg * 2);
        }
        yield return new WaitForSeconds(0.25f);
        shootPointList[index].transform.GetChild(0).gameObject.SetActive(false);
        shootPointList[index].transform.GetChild(1).gameObject.SetActive(false);
        shootPointList[index].gameObject.SetActive(false);
    }

    void ActivateLaser()
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(true);

            RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, laser.transform.right, laserLength, hitMask);

            if (hit)
            {
                if (currentDamageRate <= 0)
                {
                    hit.collider.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage(laserDmg);
                    currentDamageRate = damageRate;
                }
            }
        }
    }

    void ActivateLaserRandomly()
    {
        int randomShootpointIndex = Random.Range(1, shootPointList.Count);
        shootPointList[randomShootpointIndex].gameObject.SetActive(true);
        StartCoroutine(ActivateSingleLaser(randomShootpointIndex));
    }

    void RotateGunTowardPlayer()
    {
        Vector3 differance = player.transform.position - gunHolderPos.position;
        float rotZ = Mathf.Atan2(differance.y, differance.x) * Mathf.Rad2Deg;
        Quaternion targetQuaternion = Quaternion.Euler(0f, 0f, rotZ);
        if (rotZ < -90 || rotZ > 90)
        {
            targetQuaternion = Quaternion.Euler(180f, 0, -rotZ);
        }

        // Rotate towards the target rotation
        gunHolderPos.rotation = Quaternion.RotateTowards(gunHolderPos.rotation, targetQuaternion, gunHolderRotateSpeed * Time.deltaTime);
    }

    void ShootPlayer()
    {
        if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 75) / 100) && enemyTakeDmg.health <= enemyTakeDmg.maxHealth)
        {
            if (currentFireRate <= 0)
            {
                for (int i = 0; i < numberOfBullet; i++)
                {
                    spread = Random.Range(-spreadAngle, spreadAngle);
                    currentBulletSpeed = Random.Range(minBulletSpeed, maxBulletSpeed);
                    Vector2 direction = Quaternion.Euler(0, 0, spread) * firePoint.right;
                    //GameObject bulletClone = Instantiate(bullet, firePoint.position, firePoint.rotation);
                    GameObject bulletClone = ObjectPoolManager.SpawnObject(bullet, firePoint.position, firePoint.rotation, ObjectPoolManager.PoolType.GameObject);
                    bulletClone.GetComponent<Bullet>().SetDmg(bulletDamage);
                    bulletClone.transform.right = direction.normalized;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(direction.normalized * currentBulletSpeed);
                }
                currentFireRate = fireRate;
            }

        }
        else
        {
            if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 50) / 100) && enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 75) / 100))
            {
                gatling.SetActive(false);
                flamethrower.SetActive(true);
                for (int i = 0; i < flameParticle.Count; i++)
                {
                    flameParticle[i].Play();
                }
                if (currentFireRate <= 0)
                {
                    flameDetectPlayer.damge = flameDamage;
                    flameDetectPlayer.damageRate = currentFireRate;
                    currentFireRate = flameFireRate;
                }
                else
                {
                    Debug.Log("Do nothing");
                    flameDetectPlayer.damge = 0;
                    flameDetectPlayer.damageRate = flameFireRate;
                }
            }
            if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 25) / 100) && enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 50) / 100) || enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 25) / 100) && enemyTakeDmg.health > 0)
            {
                missleLauncher.SetActive(true);

                if (currentFireRate <= 0)
                {
                    int numberMissile = Random.Range(minNumberMissle, maxNumberMissle);
                    Debug.Log(numberMissile);

                    for (int i = 0; i < numberMissile; i++)
                    {
                        Vector2 missleIndicatorPos = new(Random.Range(minXpos, maxXpos + .1f), Random.Range(minYpos, maxYpos + .1f));
                        //Vector2 missleIndicatorPos = (Vector2)player.position + new Vector2(Random.Range(minXpos, maxXpos), Random.Range(minYpos, maxYpos));
                        //missleIndicatorPos.x = missleIndicatorPos.x > maxXpos ? maxXpos : missleIndicatorPos.x < minXpos ? minXpos : missleIndicatorPos.x;
                        //missleIndicatorPos.y = missleIndicatorPos.y > maxYpos ? maxYpos : missleIndicatorPos.y < minYpos ? minYpos : missleIndicatorPos.y;
                        StartCoroutine(LaunchMissle(missleIndicatorPos, i));
                    }
                    currentFireRate = timeBtwLaunchingMissile;
                }
            }
        }

    }

    void GolemAttack()
    {
        if (currentDamageRate <= 0)
        {
            animator.Play(attackClip.name);
            currentDamageRate = shootFireBallRate;
        }
        if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 50) / 100) && enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 75) / 100))
        {
            drone.ForEach(d => d.SetActive(true));
            RotateRing();

        }
        if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 25) / 100) && enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 50) / 100) || enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 25) / 100) && enemyTakeDmg.health > 0)
        {
            drone.ForEach(d => d.SetActive(false));
            if (currentRandomLaserTime > 0)
            {
                ActivateLaserRandomly();
            }
        }
    }

    void DeathAttack()
    {
        if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 75) / 100))
        {
            if (currentAttackRate <= 0)
            {
                animator.Play(attackClip.name);
                currentAttackRate = attackRate;
            }
        }
        if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 50) / 100) && enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 75) / 100))
        {
            currentAttackRate = 0;
            DeathDisappear();
            //blackHole.SetActive(true);
            //blackHole.transform.Rotate(0, 0, blackHoleRotateSpeed * Time.deltaTime);

        }
        if (enemyTakeDmg.health >= ((enemyTakeDmg.maxHealth * 25) / 100) && enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 50) / 100) || enemyTakeDmg.health < ((enemyTakeDmg.maxHealth * 25) / 100) && enemyTakeDmg.health > 0)
        {
            Time.timeScale = 1.0f;
            enemyChasePlayerTest.rb.drag = 3;
            enemyChasePlayerTest.speed = currentSpeed;
            enemyChasePlayerTest.transformRotationSpeed = currentTransformRotateSpeed;
            enemyChasePlayerTest.c2d.enabled = true;
            leftEyeTrail.SetActive(true);
            rightEyeTrail.SetActive(true);
            enemyChasePlayerTest.speed = 1000000;
            enemyChasePlayerTest.avoidanceForce = 10000;
            if (currentAttackRate <= 0)
            {
                animator.Play(attack_2.name);
                currentAttackRate = attackRate;
            }
            StartCoroutine(Blink());
        }
    }

    void DeathDisappear()
    {
        if (currentDisappearRate > 0)
        {
            animator.Play(disappearClip.name);
            enemyChasePlayerTest.c2d.enabled = false;
            currentDisappearRate -= Time.deltaTime;
            enemyChasePlayerTest.speed = 0;
            disappear = true;
        }
        else
        {
            enemyChasePlayerTest.c2d.enabled = true;
            enemyChasePlayerTest.speed = currentSpeed;
            StartCoroutine(DeathAppear());
        }
    }

    IEnumerator DeathAppear()
    {
        if (disappear)
        {
            animator.Rebind();
            transform.position = (Vector2)player.transform.position + new Vector2(Random.Range(-7, 7), 0);
            Time.timeScale = .5f;
            enemyTakeDmg.healtCanvas.SetActive(true);
            animator.Play(attack_2.name);
        }
        disappear = false;
        yield return new WaitForSeconds(attack_2.length);
        shootPointListObject.SetActive(true);
        enemyChasePlayerTest.speed = 0;
        enemyChasePlayerTest.transformRotationSpeed = 0;
        animator.Play(attack_3.name);
        yield return new WaitForSeconds(attack_3.length);
        currentDisappearRate = disappearRate;
        currentAttackRate = 0;
        enemyChasePlayerTest.speed = currentSpeed;
        enemyChasePlayerTest.transformRotationSpeed = currentTransformRotateSpeed;
        shootPointListObject.SetActive(false);
        enemyTakeDmg.healtCanvas.SetActive(false);
        yield return new WaitForSeconds(appearRate);
    }

    IEnumerator Blink()
    {
        while (true)
        {
            deathMask.sprite = deathSprite.sprite;
            deathSprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            yield return new WaitForSeconds(10f);
            deathMask.sprite = deathSprite.sprite;
            deathSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            yield return new WaitForSeconds(10f);
        }
    }

    public void ShootSplash()
    {
        if (currentShootRate <= 0)
        {
            shootPointListObject.SetActive(true);
            StartCoroutine(SpawnSplash());
        }
        else
        {
            shootPointListObject.SetActive(false);
            currentShootRate -= Time.deltaTime;
        }

    }

    IEnumerator SpawnSplash()
    {
        enemyChasePlayerTest.speed = 0;
        enemyChasePlayerTest.transformRotationSpeed = 0;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < shootPointList.Count; i++)
        {
            Debug.Log(i);
            GameObject splashClone = ObjectPoolManager.SpawnObject(splash, shootPointList[i].position, shootPointList[i].rotation, ObjectPoolManager.PoolType.GameObject);
            splashClone.GetComponent<Bullet>().SetDmg(15);
            splashClone.GetComponent<Rigidbody2D>().AddForce(shootPointList[i].right.normalized * splashSpeed, ForceMode2D.Impulse);
        }
        currentShootRate = shootRate;
        enemyChasePlayerTest.speed = currentSpeed;
        enemyChasePlayerTest.transformRotationSpeed = currentTransformRotateSpeed;
    }

    public void Explode()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        if ((int)Vector2.Distance(transform.position, player.transform.position) <= splashRadius)
        {
            Vector2 explosionVector = (player.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;
            TopDownPlayerMovement.instance.explosionForce = new Vector2(explosionVector.x * explosionForce, explosionVector.y * explosionForce);
            TopDownPlayerMovement.instance.explosionTime = explosionTime;
            TopDownPlayerMovement.instance.fadeDuration = explosionTime;
            TopDownPlayerMovement.instance.TakeDamage(explosionDmg);
        }
        gameObject.SetActive(false);
    }

    IEnumerator LaunchMissle(Vector2 missleIndicatorPos, int index)
    {
        yield return new WaitForSeconds(1f);
        GameObject missleClone = ObjectPoolManager.SpawnObject(missle, missleSpawnPoint.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        GameObject targetClone = ObjectPoolManager.SpawnObject(missleIndicator, missleIndicatorPos, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        targetClone.name = $"target_{index}";
        missleClone.GetComponent<Rigidbody2D>().AddForce(missleSpawnPoint.transform.up.normalized * missleSpeed);
        missleClone.GetComponent<CrapHomingMissle>().target = targetClone.transform;
    }

    // this function is for inspector button click event
    public void CalculateRotation()
    {
        if (shootPointList.Count <= 0)
        {
            return;
        }
        float radius = Mathf.Min(ring.transform.localScale.x, ring.transform.localScale.y) * radiusMultiplier;

        // Calculate the starting angle to ensure the first transform is at the top
        float startAngle = 90f;

        for (int i = 0; i < numberOfShootPoint; i++)
        {
            GameObject shootPointClone = Instantiate(shootPointList[0].gameObject, ring.transform.position, Quaternion.identity);
            shootPointClone.transform.parent = ring.transform;

            float angle = startAngle + i * rotation / numberOfShootPoint;
            float radians = Mathf.Deg2Rad * angle;

            float x = Mathf.Cos(radians) * radius;
            float y = Mathf.Sin(radians) * radius;

            Vector3 position = new Vector3(x, y, 0f);

            shootPointClone.transform.localPosition = position;
            shootPointClone.transform.localRotation = Quaternion.Euler(0, 0, angle);
            shootPointClone.transform.localScale = new(1, 1, 0);

            shootPointList.Add(shootPointClone.transform);
        }

    }
}
