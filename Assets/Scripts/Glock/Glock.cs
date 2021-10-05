using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class Glock : MonoBehaviour
{
    public GameObject hand;
    Hand scriptHand;
    float force = 155;
    public GameObject magazine;
    public GameObject casingPrefab;
    public GameObject PlaceForMagazine;
    public GameObject ReloadCollider;
    public GameObject centerPistol;
    ReloadColliderForGlockMagazine colliderForPistol;
    //public AudioSource source;
    //public AudioClip fireSound;
    //public AudioClip noAmmoSound;
    //public AudioClip detachMagazineSound; //58417577

    private bool hasSlide = true;// сделать

    [SerializeField] private Animator gunAnimator;// анимация

    [Header("Prefab Refrences")]
    public GameObject bulletPrefab; //префаб патрона
    public GameObject muzzleFlashPrefab; //префаб дульной вспышки
    public GameObject emptyPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Transform barrelLocation; //крайняя точка дула
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] public float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] public float shotPower = 600f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    public float recoilForce = 100f;

    private Interactable interactable;
    public SteamVR_Behaviour_Pose Pos = null; // Хранит правый контроллер - поле назначается из редактора Unity
    private SteamVR_Action_Boolean buttonGrabPinch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Action_Boolean buttonGrabGrip = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    private bool OnPress;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        OnPress = false;

        scriptHand = hand.GetComponent<Hand>();

        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null) //тоже анимация
            gunAnimator = GetComponentInChildren<Animator>();

        colliderForPistol = ReloadCollider.GetComponent<ReloadColliderForGlockMagazine>();

        //bulletsList = new List<BulletInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scriptHand.currentAttachedObject != null)
        {
            if (scriptHand.currentAttachedObject.GetComponent<Glock>())
            {
                if (magazine == null)
                {
                    if (scriptHand.currentAttachedObject.transform.Find("magazine"))
                    {
                        magazine = scriptHand.currentAttachedObject.transform.Find("magazine").gameObject;
                    }
                }

                if (magazine == null || hasSlide == false)
                {
                    gunAnimator.enabled = false;
                }
                else if (magazine.GetComponent<GlockMagazine>().currentAmmo <= 0 || hasSlide == false)
                {
                    gunAnimator.enabled = false;
                }
                else gunAnimator.enabled = true;

                if (colliderForPistol.hasSlide != hasSlide) hasSlide = colliderForPistol.hasSlide;

                if /*(Input.GetKeyDown("space")*/(buttonGrabPinch.GetStateDown(Pos.inputSource) && OnPress == false) //изменить кнопку на кнопку на контроллере
                {
                    if(magazine.GetComponent<GlockMagazine>().currentAmmo > 0 && GlockParams.isEmptyMagazine == false && hasSlide)
                    {
                        gunAnimator.SetTrigger("Fire");
                        OnPress = true;
                    }
                    //else source.PlayOneShot(noAmmoSound); включить потом
                }
                Reload();
                if (magazine != null) ToggleMagMode();
            }
        }
        if (buttonGrabPinch.GetStateUp(Pos.inputSource)) { OnPress = false; } //Включить
        if (buttonGrabGrip.GetStateUp(Pos.inputSource)) { OnPress = false; }
    }

    void Shoot()
    {
            //source.PlayOneShot(fireSound); включить потом

            if (muzzleFlashPrefab)
            {
                //создание и удаление мазл флеша
                GameObject tempFlash;
                tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
                Destroy(tempFlash, destroyTimer);
            }

            GameObject tempBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation * Quaternion.Euler(180, -90, 1));
            tempBullet.GetComponent<Rigidbody>().AddForce(barrelLocation.right * shotPower);
            tempBullet.GetComponent<BulletDestroy>().mode = 7;

            gameObject.GetComponent<Rigidbody>().AddForce(barrelLocation.up * recoilForce); //вроде работает

            magazine.GetComponent<GlockMagazine>().currentAmmo--;        
    }
    void Reload()
    {
        if (buttonGrabGrip.GetStateDown(Pos.inputSource) && OnPress == false)
        {
            if (GlockParams.isEmptyMagazine == false)
            {
                //source.PlayOneShot(detachMagazineSound); включить потом
                Detach();
                OnPress = true;
            }
        }
    }

    public void Slide()
    {
        hasSlide = true;
        //audio kakoe nibud
    }

    float DistanceFromMagToPlace(GameObject magazine, GameObject PlaceForMag)
    {
        float dist = Vector3.Distance(PlaceForMag.transform.position, magazine.transform.position);
        return dist;
    }

    void ToggleMagMode()
    {
        if (magazine.GetComponent<GlockMagazine>())
        {
            if (magazine.GetComponent<GlockMagazine>().mode == 1)
            {
                if (DistanceFromMagToPlace(magazine, PlaceForMagazine) >= 0.2f)
                {
                    magazine.GetComponent<GlockMagazine>().mode = 2;
                    magazine = null;
                }
            }
        }
    }

    void Detach()
    {
        if (magazine)
        {
            magazine.GetComponent<Rigidbody>().isKinematic = false;
            magazine.transform.SetParent(null);
            GlockParams.isEmptyMagazine = true;
            hasSlide = false;
        }
    }

    void CasingRelease()
    {
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
        Destroy(tempCasing, destroyTimer);
    }

}
