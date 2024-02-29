using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum UsageType
    {
        Normal,
        AnimationFunction
    }
    public enum AttackType
    {
        SelfDestruct,
        ShootLaser,
        ShootProjectile,
        ShootFlame
    }
    public UsageType usageType;
    public AttackType attackType;
    [SerializeField] GameObject player;
    [SerializeField] bool useDeadEffect;
    [SerializeField] GameObject deathEffect;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathClip;
    [SerializeField] int explosionDmg;
    [SerializeField] float explosionForce, explosionTime, splashRadius;

    [SerializeField] bool haveMultipleFirePoint;
    [SerializeField] List<Transform> firePointList;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    [SerializeField] int damage;
    float currentFireRate;

    [SerializeField] List<ParticleSystem> flameParticleList;
    [SerializeField] float flameRaycastLength, gunRotationSpeed;
    [SerializeField] Transform gunTransform;
    [SerializeField] List<FlameDetectPlayer> flameDetectPlayerList;

    [SerializeField] LineRenderer laser;
    [SerializeField] int laserLength;
    [SerializeField] float laserExistTime;
    [SerializeField] LayerMask playerMask;
    RaycastHit2D hit;
    float currentLaserExistTime;

    private void Awake()
    {
        player = GameObject.Find("Player");
        switch (usageType)
        {
            case UsageType.Normal:
                switch (attackType)
                {
                    case AttackType.ShootLaser:
                        laser.SetPosition(1, new Vector2(laserLength, 0));
                        laser.gameObject.SetActive(false);
                        break;
                    case AttackType.ShootFlame:
                        for (int i = 0; i < flameParticleList.Count; i++)
                        {
                            flameParticleList[i].Stop();
                        }
                        break;
                }
                break;
        }

    }

    public void Attack()
    {
        switch (attackType)
        {
            case AttackType.SelfDestruct:
                Explode();
                break;
            case AttackType.ShootLaser:
                ShootLaser();
                if (currentFireRate > 0) currentFireRate -= Time.deltaTime;
                break;
            case AttackType.ShootFlame:
                RotateGun();
                ShootFlame();
                if (currentFireRate > 0) currentFireRate -= Time.deltaTime;
                break;
        }
    }

    public void Explode()
    {
        if (useDeadEffect)
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
        else
        {
            StartCoroutine(PlayDeathAnimation());
        }


    }

    IEnumerator PlayDeathAnimation()
    {
        animator.Play(deathClip.name);
        if ((int)Vector2.Distance(transform.position, player.transform.position) <= splashRadius)
        {
            Debug.Log("in explosion range");
            Vector2 explosionVector = (player.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;
            TopDownPlayerMovement.instance.explosionForce = new Vector2(explosionVector.x * explosionForce, explosionVector.y * explosionForce);
            TopDownPlayerMovement.instance.explosionTime = explosionTime;
            TopDownPlayerMovement.instance.fadeDuration = explosionTime;
            TopDownPlayerMovement.instance.TakeDamage(explosionDmg);
        }
        yield return new WaitForSeconds(deathClip.length);
        gameObject.SetActive(false);
    }

    void ApplyExplosion()
    {
        Debug.Log(splashRadius);
        Debug.Log((int)Vector2.Distance(transform.parent.position, player.transform.position));
        Debug.Log((int)Vector2.Distance(transform.parent.position, player.transform.position) <= splashRadius);
        if ((int)Vector2.Distance(transform.parent.position, player.transform.position) <= splashRadius)
        {
            Debug.Log("in explosion range");
            Vector2 explosionVector = (player.GetComponent<Rigidbody2D>().transform.position - transform.parent.position).normalized;
            TopDownPlayerMovement.instance.explosionForce = new Vector2(explosionVector.x * explosionForce, explosionVector.y * explosionForce);
            TopDownPlayerMovement.instance.explosionTime = explosionTime;
            TopDownPlayerMovement.instance.fadeDuration = explosionTime;
            TopDownPlayerMovement.instance.TakeDamage(explosionDmg);
        }
    }
    void DisableGameObject()
    {
        transform.parent.gameObject.SetActive(false);
    }
    void ShootLaser()
    {
        if (currentLaserExistTime > 0)
        {
            currentLaserExistTime -= Time.deltaTime;
        }
        if (currentLaserExistTime <= 0)
        {
            StartCoroutine(ResetLaser());
        }

    }

    IEnumerator ResetLaser()
    {
        ActivateLaser();
        yield return new WaitForSeconds(1f);
        currentLaserExistTime = laserExistTime;
        laser.gameObject.SetActive(false);
        currentFireRate = 0;
    }

    void ActivateLaser()
    {
        laser.gameObject.SetActive(true);
        hit = Physics2D.Raycast(firePoint.position, firePoint.right, laserLength, playerMask);
        if (hit)
        {
            if (currentFireRate <= 0)
            {
                TopDownPlayerMovement.instance.TakeDamage(damage);
                currentFireRate = fireRate;
            }
        }
    }

    public void DisableAttack()
    {
        switch (attackType)
        {
            case AttackType.ShootLaser:
                currentLaserExistTime = laserExistTime;
                laser.gameObject.SetActive(false);
                currentFireRate = 0;
                break;
            case AttackType.ShootFlame:
                for (int i = 0; i < flameParticleList.Count; i++)
                {
                    flameParticleList[i].Stop();
                }
                currentFireRate = 0;
                RotateGun();
                break;
        }
    }

    void RotateGun()
    {
        Vector3 differance = player.transform.position - gunTransform.position;
        float rotZ = Mathf.Atan2(differance.y, differance.x) * Mathf.Rad2Deg;
        Quaternion targetQuaternion = Quaternion.Euler(0f, 0f, rotZ);
        if (rotZ < -90 || rotZ > 90)
        {
            targetQuaternion = Quaternion.Euler(180f, 0, -rotZ);
        }

        // Rotate towards the target rotation
        gunTransform.rotation = Quaternion.RotateTowards(gunTransform.rotation, targetQuaternion, gunRotationSpeed * Time.deltaTime);
    }

    void ShootFlame()
    {
        for (int i = 0; i < flameParticleList.Count; i++)
        {
            flameParticleList[i].Play();
        }
        if (currentFireRate <= 0)
        {
            for (int i = 0; i < flameDetectPlayerList.Count; i++)
            {
                flameDetectPlayerList[i].damge = damage;
                flameDetectPlayerList[i].damageRate = currentFireRate;
            }
            currentFireRate = fireRate;
        }
        else
        {
            for (int i = 0; i < flameDetectPlayerList.Count; i++)
            {
                flameDetectPlayerList[i].damge = 0;
                flameDetectPlayerList[i].damageRate = fireRate;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
