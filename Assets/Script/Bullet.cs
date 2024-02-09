using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] int maxBounceTime;
    [SerializeField] float maxExistTime;
    public bool canBounce { get; set; }
    int damage;
    Vector3 lastVelocity;
    int currentBounceTime;
    Coroutine _returnToPoolTime;

    private void OnEnable()
    {
        _returnToPoolTime = StartCoroutine(DestroyAfter());
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
                ObjectPoolManager.ReturnObjectToPool(gameObject);
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.gameObject.GetComponent<EnemyTakeDmg>().TakeDamage(damage);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
                break;

            case "Ground/Wall":
                if (canBounce)
                {
                    currentBounceTime++;
                    if (currentBounceTime > maxBounceTime)
                    {
                        ObjectPoolManager.ReturnObjectToPool(gameObject);
                    }
                    var speed = lastVelocity.magnitude;
                    var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
                    transform.right = direction.normalized;
                    rb.velocity = direction.normalized * speed;
                }
                else
                {
                    ObjectPoolManager.ReturnObjectToPool(gameObject);
                }
                break;
        }
    }

    public void SetDmg(int dmg)
    {
        damage = dmg;
    }

    IEnumerator DestroyAfter()
    {
        //Destroy(gameObject);
        float elapsedTime = 0;
        while(elapsedTime < maxExistTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
