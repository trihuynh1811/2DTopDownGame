using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemStat), typeof(Rigidbody2D), typeof(BoxCollider2D))]
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
    [SerializeField] FlameDetectPlayer flameDetectPlayer;

    // Variable for laser
    [SerializeField] LineRenderer laser;
    [SerializeField] GameObject laserStart, laserEnd;

    [SerializeField] LayerMask laserHitMask;
    [SerializeField] float rayCastLength;

    [SerializeField] int damage;
    [SerializeField] int criticalHitChance;
    public int energyConsume;
    public SpriteRenderer gunSprite;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float maxBulletSpeed;
    [SerializeField] float minBulletSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float spreadAngle;
    [SerializeField] int numberOfBullet;
    [SerializeField] bool isShotGun;
    float currentFireRate;
    float spread;
    float currentBulletSpeed;
    float currentRayCastLength;
    float distance;
    int currentDamage;
    int currentNumberOfBullet;
    int currentCriticalChance;
    float currentSpreadAngle;
    float currentRateOfFire;
    RaycastHit2D hit;
    RaycastHit2D hitObject;
    // Start is called before the first frame update
    private void Awake()
    {
        this.enabled = false;
        currentDamage = damage;
        currentNumberOfBullet = numberOfBullet;
        currentCriticalChance = criticalHitChance;
        currentSpreadAngle = spreadAngle;
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
                if (TopDownPlayerMovement.instance.energy <= 0)
                {
                    TopDownPlayerMovement.instance.energy = 0;
                    TopDownPlayerMovement.instance.UpdateUi();
                    return;
                }
                currentFireRate = fireRate - (fireRate * TopDownPlayerMovement.instance.rateOfFire);
                for (int i = 0; i < currentNumberOfBullet; i++)
                {
                    CalculteCriticalHitChance();
                    spread = Random.Range(-currentSpreadAngle, currentSpreadAngle);
                    currentBulletSpeed = Random.Range(minBulletSpeed, maxBulletSpeed);
                    Vector2 direction = Quaternion.Euler(0, 0, spread) * firePoint.right;
                    //GameObject bulletClone = Instantiate(bullet, firePoint.position, firePoint.rotation);
                    GameObject bulletClone = ObjectPoolManager.SpawnObject(bullet, firePoint.position, firePoint.rotation, ObjectPoolManager.PoolType.GameObject);
                    bulletClone.GetComponent<Bullet>().canBounce = TopDownPlayerMovement.instance.canBounce;
                    bulletClone.GetComponent<Bullet>().SetDmg(currentDamage);
                    bulletClone.transform.right = direction.normalized;
                    bulletClone.GetComponent<Rigidbody2D>().AddForce(direction.normalized * currentBulletSpeed);
                }
                break;
            case GunType.Flamethrower:
                if (TopDownPlayerMovement.instance.energy <= 0)
                {
                    TopDownPlayerMovement.instance.energy = 0;
                    TopDownPlayerMovement.instance.UpdateUi();
                    for (int i = 0; i < flameParticle.Count; i++)
                    {
                        flameParticle[i].Stop();
                    }
                    return;
                }
                for (int i = 0; i < flameParticle.Count; i++)
                {
                    flameParticle[i].Play();
                }
                if (currentFireRate <= 0)
                {
                    flameDetectPlayer.damge = damage;
                    flameDetectPlayer.damageRate = currentFireRate;
                    currentFireRate = fireRate;
                }
                else
                {
                    flameDetectPlayer.damge = 0;
                    flameDetectPlayer.damageRate = fireRate;
                }

                break;
            case GunType.LaserGun:
                if (TopDownPlayerMovement.instance.energy <= 0)
                {
                    TopDownPlayerMovement.instance.energy = 0;
                    TopDownPlayerMovement.instance.UpdateUi();
                    laserStart.SetActive(false);
                    laser.SetPosition(1, new Vector2(0, 0));
                    laserEnd.SetActive(false);
                    return;
                }
                laserStart.SetActive(true);
                hit = Physics2D.Raycast(firePoint.position, firePoint.right, rayCastLength, laserHitMask);
                if (hit)
                {
                    if (hit.collider.gameObject.CompareTag("Enemy") && currentFireRate <= 0)
                    {
                        hit.collider.gameObject.GetComponent<EnemyTakeDmg>().TakeDamage(currentDamage);
                        currentFireRate = fireRate;
                    }
                    laserEnd.SetActive(true);
                    distance = ((Vector2)hit.point - (Vector2)firePoint.transform.position).magnitude;
                    laserEnd.transform.position = hit.point;
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

    void CalculteCriticalHitChance()
    {
        int randomChance = Random.Range(0, 100);

        if (randomChance < currentCriticalChance)
        {
            currentDamage = (damage + TopDownPlayerMovement.instance.dmg) * TopDownPlayerMovement.instance.criticalDmgMultiplier;
            Debug.Log("score critical hit");
            return;
        }
        currentDamage = damage + TopDownPlayerMovement.instance.dmg;
    }

    public void ApplyBuff()
    {
        if (currentSpreadAngle <= 0)
        {
            currentSpreadAngle = 0.01f;
        }

        if (currentNumberOfBullet != numberOfBullet + TopDownPlayerMovement.instance.numberOfBullet && isShotGun)
        {
            currentNumberOfBullet = numberOfBullet + TopDownPlayerMovement.instance.numberOfBullet;
        }

        if (currentSpreadAngle != spreadAngle - (spreadAngle * TopDownPlayerMovement.instance.accuracy))
        {
            currentSpreadAngle = spreadAngle - (spreadAngle * TopDownPlayerMovement.instance.accuracy);
        }

        if (currentCriticalChance != criticalHitChance + TopDownPlayerMovement.instance.criticalChance)
        {
            currentCriticalChance = criticalHitChance + TopDownPlayerMovement.instance.criticalChance;
        }

        if (currentDamage != damage + TopDownPlayerMovement.instance.dmg)
        {
            currentDamage = damage + TopDownPlayerMovement.instance.dmg;
        }

        if (currentRateOfFire != fireRate + TopDownPlayerMovement.instance.rateOfFire)
        {
            currentRateOfFire = fireRate + TopDownPlayerMovement.instance.rateOfFire;
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

    public string Damage => damage.ToString();
    public string CriticalHitChance => criticalHitChance.ToString() + "%";
    public string EnergyConsume => energyConsume.ToString();
    public string FireRate => (60 / fireRate).ToString();
    public string SpreadAngle => (100 - spreadAngle).ToString() + "%";

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(firePoint.position, firePoint.right * currentRayCastLength);
    }
}
