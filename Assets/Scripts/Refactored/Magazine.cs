using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private int _ammo = 0;
    [SerializeField] private int _weaponId = 0; //идентификатор оружия, чтобы знать к кому подключать 
    private bool isCanToAttach;
    public int GetAmmo()
    {
        return _ammo;
    }

    public int GetWeaponId()
    {
        return _weaponId;
    }

    public void DecreaseAmmo()
    {
        _ammo--;
    }

    float DistanceFromMagToPlace(GameObject magazine, GameObject PlaceForMag)
    {
        float _dist = Vector3.Distance(PlaceForMag.transform.position, magazine.transform.position);
        return _dist;
    }

}
