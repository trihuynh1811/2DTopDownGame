using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    [SerializeField] Transform weaponPos;
    [SerializeField] Transform dropWeaponPos;
    public List<GameObject> weaponList;
    [SerializeField] int maxWeaponCanHold;
    [SerializeField] GameObject currentWeapon;
    [SerializeField] Image gunImgBg, gunImg;
    [SerializeField] Text energyText;
    public int next = -1;

    private void Awake()
    {
        gunImgBg.gameObject.SetActive(false);
    }

    public void PickUpWeapon(GameObject weapon)
    {
        if (weaponList.Count >= maxWeaponCanHold)
        {
            DropWeapon();
        }
        weapon.GetComponent<Gun>().enabled = true;
        weapon.transform.parent = weaponPos;
        weapon.transform.position = weaponPos.position;
        weapon.transform.rotation = weaponPos.rotation;
        weapon.layer = 0;
        weapon.SetActive(false);
        weaponList.Add(weapon);

        if (currentWeapon == null)
        {
            currentWeapon = weapon;
            next = 0;
            gunImg.sprite = currentWeapon.GetComponent<Gun>().gunSprite.sprite;
            energyText.text = currentWeapon.GetComponent<Gun>().energyConsume.ToString();
            weapon.SetActive(true);
            gunImgBg.gameObject.SetActive(true);
        }

    }

    public void DropWeapon()
    {
        if (currentWeapon == null)
        {
            return;
        }

        if (next != -1)
        {
            currentWeapon = weaponList[next];

            // Set the weapon's position to the drop position
            currentWeapon.GetComponent<Gun>().enabled = false;
            currentWeapon.transform.parent = dropWeaponPos;
            currentWeapon.transform.position = dropWeaponPos.position;
            currentWeapon.transform.rotation = dropWeaponPos.rotation;
            currentWeapon.transform.parent = null;
            currentWeapon.layer = 9;
            weaponList.RemoveAt(next);
            currentWeapon = null;

        }
    }

    public void SwitchWeapon()
    {
        if (weaponList.Count <= 0)
        {
            gunImgBg.gameObject.SetActive(false);
            return;
        }
        if (currentWeapon == null)
        {
            currentWeapon = weaponList[0];
            next = 0;
            gunImg.sprite = currentWeapon.GetComponent<Gun>().gunSprite.sprite;
            energyText.text = currentWeapon.GetComponent<Gun>().energyConsume.ToString();
            currentWeapon.SetActive(true);
            return;
        }
        next = -1;
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i] == currentWeapon)
            {
                next = i + 1;
                break;
            }
        }
        if (next == -1) next = 0;
        next %= weaponList.Count;
        currentWeapon.SetActive(false);
        currentWeapon.GetComponent<Gun>().ResetCurrentFireRate();
        currentWeapon = weaponList[next];
        gunImg.sprite = currentWeapon.GetComponent<Gun>().gunSprite.sprite;
        energyText.text = currentWeapon.GetComponent<Gun>().energyConsume.ToString();
        currentWeapon.SetActive(true);

    }

    public GameObject GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
