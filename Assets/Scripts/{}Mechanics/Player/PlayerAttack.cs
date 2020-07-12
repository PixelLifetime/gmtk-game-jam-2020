using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponType weaponType;
    private Weapon[] weapons;

    private void Start()
    {
        weapons = new Weapon[] { 
        GetComponentInChildren<Sword>(),
        GetComponentInChildren<Spear>(),
        GetComponentInChildren<Bow>()
        };
    }
    private void OnAttack()
    {
        weapons[(int)weaponType].Attack();
    }
    private void OnSecondaryAttack()
    {
        weapons[(int)weaponType].SecondaryAttack();
    }
}

public enum WeaponType
{
    Sword,
    Spear,
    Bow
}
