using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using Utils;
using Unity.Netcode;

public class GeneralPlayer : NetworkBehaviour
{
    protected Flag flag;
    private GameObject flagPos;
    protected bool holdingFlag;
    GameObject goal;
    private GameMenuManager.Side scoreSide;
    public GameMenuManager.Side ScoreSide
    {
        get => scoreSide;
        set => scoreSide = value;
    }

    protected Vector3 startPos;
    //[SerializeField] private GameObject goal;
    [SerializeField] private AudioSource goalAudio;

    public GameObject Goal
    {
        get { return goal; }
    }

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
        startPos = transform.position;
    }

    protected virtual void Start() {
        flag = GameManager.Instance.Flag.GetComponent<Flag>();
        flagPos = UtilFunctions.SearchForObjectInHierarchy(transform, "FlagPos");
        if (IsHost)
            goal = GameManager.Instance.goalOwner;
        else
            goal = GameManager.Instance.goalClient;
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

    public virtual void ScoreFlag()
    {
        StartCoroutine(Invincibility());
        if (holdingFlag) {
            GameManager.Instance.UpdateScore(scoreSide);
            goalAudio.Play();
        }
        holdingFlag = false;
    }
}
