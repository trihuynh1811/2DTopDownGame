using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformerPlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] float speed = 8f;
    [SerializeField] float jumpingPower = 16f;
    private bool m_FacingRight = true;

    [SerializeField] Transform cam;
    [SerializeField] Vector3 offset;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] Animator animator;
    [SerializeField] float health;
    [SerializeField] Slider healthSlider;
    RaycastHit2D hit;

    private void Start()
    {
        healthSlider.value = health;
    }

    void Update()
    {
        cam.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, offset.z);
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // If the input is moving the player right and the player is facing left...
        if (!m_FacingRight && delta.x > 0)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (m_FacingRight && delta.x < 0)
        {
            // ... flip the player.
            Flip();
        }
        animator.SetBool("IsJumping", rb.velocity.y > 0.01f);
        JumpEnemy();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (IsGrounded())
        {
            animator.SetFloat("Speed", rb.velocity.magnitude);
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void JumpEnemy()
    {
        hit = Physics2D.Raycast(groundCheck.position, -groundCheck.up, .1f, enemyMask);
        if (hit)
        {
            Destroy(hit.collider.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(groundCheck.position, -groundCheck.up * .1f);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, -180f, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 10;
            healthSlider.value = health;
        }
    }
}
