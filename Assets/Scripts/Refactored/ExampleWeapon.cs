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
    GameObject Barrel;
    GameObject bullet;//префаб пули
    int damage; //из инспектора на каждом оружии прописываем его урон
    float fireRate; //в миллисекундах, для каждого оружия из инспектора будем прописывать частоту (раз в сколько миллисек может стрелять)
    private int weaponType; //какой тип оружия, из описанных выше
    //bool canShoot;//можем ли стрелять - меняется когда кулдаун проходит. Еще и надо проверить, есть ли магазин, есть ли в нем патроны, перезаряжено ли оружие

    private Interactable interactable;
    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    bool OnPress;

    bool hasSlide;
    GameObject magazine;
    float nextShootTime;
    bool pressedButton;

    private void Update()
    { //OnPress only for Semi-Auto
        //если родитель - рука
        if (buttonGrabPinch.GetStateDown(Pos.inputSource))
        {
            switch (weaponType)
            {
                case 1:
                    if (CheckOfPossibilityShoot())
                    {
                        Shoot();
                        отдача1, флеш1 и звук1;
                        nextShootTime = Time.time + 1f / fireRate;
                    }
                    break;
                case 2:
                    if (CheckOfPossibilityShoot())
                    {
                        Shoot();
                        отдача2, флеш2 и звук2;
                        pressedButton = true;
                    }
                    break;
                case 3:
                    if (CheckOfPossibilityShoot())
                    {
                        Shoot();
                        отдача3, флеш3 и звук3;
                        hasSlide = false;
                        pressedButton = true;
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
            if (magazine)
            {
                if (magazine.GetComponent<Magazine>()/*.ammo > 0*/)
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
                else return false;
            }
            else return false;
        }
        else return false;
    }



}
