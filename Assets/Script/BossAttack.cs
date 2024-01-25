using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public enum Boss
    {
        MechaGolem
    }
    [SerializeField] float rotation, numberOfShootPoint, radiusMultiplier;
    [SerializeField] List<Transform> shootPointList;

    [SerializeField] GameObject ring;
    [SerializeField] List<GameObject> drone, laserLines, lasers;
    [SerializeField] float ringRotationSpeed, minRotateTime, maxRotateTime, laserLength, damageRate;
    [SerializeField] int laserDmg;
    [SerializeField] LayerMask hitMask;
    float currentRotateTime, currentDamageRate, calculatedRotation;

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

            if (hit)
            {
                if(currentDamageRate <= 0)
                {
                    hit.collider.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage(laserDmg);
                    currentDamageRate = damageRate;
                }
            }
        }
    }

    public void CalculateRotation()
    {
        if(shootPointList.Count <= 0)
        {
            return;
        }
        calculatedRotation = rotation / numberOfShootPoint;
        //foreach(Transform shootPoint in shootPointList)
        //{
        //    shootPoint.localRotation 
        //}
        float radius = Mathf.Min(ring.transform.localScale.x, ring.transform.localScale.y) * radiusMultiplier;

        // Calculate the starting angle to ensure the first transform is at the top
        float startAngle = 90f;

        for (int i = 0; i < numberOfShootPoint; i++)
        {
            GameObject shootPointClone = Instantiate(shootPointList[0].gameObject, ring.transform.position, Quaternion.identity);
            shootPointClone.transform.parent = ring.transform;

            float angle = startAngle + i * rotation / numberOfShootPoint;
            float radians = Mathf.Deg2Rad * angle;

            float x = Mathf.Cos(radians) * radius;
            float y = Mathf.Sin(radians) * radius;

            Vector3 position = new Vector3(x, y, 0f);

            shootPointClone.transform.localPosition = position;
            shootPointClone.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

    }
}
