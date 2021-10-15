using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ExampleWeapon : MonoBehaviour
{
    [SerializeField] private Transform BarrelLocation;
    [SerializeField] private GameObject bulletPrefab;//префаб пули
    [SerializeField] private int _damage; //из инспектора на каждом оружии прописываем его урон
    [SerializeField] private float fireRate; //в миллисекундах, для каждого оружия из инспектора будем прописывать частоту (раз в сколько миллисек может стрелять)
    [SerializeField] private int _weaponId; //какой тип оружия, из описанных выше
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private Transform casingExitLocation;
    private float _ejectPower = 150f;

    private Interactable interactable;
    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    bool OnPress;

    [SerializeField] private Animator gunAnimator;
    private bool hasSlide = true;
    private GameObject magazine;
    private float _nextShootTime;
    private bool pressedButton;

    int _minCorner = 1, _maxCorner = 8; //для отдачи
    int _bulletSpeed = 500;

    private void Start()
    {
        //поиск по детям с компонентом магазин в magazine
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Magazine _))
                magazine = transform.GetChild(i).gameObject;
        }
        gunAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (buttonGrabPinch.GetStateDown(Pos.inputSource) || Input.GetKey(KeyCode.Space))
        {
            if(transform.parent)
            {
                if (transform.parent.TryGetComponent(out Valve.VR.InteractionSystem.Hand _))
                {
                    switch (_weaponId)
                    {
                        case 1:
                            if (CheckOfPossibilityShoot())
                            {
                                gunAnimator.SetTrigger("Fire");
                                _nextShootTime = Time.time + 1f / fireRate;
                            }
                            break;
                        case 2:
                            if (CheckOfPossibilityShoot())
                            {
                                gunAnimator.SetTrigger("Fire");
                                pressedButton = true;
                            }
                            break;
                        case 3:
                            if (CheckOfPossibilityShoot())
                            {
                                gunAnimator.SetTrigger("Fire");
                                hasSlide = false;
                                pressedButton = true;
                            }
                            break;
                    }
                }
            } 
        }

        if(buttonGrabGrip.GetStateDown(Pos.inputSource) || Input.GetKeyDown(KeyCode.R)) // вдруг уже нету магаза
        {
            if(magazine)
            {
                magazine.GetComponent<Magazine>().Detach();
            }
        }

        if (Input.GetKeyDown(KeyCode.T)) Debug.Log(hasSlide);

        //if (Input.GetKeyUp(KeyCode.Space)) pressedButton = false;
        if (buttonGrabPinch.GetStateUp(Pos.inputSource)) pressedButton = false;
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
                            if (Time.time > _nextShootTime) return true;
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
        tempBullet.GetComponent<BulletNew>().SetDamage(_damage);
        tempBullet.GetComponent<Rigidbody>().AddForce(BarrelLocation.right * _bulletSpeed);
        Recoil(transform);
        magazine.GetComponent<Magazine>().DecreaseAmmo();
        //flash, sound
    }

    private void CasingRelease()
    {
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(_ejectPower * 0.7f, _ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
        Destroy(tempCasing, 5);
    }

    public void ClearMagazineSlot()
    {
        magazine = null;
    }

    public void SetMagazine(GameObject mag)
    {
        magazine = mag;
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
        barrelLocation.localRotation = Quaternion.Euler(barrelLocation.localRotation.x,
                                                        barrelLocation.localRotation.y + Random.Range(_minCorner, _maxCorner),
                                                        barrelLocation.localRotation.z + Random.Range(_minCorner, _maxCorner));
    }

}
