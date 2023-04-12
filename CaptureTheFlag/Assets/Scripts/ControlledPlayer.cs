using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledPlayer : GeneralPlayer
{
    private PlayerMovement movement;
    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
    }

    protected override IEnumerator Invincibility() {
        invincible = true;
        movement.EnableControls(false);
        yield return new WaitForSeconds(invincibilityTime);
        movement.EnableControls(true);
        invincible = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit collision) {
        if (collision.gameObject.CompareTag("Player") && !holdingFlag && !invincible && flag.PickedUp) {
            Debug.Log("HOLD");
            CaptureFlag();
        }
        else if (collision.gameObject.CompareTag("Player") && holdingFlag) {
            holdingFlag = false;
            StartCoroutine(Invincibility());
        }
    }
}
