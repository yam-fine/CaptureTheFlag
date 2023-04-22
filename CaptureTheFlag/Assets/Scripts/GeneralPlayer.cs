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
    [HideInInspector] public GameObject flagPos;
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

    /*public bool HoldingFlag
    {
        get { return flag.; }
    }*/

    //protected bool Invincible { get => invincible; set => invincible = value; }
    //protected NetworkVariable<bool> Invincible { get {  invincible; } set => invincible = value; }

    protected float invincibilityTime = 1f;
    protected bool invincible = false;
    private bool holdingFlag = false;

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
            return;
        }
        //GameMenuManager.Instance.UpdateText($"{flag.hostHoldingFlag},{invincible.Value},{flag.PickedUp},{other.GetComponent<GeneralPlayer>().invincible.Value}");

        if (other.gameObject.CompareTag("Player")) {

            if (!holdingFlag && !invincible && flag.PickedUp) { // the other player has the flag
                //GameMenuManager.Instance.UpdateText(gameObject.name + " is taking the flag");
                if (IsOwner) {
                    Debug.Log(gameObject.name + " is taking the flag");
                    CaptureFlag();
                }
            }
            else if (holdingFlag && !other.GetComponent<GeneralPlayer>().invincible) { // we have the flag
                if (IsOwner) {
                    StartCoroutine(Invincibility());
                    holdingFlag = false;
                }

                    /*flag.transform.parent = other.transform;
                    flag.transform.position = flagPos.transform.position;
                    flag.transform.rotation = flagPos.transform.rotation;*/
                }
            }
    }
    
    protected virtual IEnumerator Invincibility()
    {
        throw new Exception("Override this function");
    }

    protected virtual void CaptureFlag()
    {
        //flag.CaptureFlag(GetComponent<ControlledPlayer>(), flagPos.transform);
        /*if (IsHost) flag.hostHoldingFlag.Value = true;*/
        holdingFlag = true;
        if (IsHost)
            flag.CaptureFlagClientRpc(NetworkObjectId);
        else
            flag.CaptureFlagServerRpc(NetworkObjectId);
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
