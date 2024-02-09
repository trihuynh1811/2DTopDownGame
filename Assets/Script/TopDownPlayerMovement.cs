using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownPlayerMovement : MonoBehaviour, ITakeDamage
{
    public static TopDownPlayerMovement instance;
    [SerializeField] Transform cam, camFollowPos;
    [SerializeField] Camera mainCam;
    [SerializeField] Vector3 offset;
    [SerializeField] PickUp pickUp;
    [SerializeField] LayerMask weaponMask, buffMask;
    [SerializeField] float pickUpRadius;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] Transform gunPos, buffPos;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    // player take damage
    [SerializeField] GameObject floatingText;
    [SerializeField] Transform floatingTextPos;
    [SerializeField] Vector2 randomFloatingTextPos;
    [SerializeField] float flashDuration;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Material hurtMat;
    // for camera shake
    [SerializeField] float magnitude;

    [SerializeField] Slider healthSlider, armourSlider, energySlider;
    float health, armour, energy;
    [SerializeField] float maxHealth, maxArmour, maxEnergy;
    [SerializeField] float regenerateArmourRate, regenerateEnegeyRate;
    [SerializeField] GameObject weaponStatCanvas;
    [SerializeField] Text weaponDmg, weaponRoF, weaponEnergyConsume, weaponCriticalHit, weaponAccuracy;

    public Text healthText, armourText, energyText;


    public int dmg { get; set; }
    public int numberOfBullet { get; set; }
    public int criticalChance { get; set; }
    public int criticalDmgMultiplier { get; set; } = 2;
    public float accuracy { get; set; }
    public float rateOfFire { get; set; }
    public bool canBounce { get; set; }

    Vector2 moveDirection, newFloatingTextPos;
    Material originalMat;
    bool m_FacingRight = true;
    float moveX, moveY;
    Collider2D weaponCollider, buffCollider;
    float energyDeductionRate;
    // for camera shake
    float fadeElapsed = 0f;
    public float fadeDuration { get; set; }

    public Vector2 explosionForce { get; set; }
    public bool isPushed { get; set; }
    public float explosionTime { get; set; }

    private void Awake()
    {
        instance = this;
        originalMat = playerSprite.material;
    }

    private void Start()
    {
        health = maxHealth;
        armour = maxArmour;
        energy = maxEnergy;

        healthSlider.maxValue = maxHealth;
        armourSlider.maxValue = maxArmour;
        energySlider.maxValue = maxEnergy;

        healthSlider.value = health;
        armourSlider.value = armour;
        energySlider.value = energy;

        healthText.text = $"{health} / {maxHealth}";
        armourText.text = $"{armour} / {maxArmour}";
        energyText.text = $"{energy} / {maxEnergy}";

        weaponStatCanvas.SetActive(false);

        InvokeRepeating("RegenerateArmor", 0f, regenerateArmourRate);
        InvokeRepeating("RegenerateEnergy", 0f, regenerateEnegeyRate);
    }
    private void Update()
    {
        CameraFollowAndPlayerLookAtMouse();
        CameraShake();
        GetInput();
        RotateGun();
        PickUpAndDropItem();
        if (energyDeductionRate <= 0)
        {
            ConsumeEnergy();
        }
        if (energyDeductionRate > 0) energyDeductionRate -= Time.deltaTime;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            pickUp.DropWeapon();
            pickUp.SwitchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pickUp.SwitchWeapon();
        }

        if (isPushed)
        {
            return;
        }
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    void Move()
    {
        rb.velocity = new Vector2((moveDirection.x * speed) + explosionForce.x, (moveDirection.y * speed) + explosionForce.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, -180f, 0f);
    }

    void RotateGun()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gunPos.position;

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        gunPos.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        if (rotationZ < -90 || rotationZ > 90)
        {
            gunPos.localRotation = Quaternion.Euler(180, 180, -rotationZ);
        }
    }

    void CameraShake()
    {
        if (explosionTime > 0)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCam.transform.localPosition = new Vector3(x, y, mainCam.transform.position.z);
            explosionTime -= Time.deltaTime;

        }
        if (explosionTime <= 0 && fadeDuration > 0)
        {
            isPushed = false;
            explosionForce = Vector2.zero;
            while (fadeElapsed < fadeDuration)
            {
                mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, Vector3.zero, fadeElapsed / fadeDuration);
                fadeElapsed += Time.deltaTime;
            }
            if (fadeElapsed >= fadeDuration)
            {
                mainCam.transform.localPosition = Vector3.zero;
            }
            fadeDuration = 0;
        }
    }

    void CameraFollowAndPlayerLookAtMouse()
    {
        cam.position = new Vector3(camFollowPos.position.x + offset.x, camFollowPos.position.y + offset.y, offset.z);
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (!m_FacingRight && delta.x > 0)
        {
            Flip();
        }
        else if (m_FacingRight && delta.x < 0)
        {
            Flip();
        }
    }

    void PickUpAndDropItem()
    {
        weaponCollider = Physics2D.OverlapCircle(transform.position, pickUpRadius, weaponMask);
        buffCollider = Physics2D.OverlapCircle(transform.position, pickUpRadius, buffMask);
        if (weaponCollider)
        {
            weaponDmg.text = weaponCollider.gameObject.GetComponent<Gun>().Damage;
            weaponRoF.text = weaponCollider.gameObject.GetComponent<Gun>().FireRate;
            weaponEnergyConsume.text = weaponCollider.gameObject.GetComponent<Gun>().EnergyConsume;
            weaponCriticalHit.text = weaponCollider.gameObject.GetComponent<Gun>().CriticalHitChance;
            weaponAccuracy.text = weaponCollider.gameObject.GetComponent<Gun>().SpreadAngle;
            weaponStatCanvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.X))
            {
                pickUp.PickUpWeapon(weaponCollider.gameObject);
            }
        }
        else
        {
            weaponDmg.text = "";
            weaponRoF.text = "";
            weaponEnergyConsume.text = "";
            weaponCriticalHit.text = "";
            weaponAccuracy.text = "";
            weaponStatCanvas.SetActive(false);
        }

        if (buffCollider)
        {
            switch (buffCollider.gameObject.tag)
            {
                case "Buff":
                    Buff buff = weaponCollider.gameObject.GetComponent<Buff>();
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        switch (buff.buffType)
                        {
                            case Buff.BuffType.MoreHealth:
                                SetMaxHealth(buff.buffAmount);
                                break;
                            case Buff.BuffType.MoreEnergy:
                                SetMaxEnergy(buff.buffAmount);
                                break;
                            case Buff.BuffType.MoreArmour:
                                SetMaxArmour(buff.buffAmount);
                                break;
                            case Buff.BuffType.MoreSpeed:
                                SetSpeed(buff.buffAmount);
                                break;
                            case Buff.BuffType.MoreWeaponDmg:
                                SetDmg(buff.buffAmount);
                                break;
                            case Buff.BuffType.MoreBulletForShotgun:
                                SetNumberOfBullet(buff.buffAmount);
                                break;
                            case Buff.BuffType.MoreWeaponRoF:
                                SetRoF();
                                break;
                            case Buff.BuffType.MoreWeaponAccuracy:
                                SetAccuracy();
                                break;
                            case Buff.BuffType.MoreWeaponCriticalChance:
                                SetCriticalChance(buff.buffAmount);
                                break;
                            case Buff.BuffType.MoreCriticalDmgMultiplier:
                                SetCriticalDmgMultiplier();
                                break;
                            case Buff.BuffType.CanBounce:
                                SetCanBounce(true);
                                break;
                        }
                        weaponCollider.gameObject.transform.parent = buffPos;
                        weaponCollider.gameObject.transform.localPosition = Vector3.zero;
                        StartCoroutine(weaponCollider.gameObject.GetComponent<Buff>().BuffPickUpEffect());
                        if (pickUp.GetCurrentWeapon() != null)
                        {
                            pickUp.GetCurrentWeapon().GetComponent<Gun>().ApplyBuff();
                        }

                    }
                    break;

                case "RefreshItemCube":
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        GameManager.instance.RefreshNewItem();
                    }
                    break;
            }

        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }

    public void TakeDamage(int dmg)
    {
        armour -= dmg;
        ShowDamage(dmg);
        if (armour <= 0)
        {
            armour = 0;
            health -= dmg;
            if (health <= 0)
            {
                health = 0;
            }
        }
        StartCoroutine(GetHit());
        UpdateUi();

    }

    IEnumerator GetHit()
    {
        playerSprite.material = hurtMat;
        yield return new WaitForSeconds(flashDuration);
        playerSprite.material = originalMat;

    }

    void ShowDamage(int dmg)
    {
        newFloatingTextPos = new(floatingTextPos.position.x + Random.Range(-randomFloatingTextPos.x, randomFloatingTextPos.x), floatingTextPos.position.y + Random.Range(0, randomFloatingTextPos.y));
        GameObject floatingTextClone = ObjectPoolManager.SpawnObject(floatingText, newFloatingTextPos, Quaternion.identity);
        floatingTextClone.transform.SetParent(gameObject.transform);
        floatingTextClone.GetComponent<TextMesh>().text = dmg.ToString();
    }

    void RegenerateArmor()
    {
        if (armour < maxArmour)
        {
            armour++;
            UpdateUi();
        }

    }

    void RegenerateEnergy()
    {
        if (energy < maxEnergy)
        {
            energy++;
            UpdateUi();
        }
    }

    public void ConsumeEnergy()
    {
        if (pickUp.GetCurrentWeapon() == null)
        {
            return;
        }

        if (pickUp.GetCurrentWeapon().GetComponent<Gun>().GetCurrentFireRate() <= 0 && Input.GetButton("Fire1"))
        {
            switch (pickUp.GetCurrentWeapon().GetComponent<Gun>().gunType)
            {
                case Gun.GunType.NormalGun:
                    energy -= pickUp.GetCurrentWeapon().GetComponent<Gun>().energyConsume;
                    energyDeductionRate = 0;
                    UpdateUi();
                    break;
                case Gun.GunType.LaserGun:
                    energy -= pickUp.GetCurrentWeapon().GetComponent<Gun>().energyConsume;
                    energyDeductionRate = pickUp.GetCurrentWeapon().GetComponent<Gun>().GetFireRate();
                    UpdateUi();
                    break;
                case Gun.GunType.Flamethrower:
                    energy -= pickUp.GetCurrentWeapon().GetComponent<Gun>().energyConsume;
                    energyDeductionRate = pickUp.GetCurrentWeapon().GetComponent<Gun>().GetFireRate();
                    UpdateUi();
                    break;

            }

        }
    }

    public void UpdateUi()
    {
        healthSlider.value = health;
        armourSlider.value = armour;
        energySlider.value = energy;

        healthText.text = $"{health} / {maxHealth}";
        armourText.text = $"{armour} / {maxArmour}";
        energyText.text = $"{energy} / {maxEnergy}";
    }

    public void SetMaxHealth(int moreHealth)
    {
        maxHealth += moreHealth;
        healthSlider.maxValue = maxHealth;
        healthText.text = $"{health} / {maxHealth}";
    }
    public void SetMaxArmour(int moreArmour)
    {
        maxArmour += moreArmour;
        armourSlider.maxValue = maxArmour;
        armourText.text = $"{armour} / {maxArmour}";
    }
    public void SetMaxEnergy(int moreEnergy)
    {
        maxEnergy += moreEnergy;
        energySlider.maxValue = maxEnergy;
        energyText.text = $"{energy} / {maxEnergy}";
    }
    public void SetDmg(int moreDmg)
    {
        dmg += moreDmg;
    }
    public void SetNumberOfBullet(int moreBullet)
    {
        numberOfBullet += moreBullet;
    }

    public void SetSpeed(int moreSpeed)
    {
        speed += moreSpeed;
    }

    public void SetRoF()
    {
        if (rateOfFire == 0)
        {
            rateOfFire = 15f / 100f;
            return;
        }
        if (rateOfFire >= 100f)
        {
            return;
        }
        rateOfFire += 15f / 100f;

    }

    public void SetAccuracy()
    {
        accuracy += 10f / 100f;
        Debug.Log(accuracy);
    }
    public void SetCriticalChance(int moreCriticalChance)
    {
        if (criticalChance >= 100)
        {
            return;
        }
        criticalChance += moreCriticalChance;
    }
    public void SetCriticalDmgMultiplier()
    {
        criticalDmgMultiplier *= 2;
    }
    public void SetCanBounce(bool ableToBounce)
    {
        canBounce = ableToBounce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
    }

}
