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
    [SerializeField] float ringRotationSpeed, minRotateTime, maxRotateTime, laserLength, damageRate, randomLaserTime;
    [SerializeField] int laserDmg;
    [SerializeField] LayerMask hitMask;
    float currentRotateTime, currentDamageRate, currentRandomLaserTime;

    // Start is called before the first frame update
    void Start()
    {
        currentRotateTime = Random.Range(minRotateTime, maxRotateTime);
        currentRandomLaserTime = randomLaserTime;
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //RotateRing();
        //if (currentDamageRate > 0) currentDamageRate -= Time.deltaTime;
        if(currentRandomLaserTime > 0)
        {
            ActivateLaserRandomly();
        }
        currentRandomLaserTime -= Time.deltaTime;
        if(currentRandomLaserTime <= 0)
        {
            currentRandomLaserTime = 0;
        }
    }

    void RotateRing()
    {
        if (currentRotateTime > 0)
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

    IEnumerator ActivateSingleLaser(int index)
    {
        shootPointList[index].transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(.25f);
        shootPointList[index].transform.GetChild(1).gameObject.SetActive(true);
        RaycastHit2D hit = Physics2D.Raycast(shootPointList[index].transform.GetChild(1).gameObject.transform.position,
            shootPointList[index].transform.GetChild(1).gameObject.transform.right, laserLength, hitMask);

        if (hit)
        {
            hit.collider.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage(laserDmg * 2);
        }
        yield return new WaitForSeconds(0.25f);
        shootPointList[index].transform.GetChild(0).gameObject.SetActive(false);
        shootPointList[index].transform.GetChild(1).gameObject.SetActive(false);
        shootPointList[index].gameObject.SetActive(false);
    }

    void ActivateLaser()
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(true);

            RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, laser.transform.right, laserLength, hitMask);

            if (hit)
            {
                if (currentDamageRate <= 0)
                {
                    hit.collider.gameObject.GetComponent<TopDownPlayerMovement>().TakeDamage(laserDmg);
                    currentDamageRate = damageRate;
                }
            }
        }
    }

    void ActivateLaserRandomly()
    {
        int randomShootpointIndex = Random.Range(1, shootPointList.Count);
        shootPointList[randomShootpointIndex].gameObject.SetActive(true);
        StartCoroutine(ActivateSingleLaser(randomShootpointIndex));
    }

    // this function is for inspector button click event
    public void CalculateRotation()
    {
        if (shootPointList.Count <= 0)
        {
            return;
        }
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
            shootPointClone.transform.localScale = new(1, 1, 0);

            shootPointList.Add(shootPointClone.transform);
        }

    }
}
