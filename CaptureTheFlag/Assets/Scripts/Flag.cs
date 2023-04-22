using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flag : NetworkBehaviour
{
    [SerializeField] Transform initPos;
    private NetworkVariable<bool> pickedUp = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public NetworkVariable<bool> hostHoldingFlag = new NetworkVariable<bool>(false);

    BoxCollider col;

    public bool PickedUp { get => pickedUp.Value;}

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
            pickedUp.Value = true;
            col.enabled = false;
            transform.parent = cc.transform;
            /*transform.position = parent.position;
            transform.rotation = parent.rotation;*/
            CaptureFlagClientRpc(player);
        //}
    }

    [ClientRpc]
    public void CaptureFlagClientRpc(ulong player) {
        if (IsHost) return;
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(player, out var playerObj);
        //if (playerObj == null || playerObj.transform.parent != null) return; // object already picked up, server authority says no

        //if (this.TryGetComponent(out NetworkObject networkObject) && networkObject.TrySetParent(playerObj)) {
            ControlledPlayer cc = playerObj.GetComponent<ControlledPlayer>();
            Transform parent = cc.flagPos.transform;
            pickedUp.Value = true;
            col.enabled = false;
            transform.parent = cc.transform;
            /*transform.position = parent.position;
            transform.rotation = parent.rotation;*/
        //}
    }

    /*public IEnumerator Refresh() {
        canHold = false;
        yield return new WaitForSeconds()
    }*/

    public void Goal() {
        pickedUp.Value = false;
        transform.parent = null;
        transform.position = initPos.position;
        transform.rotation = initPos.rotation;
        col.enabled = true;
    }
}
