using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LootBox : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip idle, open;
    public float minDropChance, maxDropChance;
    public List<GameObject> guns;
    public List<ItemStat> itemStats;
    public int numberOfFlashTime;
    public int Rand;
    GameObject currentGun;
    float totalDropChances = 0f;

    private void Awake()
    {
        currentGun = guns[0];
        itemStats = itemStats.OrderBy(x => x.dropChance).ToList();
        minDropChance = itemStats.Min(x => x.dropChance);
        maxDropChance = itemStats.Max(x => x.dropChance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CrateOpen()
    {
        animator.Play(open.name);

        #region old loot logic
        //for(int i = 0; i < numberOfFlashTime; i++)
        //{
        //    for(int j = 0; j < guns.Count; j++)
        //    {
        //        currentGun.SetActive(false);
        //        guns[j].SetActive(true);

        //    }
        //}
        //int index = Random.Range(0, guns.Count);
        //while (index == Rand)
        //{
        //    index = Random.Range(0, guns.Count);
        //    guns[index].SetActive(true);
        //}
        //Rand = index;
        //for (int i = 0; i < guns.Count; i++)
        //{
        //    guns[i].SetActive(i == index);
        //}
        #endregion

        itemStats.ForEach(item => item.gameObject.SetActive(false));
        float randomChance = Random.Range(minDropChance, maxDropChance + .1f);
        Debug.Log(randomChance);
        List<ItemStat> iStat = itemStats.FindAll(stat => stat.dropChance < randomChance).ToList();
        if(iStat.Count > 0)
        {
            int randomIndex = Random.Range(0, iStat.Count);
            iStat[randomIndex].gameObject.SetActive(true);
        }
        else
        {
            iStat = itemStats.FindAll(x => x.dropChance.Equals(maxDropChance)).ToList();
            int randomIndex = Random.Range(0, iStat.Count);
            iStat[randomIndex].gameObject.SetActive(true);
        }

    }




    public void CrateClose()
    {
        animator.Rebind();
    }
}
