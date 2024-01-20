using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float minSpawnX, maxSpawnX;
    [SerializeField] float minSpawnY, maxSpawnY;
    [SerializeField] float spawnRate;
    [SerializeField] int numberOfMonsterPerSpawn;
    //[SerializeField] List<GameObject> monsterList;
    [SerializeField] GameObject spawnIndicator;
    float currentSpawnRate;
    float spawnX, spawnY;
    int randomNumberOfMonsterToSpawn;
    Vector2 newSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawnRate > 0) currentSpawnRate -= Time.deltaTime;
        if(currentSpawnRate <= 0)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        currentSpawnRate = spawnRate;

        randomNumberOfMonsterToSpawn = Random.Range(1, numberOfMonsterPerSpawn);

        for (int i = 0; i < randomNumberOfMonsterToSpawn; i++)
        {
            spawnX = Random.Range(minSpawnX, maxSpawnX);
            spawnY = Random.Range(minSpawnY, maxSpawnY);

            newSpawnPoint = new Vector2(transform.position.x + spawnX, transform.position.y + spawnY);

            GameObject spawnIndicatorClone = Instantiate(spawnIndicator, newSpawnPoint, Quaternion.identity);
        }
    }
}
