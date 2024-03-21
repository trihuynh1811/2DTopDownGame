using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public GameObject itemCanvas, weaponStatCanvas;
    public GameObject item;
    public int itemIndex { get; set; }
    public string _name;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public float dropChance;
    public int price { get; set; }
    public BoxCollider2D bc2d;




    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        itemCanvas.SetActive(true);
    //        if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            Debug.Log("player bought " + _name);
    //            switch (item.GetComponent<ItemStat>().itemType)
    //            {
    //                case ItemStat.ItemType.Buff:
    //                    Buff buff = collision.gameObject.GetComponent<Buff>();
    //                    switch (buff.buffType)
    //                    {
    //                        case Buff.BuffType.MoreHealth:
    //                            TopDownPlayerMovement.instance.SetMaxHealth(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreEnergy:
    //                            TopDownPlayerMovement.instance.SetMaxEnergy(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreArmour:
    //                            TopDownPlayerMovement.instance.SetMaxArmour(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreSpeed:
    //                            TopDownPlayerMovement.instance.SetSpeed(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreWeaponDmg:
    //                            TopDownPlayerMovement.instance.SetDmg(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreBulletForShotgun:
    //                            TopDownPlayerMovement.instance.SetNumberOfBullet(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreWeaponRoF:
    //                            TopDownPlayerMovement.instance.SetRoF();
    //                            break;
    //                        case Buff.BuffType.MoreWeaponAccuracy:
    //                            TopDownPlayerMovement.instance.SetAccuracy();
    //                            break;
    //                        case Buff.BuffType.MoreWeaponCriticalChance:
    //                            TopDownPlayerMovement.instance.SetCriticalChance(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreCriticalDmgMultiplier:
    //                            TopDownPlayerMovement.instance.SetCriticalDmgMultiplier();
    //                            break;
    //                        case Buff.BuffType.CanBounce:
    //                            TopDownPlayerMovement.instance.SetCanBounce(true);
    //                            break;
    //                        case Buff.BuffType.AllowHealthRegen:
    //                            TopDownPlayerMovement.instance.allowHealthRegen = true;
    //                            break;
    //                    }
    //                    collision.gameObject.transform.parent = TopDownPlayerMovement.instance.buffPos;
    //                    collision.gameObject.transform.localPosition = Vector3.zero;
    //                    StartCoroutine(collision.gameObject.GetComponent<Buff>().BuffPickUpEffect());
    //                    if (TopDownPlayerMovement.instance.pickUp.GetCurrentWeapon() != null)
    //                    {
    //                        TopDownPlayerMovement.instance.pickUp.GetCurrentWeapon().GetComponent<Gun>().ApplyBuff();
    //                    }
    //                    break;
    //            }
    //            TopDownPlayerMovement.instance.coin -= price;
    //            TopDownPlayerMovement.instance.UpdateUi();
    //            bc2d.enabled = false;
    //        }
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        itemCanvas.SetActive(true);
    //        if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            Debug.Log("player bought " + _name);
    //            switch (item.GetComponent<ItemStat>().itemType)
    //            {
    //                case ItemStat.ItemType.Buff:
    //                    Buff buff = item.GetComponent<Buff>();
    //                    switch (buff.buffType)
    //                    {
    //                        case Buff.BuffType.MoreHealth:
    //                            TopDownPlayerMovement.instance.SetMaxHealth(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreEnergy:
    //                            TopDownPlayerMovement.instance.SetMaxEnergy(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreArmour:
    //                            TopDownPlayerMovement.instance.SetMaxArmour(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreSpeed:
    //                            TopDownPlayerMovement.instance.SetSpeed(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreWeaponDmg:
    //                            TopDownPlayerMovement.instance.SetDmg(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreBulletForShotgun:
    //                            TopDownPlayerMovement.instance.SetNumberOfBullet(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreWeaponRoF:
    //                            TopDownPlayerMovement.instance.SetRoF();
    //                            break;
    //                        case Buff.BuffType.MoreWeaponAccuracy:
    //                            TopDownPlayerMovement.instance.SetAccuracy();
    //                            break;
    //                        case Buff.BuffType.MoreWeaponCriticalChance:
    //                            TopDownPlayerMovement.instance.SetCriticalChance(buff.buffAmount);
    //                            break;
    //                        case Buff.BuffType.MoreCriticalDmgMultiplier:
    //                            TopDownPlayerMovement.instance.SetCriticalDmgMultiplier();
    //                            break;
    //                        case Buff.BuffType.CanBounce:
    //                            TopDownPlayerMovement.instance.SetCanBounce(true);
    //                            break;
    //                        case Buff.BuffType.AllowHealthRegen:
    //                            TopDownPlayerMovement.instance.allowHealthRegen = true;
    //                            break;
    //                    }
    //                    item.transform.parent = TopDownPlayerMovement.instance.buffPos;
    //                    item.transform.localPosition = Vector3.zero;
    //                    StartCoroutine(item.GetComponent<Buff>().BuffPickUpEffect());
    //                    if (TopDownPlayerMovement.instance.pickUp.GetCurrentWeapon() != null)
    //                    {
    //                        TopDownPlayerMovement.instance.pickUp.GetCurrentWeapon().GetComponent<Gun>().ApplyBuff();
    //                    }
    //                    break;
    //            }
    //            TopDownPlayerMovement.instance.coin -= price;
    //            TopDownPlayerMovement.instance.UpdateUi();
    //            bc2d.enabled = false;
    //        }
    //    }
    //}


}
