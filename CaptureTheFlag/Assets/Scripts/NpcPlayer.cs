using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPlayer : GeneralPlayer
{
    private NPCMovement movement;
    [SerializeField] private float _runToGoalSpeed = 2f;
    [SerializeField] private float _runToPlayerSpeed = 3f;

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
        movement.Speed = _runToGoalSpeed;
    }

    protected override IEnumerator Invincibility()
    {
        Invincible = true;
        NPCMovement.MovementType initialMovementType = movement.movementType;
        movement.Target = GameManager.Instance.controlledPlayer.gameObject;
        movement.Speed = _runToPlayerSpeed;
        movement.movementType = NPCMovement.MovementType.noMove;
        yield return new WaitForSeconds(invincibilityTime);
        movement.movementType = initialMovementType;
        Invincible = false;
    }

    public override void ScoreFlag()
    {
        movement.MyNavMeshAgent.Warp(startPos);
        base.ScoreFlag();
        movement.Target = flag.gameObject;
        movement.Speed = _runToPlayerSpeed;
    }
    
}
