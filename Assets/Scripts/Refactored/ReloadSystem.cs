using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ReloadSystem : MonoBehaviour
{
    private bool magazineInSlot = true;
    [SerializeField] private Transform connectingPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(magazineInSlot == false)
        {
            if (other.TryGetComponent(out Magazine magazineComp))
            {
                ExampleWeapon exmWeapon = GetComponentInParent(typeof(ExampleWeapon)) as ExampleWeapon;
                if (exmWeapon.GetWeaponId() == magazineComp.GetWeaponId())
                {
                    AttachMagazine(connectingPoint, other.gameObject);
                    exmWeapon.SetMagazine(other.gameObject);
                }
            }
        }
    }

    public Vector3 GetPointToAttach()
    {
        return connectingPoint.position;
    }

    public void SetSlotFalse()
    {
        magazineInSlot = false;
    }

    private void AttachMagazine(Transform pointToAttach, GameObject nonConnectedMagazine)
    {
        nonConnectedMagazine.transform.SetParent(transform.parent);
        nonConnectedMagazine.GetComponent<Rigidbody>().isKinematic = true;
        nonConnectedMagazine.transform.position = pointToAttach.position;
        nonConnectedMagazine.transform.rotation = pointToAttach.rotation;
        magazineInSlot = true;
        nonConnectedMagazine.GetComponent<Magazine>().SetMagazineInWeapon();
        Destroy(nonConnectedMagazine.GetComponent<Throwable>());
        Destroy(nonConnectedMagazine.GetComponent<Interactable>());
    }
}
// Когда нажимаешь на перезарядку, магаз падает, все ок, но потом, когда ты одной рукой держишь автомат, а другой магазин подносишь, то он не сразу коннектится.
// Присоедененное оружие не имеет коллайдера как-будто, но коллайдеры есть
// Надо, что когда магаз в оружии, то удалять тхроваблы