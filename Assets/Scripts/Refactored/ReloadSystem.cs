using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSystem : MonoBehaviour
{
    private bool magazineInSlot = true;

    public void Detach(GameObject magazine)
    {
        if (magazineInSlot)
        {
            magazine.GetComponent<Rigidbody>().isKinematic = false;
            magazine.transform.SetParent(null);
            ExampleWeapon exmWeapon = GetComponentInParent(typeof(ExampleWeapon)) as ExampleWeapon;
            exmWeapon.NegativeSlide();
        }
    }

    private float DistanceFromMagToPlace(GameObject magazine, GameObject PlaceForMag)
    {
        float dist = Vector3.Distance(PlaceForMag.transform.position, magazine.transform.position);
        return dist;
    }

}
