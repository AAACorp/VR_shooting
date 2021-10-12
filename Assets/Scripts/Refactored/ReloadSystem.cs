using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSystem : MonoBehaviour
{
    private bool magazineInSlot = true;
    [SerializeField] private Transform connectPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(magazineInSlot == false)
        {
            if (other.TryGetComponent(out Magazine magazineComp))
            {
                ExampleWeapon exmWeapon = GetComponentInParent(typeof(ExampleWeapon)) as ExampleWeapon;
                if (exmWeapon.GetWeaponId() == magazineComp.GetWeaponId())
                {
                    AttachMagazine(connectPoint, other.gameObject);
                }
            }
        }
    }

    public Vector3 GetPointToAttach()
    {
        return connectPoint.position;
    }

    public void SetSlotFalse()
    {
        magazineInSlot = false;
    }

    private void AttachMagazine(Transform pointToAttach, GameObject nonConnectedMagazine)
    {
        nonConnectedMagazine.transform.SetParent(transform.parent);
        nonConnectedMagazine.transform.position = pointToAttach.position;
        nonConnectedMagazine.transform.rotation = pointToAttach.rotation;
        magazineInSlot = true;
    }
}
