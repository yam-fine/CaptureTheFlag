using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float _rotationSpeed = 0.5f;
    [SerializeField] private float _minDistance = 1.5f;

    private NavMeshAgent _navMeshAgent;
    
    public enum MovementType { costume, navmesh };

    public MovementType movementType = MovementType.costume;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (movementType)
        {
            case MovementType.costume:
                CostumeMovement();
                break;
            case MovementType.navmesh:
                NavMeshMovement();
                break;
        }
    }


    private void CostumeMovement()
    {
        var goal = GameManager.Instance.Player.transform.position;
        Vector3 realGoal = new Vector3(goal.x, transform.position.y, goal.z);
        Vector3 direction = realGoal - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed);
        Debug.DrawRay(transform.position, direction, Color.green); // To show where AI facing

        if (direction.magnitude >= _minDistance)
        {
            Vector3 pushVector = direction.normalized * _speed;
            transform.Translate(pushVector, Space.World);
        }
        else
        {
            // Animator set enum to "close" 
        }
    }
    private void NavMeshMovement()
    {
        _navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
    }
}