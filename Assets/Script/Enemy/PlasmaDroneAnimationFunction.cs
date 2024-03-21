using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaDroneAnimationFunction : MonoBehaviour
{
    [SerializeField] EnemyAttack enemyAttack;

    public void ShootBullet()
    {
        enemyAttack.ShootBullet();
    }

    public void ResetAnimation()
    {
        enemyAttack.animator.Rebind();
    }
}
