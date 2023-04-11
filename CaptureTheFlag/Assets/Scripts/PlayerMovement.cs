using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] Transform FlagPos;
    [SerializeField] float invincibilityTime = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector2 movementInput;
    //private Vector3 movementDirection;
    static PlayerMovement instance;
    Animator anim;
    Flag flag;
    bool holdingFlag = false; // THISSSSSSSSSSS

    public static PlayerMovement Instance { get {
            if (instance == null) instance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            return instance;
        } }

    private CaptureTheFlag controls;

    private void Awake() {
        controller = GetComponent<CharacterController>();

        // Initialize the new input system controls
        controls = new CaptureTheFlag();
        controls.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += _ => movementInput = Vector2.zero;
        controls.Player.Jump.performed += _ => Jump();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start() {
        flag = GameManager.Instance.Flag.GetComponent<Flag>();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    private void Update() {
        // Check if the character is grounded
        isGrounded = controller.isGrounded;

        // Apply gravity
        if (isGrounded && velocity.y < 0) {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;

        Move();
    }

    private void Move() {
        // Get the camera's forward vector and flatten it on the XZ plane
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;

        // Calculate the movement direction based on player input and camera direction
        Vector3 moveDirection = (cameraForward * movementInput.y + Camera.main.transform.right * movementInput.x).normalized;

        // Rotate the character towards the movement direction
        if (moveDirection.magnitude > 0.1f) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            anim.SetTrigger("RUN");
        }
        else {
            anim.SetTrigger("IDLE");
        }

        // Move the character
        Vector3 movement = moveDirection * speed * Time.deltaTime;
        controller.Move(movement);
    }

    private void Jump() {
        if (isGrounded) {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    IEnumerator Invincibility() {
        controls.Disable();
        yield return new WaitForSeconds(invincibilityTime);
        controls.Enable();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && !holdingFlag) {
            flag.CaptureFlag(FlagPos);
            holdingFlag = true;
        }
        else if (collision.gameObject.CompareTag("Player")) {
            holdingFlag = false;
            StartCoroutine(Invincibility());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Flag")) {
            flag.CaptureFlag(FlagPos);
            holdingFlag = true;
        }
    }
}