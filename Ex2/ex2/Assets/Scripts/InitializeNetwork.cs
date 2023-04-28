using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InitializeNetwork : NetworkBehaviour
{
    [SerializeField] private MonoBehaviour[] _componentsWithInput;
    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        foreach (var component in _componentsWithInput)
        {
            component.enabled = true;
        }
    }
}
