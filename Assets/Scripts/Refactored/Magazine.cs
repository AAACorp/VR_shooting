using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private int _ammo = 0;
    [SerializeField] private int _weaponId = 0; //идентификатор оружия, чтобы знать к кому подключать 
    //private bool isCanToAttach; сделать из этого функцию
    private bool magazineInWeapon = false;
    private GameObject ReloadCollider;

    private void Start()
    {
        if (transform.parent.TryGetComponent(out ExampleWeapon exmWeapon)) 
        {
            magazineInWeapon = true;

            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).TryGetComponent(out ReloadSystem reloadSystem))
                {
                    ReloadCollider = transform.parent.GetChild(i).gameObject;
                }
                else ReloadCollider = null;
            }
        }
    }

    private void Update()
    {
        if(magazineInWeapon)
        {
            if (transform.parent)
            {
                if (DistanceFromMagToPlace(transform, ReloadCollider.GetComponent<ReloadSystem>().GetPointToAttach()) >= 0.8f)
                {
                    magazineInWeapon = false;
                    ReloadCollider.GetComponent<ReloadSystem>().SetSlotFalse();
                    ReloadCollider = null;
                }
            }
                
        }
        else if(_ammo == 0)
        {
            Destroy(gameObject, 4f);
        }
    }

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

    public void Detach()
    {
        if (magazineInWeapon)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            ExampleWeapon exmWeapon = GetComponentInParent(typeof(ExampleWeapon)) as ExampleWeapon;
            exmWeapon.NegativeSlide();
            transform.SetParent(null);
        }
    }

    private float DistanceFromMagToPlace(Transform magazine, Vector3 PlaceForMag)
    {
        float _dist = Vector3.Distance(PlaceForMag, magazine.position);
        return _dist;
    }

}
