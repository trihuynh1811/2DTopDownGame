using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemFireBall : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform fireBallSpawnPos;
    [SerializeField] GameObject fireBall;
    [SerializeField] float fireBallSpeed;
    [SerializeField] int fireBallDmg;

    public void ShootFireBall()
    {
        GameObject fireBallClone = ObjectPoolManager.SpawnObject(fireBall, fireBallSpawnPos.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        fireBallClone.GetComponent<Rigidbody2D>().AddForce(fireBallSpawnPos.right.normalized * fireBallSpeed, ForceMode2D.Force);
        fireBallClone.GetComponent<Bullet>().SetDmg(fireBallDmg);
        animator.Rebind();
    }
}
