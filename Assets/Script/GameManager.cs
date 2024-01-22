using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] GameObject triggerNewWaveObject;

    [SerializeField] float minSpawnX, maxSpawnX;
    [SerializeField] float minSpawnY, maxSpawnY;
    [SerializeField] float spawnRate;
    [SerializeField] int numberOfMonsterPerSpawn, maxTime, maxWave;
    [SerializeField] List<GameObject> monsterList;
    [SerializeField] GameObject spawnIndicator;
    float currentSpawnRate;
    float spawnX, spawnY;
    int randomNumberOfMonsterToSpawn, time, wave = 1;
    Vector2 newSpawnPoint;
    bool timerRunning = true, newWaveStart;
    public static List<GameObject> spawnedMonsterList = new List<GameObject>();
    [HideInInspector] public static bool endOfWave;

    // Start is called before the first frame update
    void Start()
    {
        time = maxTime;
        timerText.text = time.ToString();
        triggerNewWaveObject.SetActive(false);
        InvokeRepeating("CountDown", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawnRate > 0) currentSpawnRate -= Time.deltaTime;
        if (currentSpawnRate <= 0 && timerRunning)
        {
            Spawn();
        }
        if (newWaveStart)
        {
            time = maxTime;
            timerText.text = time.ToString();
            timerRunning = true;
            newWaveStart = false;
            endOfWave = false;
            foreach(GameObject monster in spawnedMonsterList)
            {
                Destroy(monster);
            }
            spawnedMonsterList.Clear();
            triggerNewWaveObject.SetActive(false);
            wave++;
            Debug.Log(wave);
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
            spawnIndicator.GetComponent<ActivateMonster>().monster = randomMonster();

            GameObject spawnIndicatorClone = Instantiate(spawnIndicator, newSpawnPoint, Quaternion.identity);
        }
    }

    void CountDown()
    {
        if (timerRunning)
        {
            if (time > 0)
            {
                time -= 1;
                timerText.text = time.ToString();
            }
            else
            {
                time = 0;
                timerText.text = time.ToString();
                timerRunning = false;
                triggerNewWaveObject.SetActive(true);
                endOfWave = true;
            }
        }

    }

    GameObject randomMonster()
    {
        int randomIndex = Random.Range(0, monsterList.Count);

        return monsterList[randomIndex];
    }

    public void SetNewWaveStart(bool startNewWave)
    {
        newWaveStart = startNewWave;
    }
}
