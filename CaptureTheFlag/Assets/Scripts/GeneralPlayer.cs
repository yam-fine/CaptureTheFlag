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

    protected float invincibilityTime = 1f;
    protected bool invincible = false;
    [HideInInspector] public bool holdingFlag = false;
    //protected NetworkVariable<bool> invincible = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //private NetworkVariable<bool> holdingFlag = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /*protected virtual void OnNetworkStart()
    {
        startPos = transform.position;
    }*/
    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
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

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && IsHost && IsOwner) {
            Debug.Log("hello " + NetworkObjectId.ToString());
            ChangeItServerRpc();
        }
        
        
        /*if (other.CompareTag("Flag")) {
            if (!IsHost) flag.InitPickupServerRpc();
            else flag.InitPickupClientRpc();
            CaptureFlag();
            return;
        }
        //GameMenuManager.Instance.UpdateText($"{flag.hostHoldingFlag},{invincible.Value},{flag.PickedUp},{other.GetComponent<GeneralPlayer>().invincible.Value}");

        if (other.gameObject.CompareTag("Player")) {
            if (!holdingFlag && !invincible && flag.PickedUp) { // the other player has the flag
                GameMenuManager.Instance.UpdateText(gameObject.name + " is taking the flag");
                if (IsOwner) {
                    CaptureFlag(other);
                    StartCoroutine(other.GetComponent<ControlledPlayer>().Invincibility());
                }
            }
            *//*else if (holdingFlag.Value && !other.GetComponent<GeneralPlayer>().invincible.Value) { // we have the flag
                if (IsOwner) {
                    Debug.Log(gameObject.name + " is invincible");
                    StartCoroutine(Invincibility());
                    holdingFlag.Value = false;
                }

                    *//*flag.transform.parent = other.transform;
                    flag.transform.position = flagPos.transform.position;
                    flag.transform.rotation = flagPos.transform.rotation;*//*
                }*//*
            }*/
    }

    [ServerRpc(RequireOwnership =false)]
    void ChangeItServerRpc() {
        GameManager.Instance.HostIsIt = !GameManager.Instance.HostIsIt;
        ChangeItClientRpc();
    }

    [ClientRpc]
    void ChangeItClientRpc() {
        if (IsHost) return;
        GameManager.Instance.HostIsIt = !GameManager.Instance.HostIsIt;
    }


    protected virtual IEnumerator Invincibility()
    {
        throw new Exception("Override this function");
    }

    protected virtual void CaptureFlag(Collider other=null)
    {
        //flag.CaptureFlag(GetComponent<ControlledPlayer>(), flagPos.transform);
        /*if (IsHost) flag.hostHoldingFlag.Value = true;*/
        flag.CaptureFlagServerRpc(NetworkObjectId);
        /*if (IsHost)
            flag.CaptureFlagClientRpc(NetworkObjectId, true);
        else
            flag.CaptureFlagServerRpc(NetworkObjectId);*/
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
