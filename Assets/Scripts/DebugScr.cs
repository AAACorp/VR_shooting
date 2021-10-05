using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DebugScr : MonoBehaviour
{
    public GameObject hand;
    private Hand scriptHand;
    public Text txtForHs;

    // Start is called before the first frame update
    void Start()
    {
        scriptHand = hand.GetComponent<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        if(scriptHand.currentAttachedObject)
        {
            if(scriptHand.currentAttachedObject.GetComponent<Rifle2>())
            {
                txtForHs.text = "Rifle2 has slide = " + scriptHand.currentAttachedObject.GetComponent<Rifle2>().hasSlide + ". isEmptyMag = " + RifleParams.isEmptyMagazine + ".";
            }

            if (scriptHand.currentAttachedObject.GetComponent<SniperRifle>())
            {
                txtForHs.text = "SniperRifle has slide = " + scriptHand.currentAttachedObject.GetComponent<SniperRifle>().hasSlide + ". isEmptyMag = " + SniperRifleParams.isEmptyMagazine + ".";
            }

            if (scriptHand.currentAttachedObject.GetComponent<AssaultRifle>())
            {
                txtForHs.text = "AssaultRifle has slide = " + scriptHand.currentAttachedObject.GetComponent<AssaultRifle>().hasSlide + ". isEmptyMag = " + AssaultRifleParams.isEmptyMagazine + ".";
            }

            if (scriptHand.currentAttachedObject.GetComponent<AutoShotgun>())
            {
                txtForHs.text = "AutoShotgun isEmptyMag = " + AutoShotgunParams.isEmptyMagazine + ".";
            }

            if (scriptHand.currentAttachedObject.GetComponent<Smg>())
            {
                txtForHs.text = "Smg has slide = " + scriptHand.currentAttachedObject.GetComponent<Smg>().hasSlide + ". isEmptyMag = " + SmgParams.isEmptyMagazine + ".";
            }

            if (scriptHand.currentAttachedObject.GetComponent<Shotgun>())
            {
                txtForHs.text = "Shotgun has slide = " + scriptHand.currentAttachedObject.GetComponent<Shotgun>().hasSlide + ".";
            }

            //if (scriptHand.currentAttachedObject.GetComponent<Glock>())
            //{
            //    txtForHs.text = "Glock has slide = " + scriptHand.currentAttachedObject.GetComponent<Glock>().hasSlide + ". isEmptyMag = " + GlockParams.isEmptyMagazine + ".";
            //}
        }
    }
}
