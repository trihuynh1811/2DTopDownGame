using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] int maxBounceTime;
    [SerializeField] float maxExistTime;
    [SerializeField] bool canBounce;
    Vector3 lastVelocity;
    int currentBounceTime;

    private void Start()
    {
        if (!canBounce)
        {
            StartCoroutine(DestroyAfter());
        }
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Destroy(gameObject);
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Destroy(gameObject);
                break;

            case "Ground/Wall":
                if (canBounce)
                {
                    currentBounceTime++;
                    if (currentBounceTime > maxBounceTime)
                    {
                        Destroy(gameObject);
                    }
                    var speed = lastVelocity.magnitude;
                    var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
                    transform.right = direction.normalized;
                    rb.velocity = direction.normalized * speed;
                }
                else
                {
                    Destroy(gameObject);
                }
                break;
        }
    }


    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(maxExistTime);
        Destroy(gameObject);
    }
}
