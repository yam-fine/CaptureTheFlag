using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform cameraTarget;
    //[SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float rotationSpeed;
    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 movementInput;
    //private Vector3 movementDirection;
    static PlayerMovement instance;
    Animator anim;
    private CaptureTheFlag.CaptureTheFlagInputs _input;
    private int isFallingdCounter = 0; // it is always alternating between true and false, if there is 3 in a row we can conclude its falling.
    private Vector3 lastSeenPos;
    Vector3 cameraForward;

    public static PlayerMovement Instance { get {
            if (instance == null) instance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            return instance;
        } }

    private void Awake() {
        if (!IsOwner) return;

        

        // Initialize the new input system controls
        //controls.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        //controls.Player.Movement.canceled += _ => movementInput = Vector2.zero;
        //controls.Player.Jump.performed += _ => Jump();
    }

    private void Start() {
        CinemachineCameraController cam = FindObjectOfType<CinemachineCameraController>();
        //cam.transform.position = transform.forward * -10;
        cam.target = cameraTarget;
        cameraForward = Camera.main.transform.forward;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        _input = GetComponent<CaptureTheFlag.CaptureTheFlagInputs>();
    }

    /*private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }*/

    private void Update() {
        if (controller.isGrounded)
        {
            isFallingdCounter = 0;
            lastSeenPos = transform.position;
        }
        else
        {
            isFallingdCounter++;
        }

        if (isFallingdCounter >= 5)
        {
            transform.position = lastSeenPos;
            isFallingdCounter = 0;
        }
        Move();
    }

    private void Move() {
        // Get the camera's forward vector and flatten it on the XZ plane
        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;

        // Calculate the movement direction based on player input and camera direction
        Vector3 moveDirection = (cameraForward * _input.move.y + Camera.main.transform.right * _input.move.x).normalized;
        print(_input.move);
        // Rotate the character towards the movement direction
        if (moveDirection.magnitude > 0.1f) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            anim.SetFloat("Speed", 2);
        }
        else {
            anim.SetFloat("Speed", 0);
        }

        // Move the character
        Vector3 movement = moveDirection * speed * Time.deltaTime;

        // Apply gravity
        if (controller.isGrounded) {
            velocity = Vector3.zero;
            controller.Move(movement);
        }
        else {
            velocity.y = gravity * Time.deltaTime * 0.3f;
            controller.Move(movement + velocity);
        }

    }
    

    public void EnableControls(bool enable)
    {
        //(enable ? (Action)controls.Enable : (Action)controls.Disable)();
        Debug.LogError("Bruh");
    }
}