using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrapHomingMissle : MonoBehaviour
{
    [SerializeField] float speed, rotationSpeed;
    [SerializeField] Rigidbody2D rb;
    public Transform target { get; set; }
    [SerializeField] float startHomingTime = 2;
    [SerializeField] GameObject impactEffect;
    [SerializeField] float splashRadius, explosionForce, explosionTime;
    [SerializeField] int explosionDmg;
    float currentStartHomingTime;
    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStartHomingTime = startHomingTime;
    }
    private void OnEnable()
    {
        currentStartHomingTime = startHomingTime;
    }
    private void FixedUpdate()
    {
        if (currentStartHomingTime > 0) currentStartHomingTime -= Time.deltaTime;
        if(currentStartHomingTime <= 0)
        {
            FlyTowardTarget();
        }
    }
    void FlyTowardTarget()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotationSpeed;

        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target") && collision.gameObject.name.Equals(target.name))
        {
            ObjectPoolManager.SpawnObject(impactEffect, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystem);
            collision.gameObject.SetActive(false);
            if ((int)Vector2.Distance(rb.position, player.transform.position) <= splashRadius)
            {
                Vector2 explosionVector = ((Vector2)player.GetComponent<Rigidbody2D>().transform.position - rb.position).normalized;
                TopDownPlayerMovement.instance.explosionForce = new Vector2(explosionVector.x * explosionForce, explosionVector.y * explosionForce);
                TopDownPlayerMovement.instance.explosionTime = explosionTime;
                TopDownPlayerMovement.instance.fadeDuration = explosionTime;
                TopDownPlayerMovement.instance.TakeDamage(explosionDmg);
            }
            gameObject.SetActive(false);
        }
    }
}
