using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class ControlledPlayer : GeneralPlayer
{
    [SerializeField] CharacterController cc;
    [SerializeField] PlayerMovement pm;
    [SerializeField] PlayerInput pi;
    [SerializeField] BoxCollider bc;
    private PlayerMovement movement;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
        ScoreSide = GameMenuManager.Side.Right;
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (!IsOwner) {
            bc.isTrigger = true;
            gameObject.AddComponent<Rigidbody>();
            return;
        }
        StartPos();

        cc.enabled = true;
        pi.enabled = true;
    }

    void StartPos() {
        Transform spawnPos;
        if (IsHost)
            spawnPos = GameObject.Find("HostSpawnPos").transform;
        else
            spawnPos = GameObject.Find("ClientSpawnPos").transform;
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;
        pm.enabled = true;
    }


    protected override IEnumerator Invincibility() {
        invincible = true;
        if (IsOwner) movement.EnableControls(false);
        yield return new WaitForSeconds(invincibilityTime);
        if (IsOwner) movement.EnableControls(true);
        invincible = false;
    }

    public override void ScoreFlag() {
        transform.position = startPos;
        base.ScoreFlag();
    }
}
