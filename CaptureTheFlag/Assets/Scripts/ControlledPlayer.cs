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
        movement.GetComponent<PlayerMovement>();
    }

    protected override IEnumerator Invincibility() {
        movement.EnableControls(false);
        yield return new WaitForSeconds(invincibilityTime);
        movement.EnableControls(true);
    }
}
