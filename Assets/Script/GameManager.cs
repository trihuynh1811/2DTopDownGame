using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Text timerText;
    [SerializeField] TextMeshProUGUI refreshItemPriceText, LockItemText, lockText, waveCountDownText, waveNumberText;
    [HideInInspector] public int time;
    [SerializeField] GameObject triggerNewWaveObject, refreshItemCanvas, lockItemCanvas, waveCanvas;
    [SerializeField] int refreshItemPrice;
    [SerializeField] Transform spawnedItemPos, bossSpawnPos;

    [SerializeField] float minSpawnX, maxSpawnX;
    [SerializeField] float minSpawnY, maxSpawnY;
    [SerializeField] List<Vector2> maxXYPos;
    [SerializeField] List<Vector2> minXYPos;
    [SerializeField] float spawnRate;
    [SerializeField] int numberOfMonsterPerSpawn, maxTime, maxWave, minSurvivalTime, maxSurvivalTime;
    [SerializeField] List<GameObject> monsterList;
    List<GameObject> currentMonsterList;
    public int listIndex, numberOfMonster;
    [SerializeField] GameObject spawnIndicator;

    [SerializeField] List<Transform> itemsPosList;
    public List<GameObject> items, maps, walls, bosses;
    [SerializeField] GameObject itemPosObject;
    public List<GameObject> currentItemList { get; set; }

    float currentSpawnRate, currentWaveCountDown = 3;
    float spawnX, spawnY;
    int randomNumberOfMonsterToSpawn, wave = 1, numberOfRefreshTime = 1;
    public int currentRefreshItemPrice { get; set; }
    Vector2 newSpawnPoint;
    bool timerRunning = true, newWaveStart, allStatsDisabled;
    public static List<GameObject> spawnedMonsterList = new List<GameObject>();
    public static List<GameObject> itemList = new List<GameObject>();
    public List<BoxCollider2D> boxCollider2dList = new List<BoxCollider2D>();
    public List<GameObject> spawnedItems { get; set; } = new List<GameObject>();
    public List<GameObject> lockedItemList = new List<GameObject>();
    public List<GameObject> lockList;
    public static bool endOfWave;
    public static bool itemsLocked;

    private void Awake()
    {
        instance = this;
        currentRefreshItemPrice = refreshItemPrice;
        boxCollider2dList.ForEach(collider => collider.enabled = false);
        lockList.ForEach(l => l.SetActive(false));
        for (int i = 0; i < itemsPosList.Count; i++)
        {
            itemsPosList[i].GetComponent<Item>().itemIndex = i;
        }
        waveNumberText.text = wave.ToString();
        maxSpawnX = maxXYPos[0].x;
        maxSpawnY = maxXYPos[0].y;
        minSpawnX = minXYPos[0].x;
        minSpawnY = minXYPos[0].y;
        currentMonsterList = monsterList.GetRange(listIndex, numberOfMonster);
    }

    // Start is called before the first frame update
    void Start()
    {
        time = maxTime;
        timerText.text = time.ToString();
        triggerNewWaveObject.SetActive(false);
        refreshItemCanvas.SetActive(false);
        lockItemCanvas.SetActive(false);
        itemPosObject.SetActive(false);
        waveCanvas.SetActive(false);
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
            wave++;
            NewWaveCountDown();
            Debug.Log(wave);
        }
    }

    void Spawn()
    {
        currentSpawnRate = spawnRate;
        if (wave % 4 == 0 && wave < 15)
        {
            numberOfMonster = numberOfMonster < monsterList.Count - 1 ? numberOfMonster + 1 : monsterList.Count - 1;
            currentMonsterList = monsterList.GetRange(listIndex, numberOfMonster);
        }
        if (wave == 13)
        {
            currentMonsterList.Add(monsterList[^1]);
            monsterList.Remove(monsterList[^1]);
        }
        else if (wave > 13)
        {
            currentMonsterList = monsterList.ToList();
        }
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

    void NewWaveCountDown()
    {
        currentWaveCountDown = 3;
        numberOfRefreshTime = 1;
        time = maxTime;
        timerText.text = time.ToString();
        newWaveStart = false;
        endOfWave = false;
        itemsLocked = false;
        lockText.text = "Lock Items";
        currentItemList.Clear();
        lockList.ForEach(l => l.SetActive(false));
        spawnedItems.ForEach(x => x.SetActive(false));
        foreach (GameObject monster in spawnedMonsterList)
        {
            Destroy(monster);
        }
        DisableStats();
        itemPosObject.SetActive(false);
        spawnedMonsterList.Clear();
        itemList.Clear();
        triggerNewWaveObject.SetActive(false);

        StartCoroutine(BeginNewWave());


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
                //if (lockedItemList.Count > 0)
                //{
                //    itemsPosList.ForEach(x => x.gameObject.SetActive(false));
                //    boxCollider2dList.ForEach(x => x.enabled = false);
                //    Debug.Log("locked item list count: " + lockedItemList.Count);
                //    for (int i = 0; i < lockedItemList.Count; i++)
                //    {
                //        Debug.Log("i: " + i);
                //        int itemPosIndex = itemsPosList.Count - (i + 1);
                //        Debug.Log(lockedItemList[i].name);
                //        Debug.Log(itemsPosList[itemPosIndex].name);
                //        GameObject item = Instantiate(lockedItemList[i], itemsPosList[itemPosIndex].transform.position, Quaternion.identity);
                //        itemsPosList[i].GetComponent<Item>().price = item.GetComponent<ItemStat>().price;
                //        itemsPosList[itemPosIndex].GetComponent<Item>().itemNameText.text = item.GetComponent<ItemStat>().itemName;
                //        itemsPosList[itemPosIndex].GetComponent<Item>().itemPriceText.text = item.GetComponent<ItemStat>().price.ToString();
                //        itemsPosList[itemPosIndex].GetComponent<Item>().item = item.gameObject;
                //        item.transform.parent = spawnedItemPos;
                //        //spawnedItems.Add(item);
                //        item.SetActive(true);
                //        itemsPosList[itemPosIndex].gameObject.SetActive(true);
                //        //Debug.Log(itemPosIndex);
                //        boxCollider2dList[itemPosIndex].enabled = true;
                //    }
                //    itemPosObject.SetActive(true);
                //    spawnedItems = lockedItemList;
                //    lockedItemList.Clear();

                //}
                //else
                //{
                itemsPosList.ForEach(x => x.gameObject.SetActive(false));
                boxCollider2dList.ForEach(collider => collider.enabled = false);
                currentItemList = lockedItemList.Count > 0 ? lockedItemList : items.ToList();
                int itemPosListCount = currentItemList.Count < itemsPosList.Count ? currentItemList.Count : itemsPosList.Count;
                for (int i = 0; i < itemPosListCount; i++)
                {
                    int randomItemIndex = Random.Range(0, currentItemList.Count);

                    GameObject item = Instantiate(currentItemList[randomItemIndex], itemsPosList[i].transform.position, Quaternion.identity);
                    itemsPosList[i].GetComponent<Item>().price = item.GetComponent<ItemStat>().price;
                    itemsPosList[i].GetComponent<Item>().itemNameText.text = item.GetComponent<ItemStat>().itemName;
                    itemsPosList[i].GetComponent<Item>().itemPriceText.text = item.GetComponent<ItemStat>().price.ToString();
                    itemsPosList[i].GetComponent<Item>().item = item.gameObject;
                    currentItemList.RemoveAt(randomItemIndex);
                    item.transform.parent = spawnedItemPos;
                    item.SetActive(true);
                    itemsPosList[i].gameObject.SetActive(true);
                    boxCollider2dList[i].enabled = true;
                    //currentItemList.Remove(item);
                    spawnedItems.Add(item);
                }
                //itemsPosList.ForEach(x => x.gameObject.SetActive(true));
                //boxCollider2dList.ForEach(collider => collider.enabled = true);
                itemPosObject.SetActive(true);
                //}
                for (int i = 0; i < itemList.Count; i++)
                {
                    itemList[i].GetComponent<HomingMissle>().enabled = true;
                }
            }
        }

    }

    IEnumerator BeginNewWave()
    {
        waveNumberText.text = $"Wave {wave}";
        waveCanvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        waveCanvas.SetActive(false);
        if (wave % 5 == 0)
        {
            maps.ForEach(map => map.SetActive(false));
            walls.ForEach(wall => wall.SetActive(false));
            int index = ((wave - 5) / 5);
            maps[index].SetActive(true);
            walls[index].SetActive(true);
            maxSpawnX = maxXYPos[index].x;
            maxSpawnY = maxXYPos[index].y;
            minSpawnX = minXYPos[index].x;
            minSpawnY = minXYPos[index].y;
            Instantiate(bosses[index], bossSpawnPos.position, Quaternion.identity);
        }
        timerRunning = true;
    }

    public void DisableStats()
    {
        itemsPosList.ForEach(x => x.GetComponent<Item>().itemCanvas.SetActive(false));
    }

    public void RefreshNewItem()
    {
        TopDownPlayerMovement.instance.coin -= currentRefreshItemPrice * numberOfRefreshTime;
        numberOfRefreshTime++;
        lockedItemList.ForEach(x => x.SetActive(false));
        lockedItemList.Clear();
        DisableStats();
        spawnedItems.ForEach(x => x.SetActive(false));
        spawnedItems.Clear();
        currentItemList = items.ToList();
        for (int i = 0; i < itemsPosList.Count; i++)
        {
            int randomItemIndex = Random.Range(0, currentItemList.Count);

            GameObject item = Instantiate(currentItemList[randomItemIndex], itemsPosList[i].transform.position, Quaternion.identity);
            itemsPosList[i].GetComponent<Item>().price = item.GetComponent<ItemStat>().price;
            itemsPosList[i].GetComponent<Item>().itemNameText.text = item.GetComponent<ItemStat>().itemName;
            itemsPosList[i].GetComponent<Item>().itemPriceText.text = item.GetComponent<ItemStat>().price.ToString();
            itemsPosList[i].GetComponent<Item>().item = item.gameObject;
            currentItemList.RemoveAt(randomItemIndex);
            item.transform.parent = spawnedItemPos;
            //currentItemList.Remove(item);
            spawnedItems.Add(item);
        }
        itemsPosList.ForEach(x => x.gameObject.SetActive(true));
        boxCollider2dList.ForEach(collider => collider.enabled = true);
    }

    public void LockItem()
    {
        Debug.Log(spawnedItems.Count);
        lockedItemList = spawnedItems.ToList();
        if (!itemsLocked)
        {
            for (int i = 0; i < itemsPosList.Count; i++)
            {
                if (lockedItemList.IndexOf(itemsPosList[i].gameObject.GetComponent<Item>().item) > -1)
                {
                    lockList[i].SetActive(true);
                }
            }
            itemsLocked = true;
            lockText.text = "Unlock Items";
            return;
        }
        for (int i = 0; i < lockedItemList.Count; i++)
        {
            lockList[i].SetActive(false);
        }
        lockedItemList.Clear();
        lockText.text = "Lock Items";
        itemsLocked = false;

    }

    GameObject randomMonster()
    {
        int randomIndex = Random.Range(0, currentMonsterList.Count);

        return currentMonsterList[randomIndex];
    }

    public void SetNewWaveStart(bool startNewWave)
    {
        newWaveStart = startNewWave;
    }

    public void DisableAllItemCanvas()
    {
        itemsPosList.ForEach(x => x.GetComponent<Item>().itemCanvas.SetActive(false));
    }

    public void ActiveRefreshItemCanvas()
    {
        refreshItemPriceText.text = (currentRefreshItemPrice * numberOfRefreshTime).ToString();
        refreshItemCanvas.SetActive(true);
    }

    public void DisableRefreshItemCanvas()
    {
        refreshItemCanvas.SetActive(false);
    }

    public void ActiveLockItemCanvas()
    {
        lockItemCanvas.SetActive(true);
    }

    public void DisableLockItemCanvas()
    {
        lockItemCanvas.SetActive(false);
    }
}
