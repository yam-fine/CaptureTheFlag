using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;    // The target to follow
    public float distance = 5f; // The distance from the target
    public float sensitivity = 5f; // The sensitivity of mouse movement
    public float minDistance = 2f; // The minimum distance from the target
    public float maxDistance = 10f; // The maximum distance from the target

    private float currentX = 0f; // The current x rotation of the camera
    private float currentY = 0f; // The current y rotation of the camera

    private void Start() {
        // Set the initial camera position
        transform.position = target.position - transform.forward * distance;
        transform.position = new Vector3(transform.position.x, 5);
    }

    private void LateUpdate() {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Update the camera rotation based on mouse input
        currentX += mouseX;
        currentY -= mouseY;
        currentY = Mathf.Clamp(currentY, -89f, 89f);

        // Calculate the new camera position based on the target position and rotation
        Vector3 dir = new Vector3(0f, 0f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
        Vector3 position = target.position + rotation * dir;

        // Check for collisions with the environment
        RaycastHit hitInfo;
        if (Physics.Linecast(target.position, position, out hitInfo)) {
            position = hitInfo.point;
            distance = hitInfo.distance;
        }

        // Clamp the distance to the target
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Update the camera position and rotation
        transform.position = position;
        transform.rotation = rotation;
    }
}
