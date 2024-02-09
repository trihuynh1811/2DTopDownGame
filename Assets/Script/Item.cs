using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public GameObject itemCanvas, weaponStatCanvas;
    public string _name;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public float dropChance;
    public int price;

    private void Awake()
    {
        itemCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player step in");
            itemCanvas.SetActive(true);
            switch (collision.gameObject.GetComponent<ItemStat>().itemType)
            {
                case ItemStat.ItemType.Weapon:
                    weaponStatCanvas.SetActive(true);
                    break;

                case ItemStat.ItemType.Buff:
                    weaponStatCanvas.SetActive(false);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player step out");
            itemCanvas.SetActive(false);
            switch (collision.gameObject.GetComponent<ItemStat>().itemType)
            {
                case ItemStat.ItemType.Weapon:
                    weaponStatCanvas.SetActive(false);
                    break;

                case ItemStat.ItemType.Buff:
                    weaponStatCanvas.SetActive(false);
                    break;
            }
        }
    }
}
