using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPlayer : GeneralPlayer
{
    private NPCMovement movement;
    [SerializeField] private Transform goal;
    protected override void Start()
    {
        base.Start();
        movement = GetComponent<NPCMovement>();


    }

    protected override void CaptureFlag()
    {
        base.CaptureFlag();
        movement.target = goal.gameObject;
    }

    protected override IEnumerator Invincibility()
    {
        NPCMovement.MovementType initialMovementType = movement.movementType;
        movement.target = GameManager.Instance.controlledPlayer.gameObject;
        movement.movementType = NPCMovement.MovementType.noMove;
        yield return new WaitForSeconds(invincibilityTime);
        movement.movementType = initialMovementType;
    }
}
