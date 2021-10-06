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
    [SerializeField] private int weaponType; //какой тип оружия, из описанных выше
    //bool canShoot;//можем ли стрелять - меняется когда кулдаун проходит. Еще и надо проверить, есть ли магазин, есть ли в нем патроны, перезаряжено ли оружие

    private Interactable interactable;
    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    bool OnPress;

    private bool hasSlide;
    [SerializeField] private GameObject magazine;
    private float nextShootTime;
    private bool pressedButton;

    int _minCorner = -8, _maxCorner = 8;
    int _bulletSpeed = 500;

    private void Update()
    { //OnPress only for Semi-Auto
        if (buttonGrabPinch.GetStateDown(Pos.inputSource))
        {
            if (GetComponentInParent(typeof(Valve.VR.InteractionSystem.Hand)) as Valve.VR.InteractionSystem.Hand)
            {
                switch (weaponType)
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
       
        pressedButton = false;
    }

    private bool CheckOfPossibilityShoot() //тут должна быть проверка на кд, перезаряжено ли оружие, есть ли магазин в оружии, и есть ли вообще патроны в магазине.
                                   //Опять же, здесь нужно знать тип оружия, потому что у какого-то оружия нет кд. А у некоторых(2 и 3) нужно проверять
                                   //отпустили ли кнопку и только потом можно нажать. Что у всех общее? Перезаряжено ли оружие, есть ли магаз и патроны - общее.
                                   //Для начала проверить общее, а потом частное. Пока без дробовиков.
    {
        if (hasSlide)
        {
            if (magazine) //для шотгана как вариант сделать пустышку 
            {
                if (magazine.GetComponent<Magazine>().GetAmmo() > 0)
                {
                    switch (weaponType)
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

    private void Recoil(Transform barrelLocation)
    {
        barrelLocation.localRotation = Quaternion.Euler(barrelLocation.localRotation.x + Random.Range(_minCorner, _maxCorner),
                                                        barrelLocation.localRotation.y + Random.Range(_minCorner, _maxCorner),
                                                        barrelLocation.localRotation.z + Random.Range(_minCorner, _maxCorner));
    }

}
