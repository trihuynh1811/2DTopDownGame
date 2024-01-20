using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] Transform weaponPos;
    [SerializeField] Transform dropWeaponPos;
    public List<GameObject> weaponList;
    [SerializeField] int maxWeaponCanHold;
    [SerializeField] GameObject currentWeapon;
    public int next = -1;

    private void Awake()
    {
       
    }

    public void PickUpWeapon(GameObject weapon)
    {
        if(weaponList.Count >= maxWeaponCanHold)
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
            weapon.SetActive(true);
        }

    }

    public void DropWeapon()
    {
        if(currentWeapon == null)
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
        if (weaponList.Count <= 0) return;
        if (currentWeapon == null)
        {
            currentWeapon = weaponList[0];
            next = 0;
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
        currentWeapon = weaponList[next];
        currentWeapon.SetActive(true);

    }

    public GameObject GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
