using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public enum Boss
    {
        MechaGolem
    }

    [SerializeField] GameObject ring;
    [SerializeField] List<GameObject> drone, laserLines, lasers;
    [SerializeField] float ringRotationSpeed, minRotateTime, maxRotateTime, laserLength, damageRate;
    [SerializeField] LayerMask hitMask;
    float currentRotateTime, currentDamageRate;

    // Start is called before the first frame update
    void Start()
    {
        currentRotateTime = Random.Range(minRotateTime, maxRotateTime);
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateRing();
        if (currentDamageRate > 0) currentDamageRate -= Time.deltaTime;
    }

    void RotateRing()
    {
        if(currentRotateTime > 0)
        {
            ring.transform.Rotate(0, 0, ringRotationSpeed * Time.deltaTime);
            currentRotateTime -= Time.deltaTime;
        }
        else
        {
            ring.transform.Rotate(0, 0, 0);
            StartCoroutine(ResetRandomRotateTime());
        }
    }

    IEnumerator ResetRandomRotateTime()
    {
        ActivateLaser();
        yield return new WaitForSeconds(1f);
        currentRotateTime = Random.Range(minRotateTime, maxRotateTime);
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
        currentDamageRate = 0;
    }

    void ActivateLaser()
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(true);

            RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, laser.transform.right, laserLength, hitMask);

            if (hit && currentDamageRate <= 0)
            {
                hit.collider.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage();
                currentDamageRate = damageRate;
            }
        }
    }
}
