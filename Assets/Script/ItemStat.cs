using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStat : MonoBehaviour
{
    public enum ItemType
    {
        Weapon,
        Buff
    }

    public ItemType itemType;
    public string itemName;
    public int price;
    public float dropChance;
    [TextArea]
    public string itemDescription;
}
