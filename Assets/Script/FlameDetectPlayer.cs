using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDetectPlayer : MonoBehaviour
{
    public enum EnemyType
    {
        Player,
        Enemy
    }
    public EnemyType enemyType;
    public int damge;
    public float damageRate;
    void OnParticleCollision(GameObject other)
    {
        switch (enemyType)
        {
            case EnemyType.Player:
                if (other.gameObject.CompareTag("Player") && damageRate <= 0)
                {
                    other.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage(damge);
                }
                break;
            case EnemyType.Enemy:
                if(other.gameObject.CompareTag("Enemy") && damageRate <= 0)
                {
                    other.gameObject.GetComponent<EnemyTakeDmg>().TakeDamage(damge);
                }
                break;
        }


    }

}
