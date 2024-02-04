using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum AttackType
    {
        SelfDestruct,
        ShootLaser
    }
    public AttackType attackType;
    [SerializeField] GameObject player;
    [SerializeField] GameObject deathEffect;
    [SerializeField] float explosionForce, explosionTime, splashRadius;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    public void Attack()
    {
        switch (attackType)
        {
            case AttackType.SelfDestruct:
                Suicide();
                break;
            default:
                Debug.Log("Attacking the player");
                break;
        }
    }

    void Suicide()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Vector2 explosionVector = (player.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;
        TopDownPlayerMovement.instance.explosionForce =  new Vector2(explosionVector.x * explosionForce, explosionVector.y * explosionForce);
        TopDownPlayerMovement.instance.explosionTime = explosionTime;
        TopDownPlayerMovement.instance.fadeDuration = explosionTime;
        gameObject.SetActive(false);
    }
}
