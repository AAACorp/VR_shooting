// Мануал по замене
// В самом оружии кидаем скрипт этот и удаляем предыдущий. В анимации меняем патрон. В слайдере заменяем Слайд на новый (с этого скрипта). В релоад коллайдер
// релоад систем пихаем, старое удаляем. Магазин: берем меш, пихаем в пустой объект, у него коллайдер, ригидбади и скрипт магазин, айдишники меняем. Мейн тег на 
// веапон меняем
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ExampleWeapon : MonoBehaviour
{
   // [SerializeField] private GameObject _bulletPrefab;//префаб пули
    [SerializeField] private GameObject _muzzleFlashPrefab; //дульная вспышка
    [SerializeField] private GameObject _casingPrefab;

    [SerializeField] private Transform _barrelLocation;
    [SerializeField] private Transform _casingExitLocation;

    [SerializeField] private Animator _gunAnimator;

    [SerializeField] private float _fireRate; //в миллисекундах, для каждого оружия из инспектора будем прописывать частоту (раз в сколько миллисек может стрелять)
    [SerializeField] private int _weaponId; //какое оружие (автомат, смг или что-то другое)
    [SerializeField] private int _weaponType; //какой тип оружия (автоматическое, полуавтоматическое, дробаш)
    [SerializeField] private int _damage; //из инспектора на каждом оружии прописываем его урон

    private GameObject _magazine;

    private float _ejectPower = 150f;
    private float _nextShootTime;

    private int _minCorner = 1, _maxCorner = 8; //для отдачи
    private int _bulletSpeed = 500;

    private bool _hasSlide = true;    
    private bool _pressedButton;

    private Interactable interactable;
    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");  

    private void Start()
    {
        //поиск по детям с компонентом магазин в magazine
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Magazine _))
                _magazine = transform.GetChild(i).gameObject;
        }
        _gunAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if ((buttonGrabPinch.GetState(Pos.inputSource) || Input.GetKey(KeyCode.Space)) && _pressedButton == false)
        {
            if(transform.parent)
            {
                if (transform.parent.TryGetComponent(out Valve.VR.InteractionSystem.Hand _))
                {
                    if (CheckOfPossibilityShoot())
                    {
                        _gunAnimator.SetTrigger("Fire");
                        switch (_weaponType)
                        {
                            case 1:
                                if(CheckOfPossibilityShoot())
                                {
                                    Debug.Log("Shooted (Exm Weapon 73)");
                                    _nextShootTime = Time.time + 1f / _fireRate;
                                    transform.Rotate(0f, 3f, 0f, Space.Self);
                                    //transform.parent.transform.Rotate(10f, 0f, 0f, Space.Self); // не чекнул вроде
                                }  
                                break;
                            case 2:
                                _pressedButton = true;
                                break;
                            case 3:
                                Debug.Log("has slide = " + _hasSlide);
                                Debug.Log("Shooted");
                                _hasSlide = false;
                                Debug.Log("has slide = " + _hasSlide);
                                _pressedButton = true;
                                break;
                        }
                    }
                }
                
            } 
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(_hasSlide);
            Debug.Log(_magazine);
            Debug.Log(_weaponId);
            Debug.Log("Time.time = " + Time.time + " ; NextShoot = " + _nextShootTime);
        }

        if (buttonGrabGrip.GetStateDown(Pos.inputSource) || Input.GetKeyDown(KeyCode.R)) // вдруг уже нету магаза
        {
            if (transform.parent)
            {
                if (transform.parent.TryGetComponent(out Valve.VR.InteractionSystem.Hand _))
                {
                    if (_magazine)
                    {
                        _magazine.GetComponent<Magazine>().Detach();
                    }
                }
            }
        }





        if (Input.GetKeyUp(KeyCode.Space)) _pressedButton = false;
        if (buttonGrabPinch.GetStateUp(Pos.inputSource)) _pressedButton = false;
    }

    private bool CheckOfPossibilityShoot() //пока без дробовиков
    {
        if (_hasSlide)
        {
            if (_magazine) //для шотгана как вариант сделать пустышку 
            {
                if (_magazine.GetComponent<Magazine>().GetAmmo() > 0)
                {
                    switch (_weaponType)
                    {
                        case 1: //auto
                            if (Time.time > _nextShootTime) return true;
                            else return false;
                        case 2: //semi-auto
                            if (_pressedButton == false) return true;
                            else return false;
                        case 3: //need slide every shoot 
                            if (_pressedButton == false) return true;
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
        GameObject tempBullet = Instantiate(bullet, _barrelLocation.position, _barrelLocation.rotation * Quaternion.identity);
        tempBullet.GetComponent<BulletNew>().SetDamage(_damage);
        tempBullet.GetComponent<Rigidbody>().AddForce(_barrelLocation.right * _bulletSpeed);
        //Recoil(transform);
        _magazine.GetComponent<Magazine>().DecreaseAmmo();

        if (_muzzleFlashPrefab)
        {
            GameObject _tempFlash;
            _tempFlash = Instantiate(_muzzleFlashPrefab, _barrelLocation.position, _barrelLocation.rotation);
            Destroy(_tempFlash, 1f);
        }
        // sound
    }

    private void CasingRelease()
    {
        GameObject _tempCasing;
        _tempCasing = Instantiate(_casingPrefab, _casingExitLocation.position, _casingExitLocation.rotation) as GameObject;
        _tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(_ejectPower * 0.7f, _ejectPower), (_casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f), 1f);
        _tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
        Destroy(_tempCasing, 10f);
    }

    public void ClearMagazineSlot()
    {
        _magazine = null;
    }

    public void SetMagazine(GameObject mag)
    {
        _magazine = mag;
    }

    public void Slide()
    {
        _hasSlide = true;
    }

    public void NegativeSlide()
    {
        _hasSlide = false;
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
