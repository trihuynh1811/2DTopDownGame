using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum AttackType
    {
        SelfDestruct,
        ShootLaser,
        ShootProjectile
    }
    public AttackType attackType;
    [SerializeField] GameObject player;
    [SerializeField] GameObject deathEffect;
    [SerializeField] int explosionDmg;
    [SerializeField] float explosionForce, explosionTime, splashRadius;

    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    [SerializeField] int damage;
    float currentFireRate;

    [SerializeField] LineRenderer laser;
    [SerializeField] int laserLength;
    [SerializeField] float laserExistTime;
    [SerializeField] LayerMask playerMask;
    RaycastHit2D hit;
    float currentLaserExistTime;

    private void Awake()
    {
        player = GameObject.Find("Player");
        switch (attackType)
        {
            case AttackType.ShootLaser:
                laser.SetPosition(1, new Vector2(laserLength, 0));
                laser.gameObject.SetActive(false);
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
        }
    }

    public void Explode()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Debug.Log(Vector2.Distance(transform.position, player.transform.position));
        Debug.Log(Vector2.Distance(transform.position, player.transform.position) <= splashRadius);
        if ((int)Vector2.Distance(transform.position, player.transform.position) <= splashRadius)
        {
            Debug.Log("in explosion range");
            Vector2 explosionVector = (player.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;
            TopDownPlayerMovement.instance.explosionForce = new Vector2(explosionVector.x * explosionForce, explosionVector.y * explosionForce);
            TopDownPlayerMovement.instance.explosionTime = explosionTime;
            TopDownPlayerMovement.instance.fadeDuration = explosionTime;
            TopDownPlayerMovement.instance.TakeDamage(explosionDmg);
        }
        gameObject.SetActive(false);

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
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
