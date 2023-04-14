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
        ScoreSide = GameMenuManager.Side.Left;
        movement.Target = flag.gameObject;


    }

    protected override void CaptureFlag()
    {
        base.CaptureFlag();
        movement.Target = Goal;
    }

    protected override IEnumerator Invincibility()
    {
        Invincible = true;
        NPCMovement.MovementType initialMovementType = movement.movementType;
        movement.Target = GameManager.Instance.controlledPlayer.gameObject;
        movement.movementType = NPCMovement.MovementType.noMove;
        yield return new WaitForSeconds(invincibilityTime);
        movement.movementType = initialMovementType;
        Invincible = false;
    }

    public override void ScoreFlag()
    {
        base.ScoreFlag();
        movement.Target = flag.gameObject;
    }
}
