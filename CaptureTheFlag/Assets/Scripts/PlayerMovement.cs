using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float rotationSpeed;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector2 movementInput;
    private Vector3 movementDirection;

    private CaptureTheFlag controls;
    private void Awake() {
        controller = GetComponent<CharacterController>();

        // Initialize the new input system controls
        controls = new CaptureTheFlag();
        controls.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += _ => movementInput = Vector2.zero;
        controls.Player.Jump.performed += _ => Jump();
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
}