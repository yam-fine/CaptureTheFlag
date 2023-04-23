using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flag : NetworkBehaviour
{
    [SerializeField] Transform initPos;

    public enum FlagState {
        HELD_BY_CLIENT,
        HELD_BY_SERVER
    }

    private bool pickedUp = false;
    public NetworkVariable<FlagState> flagState = new NetworkVariable<FlagState>();

    BoxCollider col;

    public bool PickedUp { get => pickedUp; set => pickedUp = value; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Flag = gameObject;
        col = GetComponent<BoxCollider>();
    }

    //public void CaptureFlag(ControlledPlayer player, Transform flagPos) {
    //    pickedUp.Value = true;
    //    col.enabled = false;
    //    transform.parent = player.transform;
    //    transform.position = flagPos.position;
    //    transform.rotation = flagPos.rotation;
    //}

    [ServerRpc(RequireOwnership = false)]
    public void InitPickupServerRpc() {
        PickedUp = true;
        col.enabled = false;
        InitPickupClientRpc();
    }

    [ClientRpc]
    public void InitPickupClientRpc() {
        if (IsHost) return;
        col.enabled = false;
        PickedUp = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CaptureFlagServerRpc(ulong player) {
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(player, out var playerObj);
        /*if (playerObj == null || playerObj.transform.parent != null) { // object already picked up, server authority says no
            print("HELLO");
            return;
        }*/

        //if (TryGetComponent(out NetworkObject networkObject) && networkObject.TrySetParent(playerObj)) {
            ControlledPlayer cc = playerObj.GetComponent<ControlledPlayer>();
            Transform parent = cc.flagPos.transform;
            //GetComponent<NetworkObject>().ChangeOwnership(player);
        transform.parent = cc.transform;
        if (NetworkManager.Singleton.LocalClientId == NetworkManager.ServerClientId) {
            cc.holdingFlag = true;

        }
        //other.
            /*transform.position = parent.position;
            transform.rotation = parent.rotation;*/
            CaptureFlagClientRpc(player);
        //}
    }

    [ClientRpc]
    public void CaptureFlagClientRpc(ulong player, bool fromHost=false) {
        if (!fromHost && IsHost) return;
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(player, out var playerObj);
        //if (playerObj == null || playerObj.transform.parent != null) return; // object already picked up, server authority says no

        //if (this.TryGetComponent(out NetworkObject networkObject) && networkObject.TrySetParent(playerObj)) {
        foreach (var obj in NetworkManager.Singleton.ConnectedClientsList) {
            if (obj.PlayerObject.NetworkObjectId != player) {

            }
        }
        ControlledPlayer cc = playerObj.GetComponent<ControlledPlayer>();
        Transform parent = cc.flagPos.transform;
        //transform.parent = cc.transform;
            /*transform.position = parent.position;
            transform.rotation = parent.rotation;*/
        //}
    }

    /*public IEnumerator Refresh() {
        canHold = false;
        yield return new WaitForSeconds()
    }*/

    public void Goal() {
        pickedUp = false;
        transform.parent = null;
        transform.position = initPos.position;
        transform.rotation = initPos.rotation;
        col.enabled = true;
    }
}
