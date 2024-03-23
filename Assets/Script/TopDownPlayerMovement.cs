using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopDownPlayerMovement : MonoBehaviour, ITakeDamage
{
    public static TopDownPlayerMovement instance;
    [SerializeField] Transform cam, camFollowPos;
    [SerializeField] Camera mainCam;
    [SerializeField] Vector3 offset;
    public PickUp pickUp;
    [SerializeField] LayerMask weaponMask, buffMask, itemMask;
    [SerializeField] float pickUpRadius;
    [SerializeField] BoxCollider2D boxCollider;
    public Transform gunPos, buffPos;
    public Transform itemPos;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    // player dash
    [SerializeField] GameObject playerDashAfterImage;
    [SerializeField] int numberOfAfterImage;
    [SerializeField] float dashLength = .5f, dashCooldown = 1f, dashSpeed;
    [SerializeField] GameObject dashBg;
    [SerializeField] RectMask2D dashMask;
    [SerializeField] float maxDaskMaskValue;
    float dashCounter;
    float dashCoolCounter, timer;
    bool isDashing;
    // player Shield
    [SerializeField] GameObject shield;
    [SerializeField] float shieldOnTime, shieldCoolDownTime;
    [SerializeField] GameObject shieldBg;
    [SerializeField] RectMask2D shieldMask;
    [SerializeField] float maxShieldMaskValue;
    float currentShieldOnTime, currentShieldCoolDownTime;
    bool isShielded;
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
    int health, armour;
    public int energy { get; set; }
    public int coin { get; set; } = 0;
    [SerializeField] int maxHealth, maxArmour, maxEnergy;
    [SerializeField] float regenerateArmourRate, regenerateEnegeyRate, regenerateHealthRate;
    [SerializeField] GameObject weaponStatCanvas, buffDescCanvas;
    [SerializeField] GameObject startNewWaveTxt;
    [SerializeField] Text weaponDmg, weaponRoF, weaponEnergyConsume, weaponCriticalHit, weaponAccuracy;
    [SerializeField] TextMeshProUGUI buffDesc;
    public GameObject winLoseCanvas, winCanvas, loseCanvas, dieCanvas;
    public int reviveCoin = 500, deadTime = 0, monsterKill, damageDeal;
    public Text reviveCoinText, monsterKillText, damageDealText;

    public Text healthText, armourText, energyText, coinText;

    public GameObject buffStatPanel;
    public TextMeshProUGUI maxHealthText, maxArmourText, maxEnergyText, weaponDamageText, weaponRofText, criticalChanceText, criticalDamageText, weaponAccuracyText, movementSpeedText, numberOfBulletText;
    int currentMaxHealth, currenMaxtArmour, currentMaxEnergy, currentSpeed, currentCriticalDamage;
    float currentMoveSpeed;

    public int dmg { get; set; }
    public int numberOfBullet { get; set; }
    public int criticalChance { get; set; }
    public int criticalDmgMultiplier { get; set; } = 2;
    public float accuracy { get; set; }
    public float rateOfFire { get; set; }
    public bool canBounce { get; set; }
    public bool allowHealthRegen { get; set; }
    public bool canDash { get; set; }
    public bool canUseShield { get; set; }

    Vector2 moveDirection, newFloatingTextPos;
    Material originalMat;
    bool m_FacingRight = true;
    float moveX, moveY;
    Collider2D weaponCollider, buffCollider, itemCollider;
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
        currentMoveSpeed = speed;
        originalMat = playerSprite.material;
        //if (ObjectPoolManager.ObjectPools.Find(x => x.LookUpString.Equals(playerDashAfterImage.name)) == null)
        //{
        //    for (int i = 0; i < numberOfAfterImage; i++)
        //    {
        //        GameObject cloneObject = ObjectPoolManager.SpawnObject(playerDashAfterImage, transform.position, transform.rotation);
        //        cloneObject.GetComponent<AfterImageSprite>().objectTransform = transform;
        //        cloneObject.GetComponent<AfterImageSprite>().objectSr = playerSprite;
        //        cloneObject.transform.SetParent(transform);
        //    }
        //}
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

        coinText.text = coin.ToString();

        maxHealthText.text = $"+{currentMaxHealth}";
        maxArmourText.text = $"+{currenMaxtArmour}";
        maxEnergyText.text = $"+{currentMaxEnergy}";
        weaponDamageText.text = $"+{dmg}";
        weaponRofText.text = $"+{rateOfFire}";
        criticalChanceText.text = $"+{criticalChance}";
        criticalDamageText.text = $"+{currentCriticalDamage}";
        weaponAccuracyText.text = $"+{accuracy}";
        movementSpeedText.text = $"+{currentSpeed}";
        numberOfBulletText.text = $"+{numberOfBullet}";

        weaponStatCanvas.SetActive(false);
        buffStatPanel.SetActive(false);
        buffDescCanvas.SetActive(false);

        InvokeRepeating("RegenerateArmor", 0f, regenerateArmourRate);
        InvokeRepeating("RegenerateEnergy", 0f, regenerateEnegeyRate);
        InvokeRepeating("RegenerateHealth", 0f, regenerateHealthRate);
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
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            //timer += Time.deltaTime;
            float dashProgress = 1f - (dashCounter / dashLength);
            dashMask.padding = new Vector4(0, 0, 0, Mathf.Lerp(0, maxDaskMaskValue, dashProgress));
            if (dashCounter <= 0)
            {
                dashCounter = 0;
                speed = currentMoveSpeed;
                dashCoolCounter = dashCooldown;
                isDashing = false;
            }
            //dashMask.padding = new Vector4(0, 0, 0, Mathf.Lerp(0, maxDaskMaskValue, timer / dashLength));
        }
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
            //timer += Time.deltaTime;
            float cooldownProgress = 1f - (dashCoolCounter / dashCooldown);
            dashMask.padding = new Vector4(0, 0, 0, Mathf.Lerp(maxDaskMaskValue, 0, cooldownProgress));
            if (dashCoolCounter <= 0)
            {
                dashCoolCounter = 0;
            }
            //dashMask.padding = new Vector4(0, 0, 0, Mathf.Lerp(maxDaskMaskValue, 0, timer / dashCooldown));
        }
        if (currentShieldOnTime > 0)
        {
            currentShieldOnTime -= Time.deltaTime;
            float cooldownProgress = 1f - (currentShieldOnTime / shieldOnTime);
            shieldMask.padding = new Vector4(0, 0, 0, Mathf.Lerp(0, maxShieldMaskValue, cooldownProgress));
            if (currentShieldOnTime <= 0)
            {
                shield.SetActive(false);
                isShielded = false;
                currentShieldOnTime = 0;
                currentShieldCoolDownTime = shieldCoolDownTime;
            }
        }
        if (currentShieldCoolDownTime > 0)
        {
            currentShieldCoolDownTime -= Time.deltaTime;
            float cooldownProgress = 1f - (currentShieldCoolDownTime / shieldCoolDownTime);
            shieldMask.padding = new Vector4(0, 0, 0, Mathf.Lerp(maxShieldMaskValue, 0, cooldownProgress));
            if (currentShieldCoolDownTime <= 0)
            {
                currentShieldCoolDownTime = 0;
            }
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
        if (Input.GetKey(KeyCode.B))
        {
            UpdateUi();
            buffStatPanel.SetActive(true);
        }
        else
        {
            buffStatPanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            pickUp.DropWeapon();
            pickUp.SwitchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pickUp.SwitchWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && canUseShield)
        {
            ShieldOn();
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
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                speed = dashSpeed;
                dashCounter = dashLength;
                isDashing = true;
            }
        }
        if (isDashing && moveDirection.x == 0)
        {
            rb.velocity = new Vector2((speed) + explosionForce.x, (moveDirection.y * speed) + explosionForce.y);
        }
        else
        {
            rb.velocity = new Vector2((moveDirection.x * speed) + explosionForce.x, (moveDirection.y * speed) + explosionForce.y);
        }
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    void ShieldOn()
    {
        if (currentShieldCoolDownTime <= 0)
        {
            currentShieldCoolDownTime = 0;
            shield.SetActive(true);
            isShielded = true;
            currentShieldOnTime = shieldOnTime;
        }
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
        GameManager.instance.DisableAllItemCanvas();
        GameManager.instance.DisableRefreshItemCanvas();
        GameManager.instance.DisableLockItemCanvas();
        buffDescCanvas.SetActive(false);
        startNewWaveTxt.SetActive(false);
        weaponStatCanvas.SetActive(false);

        weaponCollider = Physics2D.OverlapCircle(transform.position, pickUpRadius, weaponMask);
        itemCollider = Physics2D.OverlapCircle(transform.position, pickUpRadius, itemMask);
        //buffCollider = Physics2D.OverlapCircle(transform.position, pickUpRadius, buffMask);
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
                Debug.Log("pick up weapon");
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

        if (itemCollider)
        {
            switch (itemCollider.gameObject.tag)
            {
                case "StartNewWave":
                    startNewWaveTxt.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        startNewWaveTxt.SetActive(false);
                        GameManager.instance.SetNewWaveStart(true);
                    }
                    break;
                case "RefreshItemCube":
                    GameManager.instance.ActiveRefreshItemCanvas();
                    if (Input.GetKeyDown(KeyCode.Return) && coin >= GameManager.instance.currentRefreshItemPrice)
                    {
                        GameManager.instance.RefreshNewItem();
                    }
                    break;
                case "LockItemCube":
                    GameManager.instance.ActiveLockItemCanvas();
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        GameManager.instance.LockItem();
                        Debug.Log(GameManager.instance.lockedItemList.Count);
                    }
                    break;
                case "Crate":
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        itemCollider.gameObject.GetComponent<LootBox>().CrateOpen();
                    }
                    break;
                case "Coin":
                    itemCollider.gameObject.GetComponent<HomingMissle>().enabled = true;
                    break;
                case "ItemPos":
                    itemCollider.gameObject.GetComponent<Item>().itemCanvas.SetActive(true);

                    switch (itemCollider.gameObject.GetComponent<Item>().item.GetComponent<ItemStat>().itemType)
                    {
                        case ItemStat.ItemType.Buff:
                            buffDesc.text = itemCollider.gameObject.GetComponent<Item>().item.GetComponent<ItemStat>().itemDescription;
                            buffDescCanvas.SetActive(true);
                            Buff buff = itemCollider.gameObject.GetComponent<Item>().item.GetComponent<Buff>();
                            if (Input.GetKeyDown(KeyCode.X) && coin >= itemCollider.gameObject.GetComponent<Item>().price)
                            {
                                Debug.Log(itemCollider.GetComponent<Item>().itemIndex);
                                GameManager.instance.spawnedItems.Remove(itemCollider.GetComponent<Item>().item);
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
                                        dmg += buff.buffAmount;
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
                                    case Buff.BuffType.AllowHealthRegen:
                                        allowHealthRegen = true;
                                        break;
                                    case Buff.BuffType.Dash:
                                        canDash = true;
                                        dashBg.SetActive(true);
                                        break;
                                    case Buff.BuffType.Shield:
                                        canUseShield = true;
                                        shieldBg.SetActive(true);
                                        break;
                                }
                                buff.gameObject.transform.parent = buffPos;
                                buff.gameObject.transform.localPosition = Vector3.zero;
                                StartCoroutine(buff.BuffPickUpEffect());
                                if (!buff.isSpecialBuff)
                                {
                                    if (pickUp.GetCurrentWeapon() != null)
                                    {
                                        pickUp.GetCurrentWeapon().gameObject.GetComponent<Gun>().ApplyBuff();
                                    }
                                    Debug.Log("buy item");
                                    Debug.Log(itemCollider.gameObject.GetComponent<Item>().price);
                                    coin -= itemCollider.gameObject.GetComponent<Item>().price;
                                    if (itemCollider.gameObject.GetComponent<Item>().item.GetComponent<ItemStat>().dropOneTime)
                                    {
                                        Debug.Log(GameManager.instance.items.RemoveAll(x => x.GetComponent<ItemStat>().itemName.Equals(itemCollider.gameObject.GetComponent<Item>().item.GetComponent<ItemStat>().itemName)));
                                    }
                                    UpdateUi();
                                }
                                else
                                {
                                    itemCollider.gameObject.SetActive(false);
                                }
                                itemCollider.gameObject.GetComponent<Item>().bc2d.enabled = false;
                            }
                            break;
                        case ItemStat.ItemType.Weapon:
                            Debug.Log("detect weapon");
                            weaponDmg.text = itemCollider.gameObject.GetComponent<Item>().item.gameObject.GetComponent<Gun>().Damage;
                            weaponRoF.text = itemCollider.gameObject.GetComponent<Item>().item.gameObject.GetComponent<Gun>().FireRate;
                            weaponEnergyConsume.text = itemCollider.gameObject.GetComponent<Item>().item.gameObject.GetComponent<Gun>().EnergyConsume;
                            weaponCriticalHit.text = itemCollider.gameObject.GetComponent<Item>().item.gameObject.GetComponent<Gun>().CriticalHitChance;
                            weaponAccuracy.text = itemCollider.gameObject.GetComponent<Item>().item.gameObject.GetComponent<Gun>().SpreadAngle;
                            weaponStatCanvas.SetActive(true);
                            if (Input.GetKeyDown(KeyCode.X) && coin >= itemCollider.gameObject.GetComponent<Item>().price)
                            {
                                GameManager.instance.spawnedItems.Remove(itemCollider.GetComponent<Item>().item);
                                Debug.Log("buy weapon");
                                Debug.Log(itemCollider.gameObject.GetComponent<Item>().price);
                                coin -= itemCollider.gameObject.GetComponent<Item>().price;
                                pickUp.PickUpWeapon(itemCollider.gameObject.GetComponent<Item>().item);
                                if (itemCollider.gameObject.GetComponent<Item>().item.GetComponent<ItemStat>().dropOneTime)
                                {
                                    Debug.Log(GameManager.instance.items.RemoveAll(x => x.GetComponent<ItemStat>().itemName.Equals(itemCollider.gameObject.GetComponent<Item>().item.GetComponent<ItemStat>().itemName)));
                                }
                                UpdateUi();
                                itemCollider.gameObject.GetComponent<Item>().bc2d.enabled = false;
                            }
                            break;
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
        if (isDashing || isShielded)
        {
            return;
        }
        int currentArmour = armour;
        if (dmg <= 0)
        {
            return;
        }
        armour -= dmg;
        //ShowDamage(dmg);
        if (armour <= 0)
        {
            armour = 0;
            health -= (dmg - currentArmour);
            if (health <= 0)
            {
                health = 0;
                Time.timeScale = 0.0f;
                reviveCoinText.text = (reviveCoin * deadTime).ToString();
                loseCanvas.SetActive(true);
                winLoseCanvas.SetActive(true);
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
    void RegenerateHealth()
    {
        if (health < (int)(maxHealth / 2) && allowHealthRegen)
        {
            health++;
            UpdateUi();
        }
    }

    public void ConsumeEnergy()
    {
        if (energy <= 0)
        {
            energy = 0;
            UpdateUi();
        }
        if (pickUp.GetCurrentWeapon() == null)
        {
            return;
        }

        if (pickUp.GetCurrentWeapon().GetComponent<Gun>().GetCurrentFireRate() <= 0 && Input.GetButton("Fire1"))
        {
            switch (pickUp.GetCurrentWeapon().GetComponent<Gun>().gunType)
            {
                case Gun.GunType.NormalGun:
                    //energy -= pickUp.GetCurrentWeapon().GetComponent<Gun>().energyConsume;
                    energyDeductionRate = 0;
                    UpdateUi();
                    break;
                case Gun.GunType.LaserGun:
                    //energy -= pickUp.GetCurrentWeapon().GetComponent<Gun>().energyConsume;
                    energyDeductionRate = pickUp.GetCurrentWeapon().GetComponent<Gun>().GetFireRate();
                    UpdateUi();
                    break;
                case Gun.GunType.Flamethrower:
                    //energy -= pickUp.GetCurrentWeapon().GetComponent<Gun>().energyConsume;
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
        coinText.text = coin.ToString();

        maxHealthText.text = $"+{currentMaxHealth}";
        maxArmourText.text = $"+{currenMaxtArmour}";
        maxEnergyText.text = $"+{currentMaxEnergy}";
        weaponDamageText.text = $"+{dmg}";
        weaponRofText.text = $"+{rateOfFire}";
        criticalChanceText.text = $"+{criticalChance}";
        criticalDamageText.text = $"+{currentCriticalDamage}";
        weaponAccuracyText.text = $"+{accuracy}";
        movementSpeedText.text = $"+{currentSpeed}";
        numberOfBulletText.text = $"+{numberOfBullet}";
    }

    public void SetMaxHealth(int moreHealth)
    {
        currentMaxHealth += moreHealth;
        maxHealth += moreHealth;
        healthSlider.maxValue = maxHealth;
        healthText.text = $"{health} / {maxHealth}";
    }
    public void SetMaxArmour(int moreArmour)
    {
        currenMaxtArmour += moreArmour;
        maxArmour += moreArmour;
        armourSlider.maxValue = maxArmour;
        armourText.text = $"{armour} / {maxArmour}";
    }
    public void SetMaxEnergy(int moreEnergy)
    {
        currentMaxEnergy += moreEnergy;
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
        currentSpeed += moreSpeed;
        speed += moreSpeed;
        currentMoveSpeed = speed;
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
        currentCriticalDamage += 2;
        criticalDmgMultiplier *= 2;
    }
    public void SetCanBounce(bool ableToBounce)
    {
        canBounce = ableToBounce;
    }

    public void AddMoreEnergy(int amount)
    {
        if (energy >= maxEnergy)
        {
            return;
        }
        energy += amount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void Revive()
    {
        if(coin >= reviveCoin * deadTime)
        {
            coin -= reviveCoin * deadTime;
            health = maxHealth;
            armour = maxArmour;
            energy = maxEnergy;
            UpdateUi();
            deadTime++;
            Time.timeScale = 1.0f;
            loseCanvas.SetActive(false);
            winLoseCanvas.SetActive(false);
            boxCollider.enabled = false;
            StartCoroutine(CountDownRevive());
        }
    }

    public void CancelRevive()
    {
        loseCanvas.SetActive(false);
        dieCanvas.SetActive(true);
    }

    public void Win()
    {
        Time.timeScale = 0.0f;
        monsterKillText.text = monsterKill.ToString();
        damageDealText.text = damageDeal.ToString();
        winCanvas.SetActive(true);
        winLoseCanvas.SetActive(true);
    }
    IEnumerator CountDownRevive()
    {
        yield return new WaitForSeconds(5f);
        boxCollider.enabled = true;
    }
}
