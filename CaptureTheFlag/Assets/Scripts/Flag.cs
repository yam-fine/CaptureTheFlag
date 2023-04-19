using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flag : NetworkBehaviour
{
    [SerializeField] Transform initPos;
    bool pickedUp = false;
    BoxCollider col;

    public bool PickedUp { get => pickedUp;}

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Flag = gameObject;
        col = GetComponent<BoxCollider>();
    }

    [ServerRpc]
    public void CaptureFlagServerRpc(ulong player) {
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(player, out var playerObj);
        if (playerObj == null || playerObj.transform.parent != null) return; // object already picked up, server authority says no

        if (this.TryGetComponent(out NetworkObject networkObject) && networkObject.TrySetParent(playerObj)) {
            Transform parent = playerObj.GetComponent<ControlledPlayer>().flagPos.GetComponent<Transform>();
            pickedUp = true;
            col.enabled = false;
            transform.parent = parent;
            transform.position = parent.position;
            transform.rotation = parent.rotation;
        }
    }

    public void Goal() {
        pickedUp = false;
        transform.parent = null;
        transform.position = initPos.position;
        transform.rotation = initPos.rotation;
        col.enabled = true;
    }
}
