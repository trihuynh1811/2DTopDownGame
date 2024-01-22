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

    public Slider healthSlider, armourSlider, energySlider;
    public float health, armour, energy;
    public float maxHealth, maxArmour, maxEnergy;

    public Text healthText, armourText, energyText;

    [SerializeField] Transform gunPos;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    Vector2 moveDirection;
    bool m_FacingRight = true;
    float moveX, moveY;
    Collider2D collider_;
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

    public void TakeDamage()
    {
        health -= 10;
        healthSlider.value = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
    }
}
