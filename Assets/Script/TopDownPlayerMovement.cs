using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownPlayerMovement : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Vector3 offset;
    [SerializeField] PickUp pickUp;
    [SerializeField] LayerMask pickUpMask;
    [SerializeField] BoxCollider2D boxCollider;

    [SerializeField] Slider healthSlider, armourSlider, energySlider;
    [SerializeField] float health, armour, energy;
    [SerializeField] float maxHealth, maxArmour, maxEnergy;
    [SerializeField] float regenerateArmourRate, regenerateEnegeyRate;

    public Text healthText, armourText, energyText;

    [SerializeField] Transform gunPos;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    Vector2 moveDirection;
    bool m_FacingRight = true;
    float moveX, moveY;
    Collider2D collider_;
    float energyDeductionRate;
    // Start is called before the first frame update

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

        InvokeRepeating("RegenerateArmor", 0f, regenerateArmourRate);
        InvokeRepeating("RegenerateEnergy", 0f, regenerateEnegeyRate);
    }
    private void Update()
    {
        cam.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, offset.z);
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (!m_FacingRight && delta.x > 0)
        {
            Flip();
        }
        else if (m_FacingRight && delta.x < 0)
        {
            Flip();
        }
        GetInput();
        RotateGun();
        if(energyDeductionRate <= 0)
        {
            ConsumeEnergy();
        }
        if (energyDeductionRate > 0) energyDeductionRate -= Time.deltaTime;
        collider_ = Physics2D.OverlapCircle(transform.position, 3f, pickUpMask);
        if (collider_)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                pickUp.PickUpWeapon(collider_.gameObject);
            }
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
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    void GetInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
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

    public void OnDrawGizmos()
    {

    }

    public void TakeDamage(int dmg)
    {
        armour -= dmg;
        if (armour <= 0)
        {
            armour = 0;
            health -= dmg;
            if (health <= 0)
            {
                health = 0;
            }
        }
        UpdateUi();

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

    void UpdateUi()
    {
        healthSlider.value = health;
        armourSlider.value = armour;
        energySlider.value = energy;

        healthText.text = $"{health} / {maxHealth}";
        armourText.text = $"{armour} / {maxArmour}";
        energyText.text = $"{energy} / {maxEnergy}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
    }
}
