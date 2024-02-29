using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletBelongTo
    {
        Player,
        Enemy
    }
    public BulletBelongTo bulletBelongTo;
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
        switch (bulletBelongTo)
        {
            case BulletBelongTo.Player:
                canBounce = TopDownPlayerMovement.instance.canBounce;
                currentBounceTime = 0;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (bulletBelongTo)
        {
            case BulletBelongTo.Enemy:
                switch (collision.gameObject.tag)
                {
                    case "Player":
                        collision.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage(damage);
                        ObjectPoolManager.ReturnObjectToPool(gameObject);
                        break;

                    case "Ground/Wall":
                        ObjectPoolManager.ReturnObjectToPool(gameObject);
                        break;
                }
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (bulletBelongTo)
        {
            case BulletBelongTo.Player:
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
        while (elapsedTime < maxExistTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
