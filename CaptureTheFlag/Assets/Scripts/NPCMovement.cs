using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] float _speed = 0.5f;
    [SerializeField] private float _rotationSpeed = 0.5f;
    [SerializeField] private float _minDistance = 1.5f;
    [SerializeField] float detectionRadius = 5f; // from where will the AI go faster
    [SerializeField] float speedMultiplier = 2;

    private NavMeshAgent _navMeshAgent;
    GameObject player;
    Animator anim;
    
    GameObject target;

    public enum MovementType { costume, navmesh, noMove };

    public MovementType movementType = MovementType.costume;

    public GameObject Target { get => target; set { target = value; } }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _speed = _navMeshAgent.speed;
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        player = GameManager.Instance.controlledPlayer.gameObject;
        Target = player;
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
            case MovementType.noMove:
                anim.SetTrigger("IDLE");
                return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius) {
            _navMeshAgent.speed = _speed * speedMultiplier;
            anim.SetTrigger("RUN");
        }
        else {
            _navMeshAgent.speed = _speed;
            anim.SetTrigger("WALK");
        }
    }


    private void CostumeMovement()
    {
        var goal = player.transform.position;
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
        Debug.Log(target.transform.position);
        _navMeshAgent.SetDestination(Target.transform.position);
    }
}