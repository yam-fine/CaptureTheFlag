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

    private PlayerMovement movement;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
        ScoreSide = GameMenuManager.Side.Right;
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (!IsOwner) return;

        cc.enabled = true;
        pm.enabled = true;
        pi.enabled = true;

        StartPos();
    }

    void StartPos() {
        Transform spawnPos;
        if (IsHost)
            spawnPos = GameObject.Find("HostSpawnPos").transform;
        else
            spawnPos = GameObject.Find("ClientSpawnPos").transform;
        transform.position = spawnPos.position;
    }

    protected override IEnumerator Invincibility() {
        Invincible = true;
        movement.EnableControls(false);
        yield return new WaitForSeconds(invincibilityTime);
        movement.EnableControls(true);
        Invincible = false;
    }

    public override void ScoreFlag() {
        transform.position = startPos;
        base.ScoreFlag();
    }
}
