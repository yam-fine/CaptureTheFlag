using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] float _speed = 2f;
    [SerializeField] private float _rotationSpeed = 0.5f;
    [SerializeField] private float _minDistance = 1.5f;
    [SerializeField] float detectionRadius = 5f; // from where will the AI go faster
    [SerializeField] float speedMultiplier = 2;
    
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    private NavMeshAgent _navMeshAgent;
    GameObject player;
    Animator anim;
    
    GameObject target;

    public enum MovementType { costume, navmesh, noMove };

    public MovementType movementType = MovementType.costume;

    public GameObject Target { get => target; set { target = value; } }

    public NavMeshAgent MyNavMeshAgent { get => _navMeshAgent; set => _navMeshAgent = value; }

    private void Awake()
    {
        MyNavMeshAgent = GetComponent<NavMeshAgent>();
        _speed = MyNavMeshAgent.speed;
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
       MyNavMeshAgent.isStopped = false;


        switch (movementType)
        {
            case MovementType.costume:
                CostumeMovement();
                break;
            case MovementType.navmesh:
                NavMeshMovement();
                break;
            case MovementType.noMove:
                anim.SetFloat("Speed", 0);
                MyNavMeshAgent.isStopped = true;
                return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius) {
            MyNavMeshAgent.speed = _speed * speedMultiplier;
            anim.SetFloat("Speed", 2);
        }
        else {
            MyNavMeshAgent.speed = _speed;
            anim.SetFloat("Speed", 1);
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
        MyNavMeshAgent.SetDestination(Target.transform.position);
    }
    
    
}