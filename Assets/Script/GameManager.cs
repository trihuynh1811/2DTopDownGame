using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Text timerText;
    [SerializeField] GameObject triggerNewWaveObject;
    [SerializeField] Transform spawnedItemPos;

    [SerializeField] float minSpawnX, maxSpawnX;
    [SerializeField] float minSpawnY, maxSpawnY;
    [SerializeField] float spawnRate;
    [SerializeField] int numberOfMonsterPerSpawn, maxTime, maxWave;
    [SerializeField] List<GameObject> monsterList;
    [SerializeField] GameObject spawnIndicator;

    [SerializeField] List<Transform> itemsPosList;
    [SerializeField] List<GameObject> items;
    [SerializeField] GameObject itemPosObject;
    List<GameObject> currentItemList;

    float currentSpawnRate;
    float spawnX, spawnY;
    int randomNumberOfMonsterToSpawn, time, wave = 1;
    Vector2 newSpawnPoint;
    bool timerRunning = true, newWaveStart, allStatsDisabled;
    public static List<GameObject> spawnedMonsterList = new List<GameObject>();
    List<GameObject> spawnedItems = new List<GameObject>();
    [HideInInspector] public static bool endOfWave;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        time = maxTime;
        timerText.text = time.ToString();
        triggerNewWaveObject.SetActive(false);
        itemPosObject.SetActive(false);
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
            currentItemList.Clear();
            foreach(GameObject monster in spawnedMonsterList)
            {
                Destroy(monster);
            }
            DisableStats();
            itemPosObject.SetActive(false);
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
                currentItemList = items.ToList();
                for (int i = 0; i < itemsPosList.Count; i++)
                {
                    int randomItemIndex = Random.Range(0, currentItemList.Count);

                    GameObject item = Instantiate(currentItemList[randomItemIndex], itemsPosList[i].transform.position, Quaternion.identity);
                    itemsPosList[i].GetComponent<Item>().itemNameText.text = item.GetComponent<ItemStat>().itemName;
                    itemsPosList[i].GetComponent<Item>().itemPriceText.text = item.GetComponent<ItemStat>().price.ToString();
                    currentItemList.RemoveAt(randomItemIndex);
                    item.transform.parent = spawnedItemPos;
                    //currentItemList.Remove(item);
                    spawnedItems.Add(item);
                }
                itemPosObject.SetActive(true);
            }
        }

    }

    public void DisableStats()
    {
        itemsPosList.ForEach(x => x.GetComponent<Item>().itemCanvas.SetActive(false));
    }

    public void RefreshNewItem()
    {
        DisableStats();
        spawnedItems.ForEach(x => x.SetActive(false));
        currentItemList = items.ToList();
        for (int i = 0; i < itemsPosList.Count; i++)
        {
            int randomItemIndex = Random.Range(0, currentItemList.Count);

            GameObject item = Instantiate(currentItemList[randomItemIndex], itemsPosList[i].transform.position, Quaternion.identity);
            itemsPosList[i].GetComponent<Item>().itemNameText.text = item.GetComponent<ItemStat>().itemName;
            itemsPosList[i].GetComponent<Item>().itemPriceText.text = item.GetComponent<ItemStat>().price.ToString();
            currentItemList.RemoveAt(randomItemIndex);
            item.transform.parent = spawnedItemPos;
            //currentItemList.Remove(item);
            spawnedItems.Add(item);
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
