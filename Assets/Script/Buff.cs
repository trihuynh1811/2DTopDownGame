using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        MoreHealth,
        MoreEnergy,
        MoreArmour,
        MoreSpeed,
        MoreWeaponDmg,
        MoreWeaponRoF,
        MoreWeaponAccuracy,
        MoreWeaponCriticalChance,
        MoreBulletForShotgun,
        MoreCriticalDmgMultiplier,
        CanBounce
    }

    Vector2 originalScale;
    public BuffType buffType;
    public int buffAmount;

    private void Awake()
    {
        originalScale = gameObject.transform.localScale;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            switch (buffType)
    //            {
    //                case BuffType.MoreHealth:
    //                    TopDownPlayerMovement.instance.SetMaxHealth(buffAmount);
    //                    break;
    //                case BuffType.MoreEnergy:
    //                    TopDownPlayerMovement.instance.SetMaxEnergy(buffAmount);
    //                    break;
    //                case BuffType.MoreArmour:
    //                    TopDownPlayerMovement.instance.SetMaxArmour(buffAmount);
    //                    break;
    //                case BuffType.MoreSpeed:
    //                    TopDownPlayerMovement.instance.SetSpeed(buffAmount);
    //                    break;
    //                case BuffType.MoreWeaponDmg:
    //                    TopDownPlayerMovement.instance.SetDmg(buffAmount);
    //                    break;
    //                case BuffType.MoreBulletForShotgun:
    //                    TopDownPlayerMovement.instance.SetNumberOfBullet(buffAmount);
    //                    break;
    //                case BuffType.MoreWeaponRoF:
    //                    TopDownPlayerMovement.instance.SetRoF();
    //                    break;
    //                case BuffType.MoreWeaponAccuracy:
    //                    TopDownPlayerMovement.instance.SetAccuracy();
    //                    break;
    //                case BuffType.MoreWeaponCriticalChance:
    //                    TopDownPlayerMovement.instance.SetCriticalChance(buffAmount);
    //                    break;
    //            }

    //            gameObject.SetActive(false);
    //        }
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.X))
    //        {
    //            switch (buffType)
    //            {
    //                case BuffType.MoreHealth:
    //                    TopDownPlayerMovement.instance.SetMaxHealth(buffAmount);
    //                    break;
    //                case BuffType.MoreEnergy:
    //                    TopDownPlayerMovement.instance.SetMaxEnergy(buffAmount);
    //                    break;
    //                case BuffType.MoreArmour:
    //                    TopDownPlayerMovement.instance.SetMaxArmour(buffAmount);
    //                    break;
    //                case BuffType.MoreSpeed:
    //                    TopDownPlayerMovement.instance.SetSpeed(buffAmount);
    //                    break;
    //                case BuffType.MoreWeaponDmg:
    //                    TopDownPlayerMovement.instance.SetDmg(buffAmount);
    //                    break;
    //                case BuffType.MoreBulletForShotgun:
    //                    TopDownPlayerMovement.instance.SetNumberOfBullet(buffAmount);
    //                    break;
    //                case BuffType.MoreWeaponRoF:
    //                    TopDownPlayerMovement.instance.SetRoF();
    //                    break;
    //                case BuffType.MoreWeaponAccuracy:
    //                    TopDownPlayerMovement.instance.SetAccuracy();
    //                    break;
    //                case BuffType.MoreWeaponCriticalChance:
    //                    TopDownPlayerMovement.instance.SetCriticalChance(buffAmount);
    //                    break;
    //            }
    //            gameObject.SetActive(false);
    //        }
    //    }
    //}

    public IEnumerator BuffPickUpEffect()
    {
        gameObject.transform.localScale = new(originalScale.x + .5f, originalScale.y + .5f);
        yield return new WaitForSeconds(.45f);
        gameObject.transform.localScale = originalScale;
        gameObject.SetActive(false);
    }
}
