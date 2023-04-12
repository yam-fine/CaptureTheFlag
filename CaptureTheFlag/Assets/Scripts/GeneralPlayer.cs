using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        throw new NotImplementedException();
    }

    protected virtual void Start() {
        flag = GameManager.Instance.Flag.GetComponent<Flag>();
        flagPos = UtilFunctions.SearchForObjectInHierarchy(transform, "FlagPos").transform;

    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && !holdingFlag) {
            flag.CaptureFlag(flagPos);
            holdingFlag = true;
        }
        else if (collision.gameObject.CompareTag("Player")) {
            holdingFlag = false;
            StartCoroutine(Invincibility());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Flag")) {
            flag.CaptureFlag(flagPos);
            holdingFlag = true;
        }
    }
    
    protected virtual IEnumerator Invincibility()
    {
        throw new Exception("Override this function");
    }
}
