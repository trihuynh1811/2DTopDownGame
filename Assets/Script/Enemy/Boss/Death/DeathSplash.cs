using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSplash : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attack_3;
    [SerializeField] List<GameObject> splash;
    [SerializeField] List<Transform> splashPos;
    [SerializeField] List<float> splashDirection;
    [SerializeField] float splashSpeed, splashRadius;
    [SerializeField] int dmg;
    [SerializeField] GameObject deathObject;

    [SerializeField] GameObject shootPointListObject;
    [SerializeField] List<Transform> shootPointList;
    [SerializeField] EnemyChasePlayerTest enemyChasePlayerTest;

    int splashIndex = 0;
    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Attack()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= splashRadius)
        {
            TopDownPlayerMovement.instance.TakeDamage(dmg);
        }
        // Determine the direction from the enemy to the player
        Vector3 directionToPlayer = player.transform.position - transform.position;

        splashIndex = 0;
        Vector2 direction = Quaternion.Euler((directionToPlayer.x > 0f) ? 0f : 150f, 0, (directionToPlayer.x > 0f) ? splashDirection[splashIndex] : -splashDirection[splashIndex]) * splashPos[splashIndex].right;
        GameObject splashClone = ObjectPoolManager.SpawnObject(splash[splashIndex], splashPos[splashIndex].position, Quaternion.Euler((directionToPlayer.x > 0f) ? 0f : 150f, 0, (directionToPlayer.x > 0f) ? splashDirection[splashIndex] : -splashDirection[splashIndex]), ObjectPoolManager.PoolType.GameObject);
        splashClone.GetComponent<Bullet>().SetDmg(dmg);
        splashClone.transform.right = direction.normalized;
        splashClone.GetComponent<Rigidbody2D>().AddForce(direction.normalized * splashSpeed, ForceMode2D.Force);
        splashIndex++;
    }

    void Attack_2()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= splashRadius)
        {
            TopDownPlayerMovement.instance.TakeDamage(dmg);
        }
    }


    void Attack_3()
    {
        for (int i = 0; i < shootPointList.Count; i++)
        {
            Debug.Log(i);
            GameObject splashClone = Instantiate(splash[0], shootPointList[i].position, shootPointList[i].rotation);
            splashClone.GetComponent<Bullet>().SetDmg(15);
            splashClone.GetComponent<Rigidbody2D>().AddForce(shootPointList[i].right.normalized * splashSpeed, ForceMode2D.Force);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }

    void ResetAnimation()
    {
        animator.Rebind();
        Time.timeScale = 1;
    }
}
