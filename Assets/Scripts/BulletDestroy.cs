using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
/*моды
1 - Rifle2
2 - SniperRifle
3 - AssaultRifle
4 - AutoShotgun
5 - SMG
6 - Shotgun
7 - Glock
*/
public class BulletDestroy : MonoBehaviour
{
    public GameObject hitEffect;
    [SerializeField] public int mode;
    [SerializeField] public int totalDamage;//для шотгана - урон от одной пули
    public GameObject emptyPrefab;
    private GameObject tempGO;

    //Debug
    public Text txtForBulColl;
    public Text txtForBulDamage;

    //
    void Start()
    {
        //Debug
        txtForBulColl = GameObject.Find("TextFieldForBulCol").GetComponent<Text>();
        txtForBulDamage = GameObject.Find("TextFieldForBulDamage").GetComponent<Text>();
        //

        tempGO = Instantiate(emptyPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }

    void Update()
    {
        //Debug
        
        //

        //Debug.Log("Distance = " + Vector3.Distance(tempGO.transform.position, gameObject.transform.position));
        if (tempGO)
        {
            switch(mode)
            {
                case 1:
                    if (Vector3.Distance(tempGO.transform.position, gameObject.transform.position) > RifleParams.range)
                    {
                        Destroy(gameObject);
                        Destroy(tempGO);
                    }
                    break;
                case 2:
                    if (Vector3.Distance(tempGO.transform.position, gameObject.transform.position) > SniperRifleParams.range)
                    {
                        Destroy(gameObject);
                        Destroy(tempGO);
                    }
                    break;
                case 3:
                    if (Vector3.Distance(tempGO.transform.position, gameObject.transform.position) > AssaultRifleParams.range)
                    {
                        Destroy(gameObject);
                        Destroy(tempGO);
                    }
                    break;
                case 4: //autoShotgun
                    if (Vector3.Distance(tempGO.transform.position, gameObject.transform.position) > AutoShotgunParams.range)
                    {
                        Destroy(gameObject);
                        Destroy(tempGO);
                    }
                    break;
                case 5:
                    if (Vector3.Distance(tempGO.transform.position, gameObject.transform.position) > SmgParams.range)
                    {
                        Destroy(gameObject);
                        Destroy(tempGO);
                    }
                    break;
                case 6:
                    if (Vector3.Distance(tempGO.transform.position, gameObject.transform.position) > ShotgunParams.range)
                    {
                        Destroy(gameObject);
                        Destroy(tempGO);
                    }
                    break;
                case 7:
                    //if (Vector3.Distance(tempGO.transform.position, gameObject.transform.position) > GlockParams.range)
                    //{
                    //    Destroy(gameObject);
                    //    Destroy(tempGO);
                    //}
                    break;
            }
            /*
            if(time>maxTime)
            {
               Destroy(tempGO);
               Destroy(gameObject);
            }
            */
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Попали в " + collision.collider.name);
        ContactPoint contactPoint = collision.contacts[0];
        GameObject temp = Instantiate(hitEffect, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
        temp.transform.SetParent(contactPoint.otherCollider.transform); // удочеряем
        gameObject.transform.SetParent(contactPoint.otherCollider.transform);

        //Debug
        //txtForBulColl.text = "Попали в " + collision.collider.name;
        //

        if (collision.collider.tag == "Enemy")
        {
            switch(mode)
            {
                case 1: //Rifle2
                    collision.collider.GetComponent<Enemy>().hp -= RifleParams.damage;

                    //Debug
                    txtForBulDamage.text = "Урон от Rifle2 = " + RifleParams.damage;
                    //

                    break;
                case 2: //SniperRifle
                    collision.collider.GetComponent<Enemy>().hp -= SniperRifleParams.damage;

                    //Debug
                    txtForBulDamage.text = "Урон от Rifle2 = " + RifleParams.damage;
                    //

                    break;
                case 3://AssaultRifle
                    collision.collider.GetComponent<Enemy>().hp -= AssaultRifleParams.damage;

                    //Debug
                    txtForBulDamage.text = "Урон от AssaultRifle = " + AssaultRifleParams.damage;
                    //

                    break;
                case 4: //AutoShotgun
                    collision.collider.GetComponent<Enemy>().hp -= totalDamage;

                    //Debug
                    txtForBulDamage.text = "Урон от AutoShotgun = " + totalDamage*10;
                    //

                    break;
                case 5: //Smg
                    collision.collider.GetComponent<Enemy>().hp -= SmgParams.damage;

                    //Debug
                    txtForBulDamage.text = "Урон от Smg = " + SmgParams.damage;
                    //

                    break;
                case 6: //Shotgun
                    collision.collider.GetComponent<Enemy>().hp -= totalDamage;

                    //Debug
                    txtForBulDamage.text = "Урон от Shothun = " + totalDamage*10;
                    //

                    break;
                case 7: //Glock
                    //collision.collider.GetComponent<Enemy>().hp -= GlockParams.damage;

                    ////Debug
                    //txtForBulDamage.text = "Урон от Glock = " + GlockParams.damage;
                    //

                    break;
            }
        }
        Destroy(tempGO);
        Destroy(gameObject);
    }
}
