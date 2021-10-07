using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ExampleWeapon : MonoBehaviour
{
    //delete this (to test)
    [SerializeField] private Transform BarrelLocation;
    [SerializeField] private GameObject bulletPrefab;//префаб пули
    [SerializeField] private int _damage; //из инспектора на каждом оружии прописываем его урон
    [SerializeField] private float fireRate; //в миллисекундах, для каждого оружия из инспектора будем прописывать частоту (раз в сколько миллисек может стрелять)
    [SerializeField] private int _weaponId; //какой тип оружия, из описанных выше

    private Interactable interactable;
    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    bool OnPress;

    private bool hasSlide;
    [SerializeField] private GameObject magazine;
    private float nextShootTime;
    private bool pressedButton;

    int _minCorner = -8, _maxCorner = 8; //для отдачи
    int _bulletSpeed = 500;

    private void Update()
    { //OnPress only for Semi-Auto
        if (buttonGrabPinch.GetStateDown(Pos.inputSource))
        {
            if (GetComponentInParent(typeof(Valve.VR.InteractionSystem.Hand)) as Valve.VR.InteractionSystem.Hand)
            {
                switch (_weaponId)
                {
                    case 1:
                        if (CheckOfPossibilityShoot())
                        {
                            Shoot(bulletPrefab);
                            nextShootTime = Time.time + 1f / fireRate;
                        }
                        break;
                    case 2:
                        if (CheckOfPossibilityShoot())
                        {
                            Shoot(bulletPrefab);
                            pressedButton = true;
                        }
                        break;
                    case 3:
                        if (CheckOfPossibilityShoot())
                        {
                            Shoot(bulletPrefab);
                            hasSlide = false;
                            pressedButton = true;
                        }
                        break;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            
        }

        pressedButton = false;
    }

    private bool CheckOfPossibilityShoot() //пока без дробовиков
    {
        if (hasSlide)
        {
            if (magazine) //для шотгана как вариант сделать пустышку 
            {
                if (magazine.GetComponent<Magazine>().GetAmmo() > 0)
                {
                    switch (_weaponId)
                    {
                        case 1: //auto
                            if (Time.time > nextShootTime) return true;
                            else return false;
                        case 2: //semi-auto
                            if (pressedButton == false) return true;
                            else return false;
                        case 3: //need slide every shoot 
                            if (pressedButton == false) return true;
                            else return false;

                    }
                    return false;
                }
                else
                {
                    //emptyMagazineSound();
                    return false;
                }
            }
            else return false;
        }
        else return false;
    }

    private void Shoot(GameObject bullet)
    {
        GameObject tempBullet = Instantiate(bullet, BarrelLocation.position, BarrelLocation.rotation * Quaternion.identity);
        bullet.GetComponent<BulletNew>().SetDamage(_damage);
        tempBullet.GetComponent<Rigidbody>().AddForce(-BarrelLocation.right * _bulletSpeed);
        Recoil(BarrelLocation);
        magazine.GetComponent<Magazine>().DecreaseAmmo();
        //flash, sound
    }

    public void Slide()
    {
        hasSlide = true;
    }

    public void NegativeSlide()
    {
        hasSlide = false;
    }

    public int GetWeaponId()
    {
        return _weaponId;
    }

    private void Recoil(Transform barrelLocation)
    {
        barrelLocation.localRotation = Quaternion.Euler(barrelLocation.localRotation.x + Random.Range(_minCorner, _maxCorner),
                                                        barrelLocation.localRotation.y + Random.Range(_minCorner, _maxCorner),
                                                        barrelLocation.localRotation.z + Random.Range(_minCorner, _maxCorner));
    }

}
