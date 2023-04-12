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
    protected float invincibilityTime = 2f;
    protected bool invincible = false;

    protected virtual void Awake()
    {
       
    }

    protected virtual void Start() {
        flag = GameManager.Instance.Flag.GetComponent<Flag>();
        flagPos = UtilFunctions.SearchForObjectInHierarchy(transform, "FlagPos");

    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && !holdingFlag && !invincible && flag.PickedUp) {
            Debug.Log("HOLD");
            CaptureFlag();
        }
        else if (collision.gameObject.CompareTag("Player") && holdingFlag) {
            holdingFlag = false;
            StartCoroutine(Invincibility());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Flag")) {
            CaptureFlag();
        }
    }
    
    protected virtual IEnumerator Invincibility()
    {
        throw new Exception("Override this function");
    }

    protected virtual void CaptureFlag()
    {
        flag.CaptureFlag(flagPos.transform);
        holdingFlag = true;
    }
}
