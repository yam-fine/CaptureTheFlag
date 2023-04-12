using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using Utils;

public class GeneralPlayer : MonoBehaviour
{
    protected Flag flag;
    private GameObject flagPos;
    protected bool holdingFlag;
    public bool HoldingFlag
    {
        get { return holdingFlag; }
    }

    protected bool Invincible { get => invincible; set => invincible = value; }

    protected float invincibilityTime = 1f;
    private bool invincible = false;
    private bool canPickup = true;

    protected virtual void Awake()
    {
       
    }

    protected virtual void Start() {
        flag = GameManager.Instance.Flag.GetComponent<Flag>();
        flagPos = UtilFunctions.SearchForObjectInHierarchy(transform, "FlagPos");

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Flag")) {
            CaptureFlag();
        }
        if (other.gameObject.CompareTag("Player")) {
            if (!holdingFlag && !Invincible && flag.PickedUp) { // the other player has the flag
                Debug.Log(gameObject.name + " is taking the flag");
                CaptureFlag();
            }
            else if (holdingFlag && !other.GetComponent<GeneralPlayer>().Invincible) { // we have the flag
                holdingFlag = false;
                StartCoroutine(Invincibility());
            }
        }
    }
    
    protected virtual IEnumerator Invincibility()
    {
        throw new Exception("Override this function");
    }

    protected virtual void CaptureFlag()
    {
            holdingFlag = true;
            flag.CaptureFlag(flagPos.transform);
    }
}
