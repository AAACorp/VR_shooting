using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSystem : MonoBehaviour
{
    private bool magazineInSlot = true;
    [SerializeField] private GameObject magazineInWeapon;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Magazine magazineComp))
        {
            ExampleWeapon exmWeapon = GetComponentInParent(typeof(ExampleWeapon)) as ExampleWeapon;
            if(exmWeapon.GetWeaponId() == magazineComp.GetWeaponId())
            {
                AttachMagazine();
            }
        }
    }

    public void Detach()
    {
        if (magazineInWeapon != null)
        {
            magazineInWeapon.GetComponent<Rigidbody>().isKinematic = false;
            magazineInWeapon.transform.SetParent(null);
            ExampleWeapon exmWeapon = GetComponentInParent(typeof(ExampleWeapon)) as ExampleWeapon;
            exmWeapon.NegativeSlide();
            magazineInWeapon = null;
        }
    }

    private void AttachMagazine()
    {

    }

    private float DistanceFromMagToPlace(GameObject magazine, GameObject PlaceForMag)
    {
        float dist = Vector3.Distance(PlaceForMag.transform.position, magazine.transform.position);
        return dist;
    }

}
