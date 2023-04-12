using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPlayer : GeneralPlayer
{
    private NPCMovement movement;
    protected override void Start()
    {
        base.Start();
        movement = GetComponent<NPCMovement>();


    }

    protected override IEnumerator Invincibility()
    {
        NPCMovement.MovementType initialMovementType = movement.movementType;
        movement.movementType = NPCMovement.MovementType.noMove;
        yield return new WaitForSeconds(invincibilityTime);
        movement.movementType = initialMovementType;
    }
}
