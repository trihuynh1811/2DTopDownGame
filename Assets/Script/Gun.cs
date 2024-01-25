using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType
    {
        NormalGun,
        Flamethrower,
        LaserGun
    }
    public GunType gunType;
    // Variable for flamethrower
    [SerializeField] List<ParticleSystem> flameParticle;

    // Variable for laser
    [SerializeField] LineRenderer laser;
    [SerializeField] GameObject laserStart, laserEnd;

    [SerializeField] LayerMask hitEnemyLayerMask;
    [SerializeField] LayerMask hitObjectLayerMask;
    [SerializeField] float rayCastLength;

    [SerializeField] float damage;
    public int energyConsume;
    public SpriteRenderer gunSprite;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float maxBulletSpeed;
    [SerializeField] float minBulletSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float spreadAngle;
    [SerializeField] int numberOfBullet;
    float currentFireRate;
    float spread;
    float currentBulletSpeed;
    float currentRayCastLength;
    float distance;
    RaycastHit2D hitEnemy;
    RaycastHit2D hitObject;
    // Start is called before the first frame update
    private void Awake()
    {
        this.enabled = false;
        currentRayCastLength = rayCastLength;
        switch (gunType)
        {
            case GunType.Flamethrower:
                for (int i = 0; i < flameParticle.Count; i++)
                {
                    flameParticle[i].Stop();
                }
                break;
            case GunType.LaserGun:
                laserStart.SetActive(false);
                laserEnd.SetActive(false);
                laser.SetPosition(1, Vector3.zero);
                break;
        }
    }

    private void OnEnable()
    {
        gunSprite.sortingLayerName = "Player";
    }
    private void OnDisable()
    {
        gunSprite.sortingLayerName = "Weapon";
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (gunType)
        {
            case GunType.NormalGun:
                if (Input.GetButton("Fire1") && currentFireRate <= 0)
                {
                    Shoot();
                }
                if (currentFireRate > 0) currentFireRate -= Time.deltaTime;
                break;
            case GunType.Flamethrower:
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
                else
                {
                    for (int i = 0; i < flameParticle.Count; i++)
                    {
                        flameParticle[i].Stop();
                    }
                }
                if (currentFireRate > 0) currentFireRate -= Time.deltaTime;
                break;
            case GunType.LaserGun:
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
                else
                {
                    laserStart.SetActive(false);
                    laserEnd.SetActive(false);
                    laser.SetPosition(1, Vector3.zero);
                }
                if (currentFireRate > 0) currentFireRate -= Time.deltaTime;
                break;
        }


    }

    void Shoot()
    {
        switch (gunType)
        {
            case GunType.NormalGun:
                currentFireRate = fireRate;
                for (int i = 0; i < numberOfBullet; i++)
                {
                    spread = Random.Range(-spreadAngle, spreadAngle);
                    currentBulletSpeed = Random.Range(minBulletSpeed, maxBulletSpeed);
                    Vector2 direction = Quaternion.Euler(0, 0, spread) * firePoint.right;
                    GameObject bulletClone = Instantiate(bullet, firePoint.position, firePoint.rotation);
                    bulletClone.transform.right = direction.normalized;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(direction.normalized * currentBulletSpeed);
                }
                break;
            case GunType.Flamethrower:
                for (int i = 0; i < flameParticle.Count; i++)
                {
                    flameParticle[i].Play();
                }
                hitEnemy = Physics2D.Raycast(firePoint.position, firePoint.right, currentRayCastLength, hitEnemyLayerMask);
                if (hitEnemy && currentFireRate <= 0)
                {
                    hitEnemy.collider.gameObject.GetComponent<EnemyMovement>().TakeDamage(damage);
                    currentFireRate = fireRate;
                }
                else
                {
                    currentRayCastLength = rayCastLength;
                }
                break;
            case GunType.LaserGun:
                laserStart.SetActive(true);
                hitEnemy = Physics2D.Raycast(firePoint.position, firePoint.right, rayCastLength, hitEnemyLayerMask);
                hitObject = Physics2D.Raycast(firePoint.position, firePoint.right, rayCastLength, hitObjectLayerMask);
                if (hitEnemy && currentFireRate <= 0)
                {
                    hitEnemy.collider.gameObject.GetComponent<EnemyMovement>().TakeDamage(damage);
                    currentFireRate = fireRate;
                }
                if (hitObject)
                {
                    distance = ((Vector2)hitObject.point - (Vector2)firePoint.position).magnitude;
                    laserEnd.transform.position = hitObject.point;
                    laserEnd.SetActive(true);
                }
                else
                {
                    laserEnd.SetActive(false);
                    distance = rayCastLength;
                }
                laser.SetPosition(1, new Vector2(distance, 0));
                break;
        }


    }

    public float GetCurrentFireRate()
    {
        return currentFireRate;
    }

    public void ResetCurrentFireRate()
    {
        currentFireRate = 0;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(firePoint.position, firePoint.right * currentRayCastLength);
    }
}
