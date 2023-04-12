using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using Utils;

public class GeneralPlayer : MonoBehaviour
{
    private Flag flag;
    private Transform flagPos;
    private bool holdingFlag;
    public bool HoldingFlag
    {
        get { return holdingFlag; }
    }
    [SerializeField] protected float invincibilityTime = 0.5f;

    protected virtual void Awake()
    {
       
    }

    protected virtual void Start() {
        flag = GameManager.Instance.Flag.GetComponent<Flag>();
        flagPos = UtilFunctions.SearchForObjectInHierarchy(transform, "FlagPos").transform;

    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && !holdingFlag) {
            Debug.Log("HOLD");
            CaptureFlag();
        }
        else if (collision.gameObject.CompareTag("Player")) {
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
        flag.CaptureFlag(flagPos);
        holdingFlag = true;
    }
}
