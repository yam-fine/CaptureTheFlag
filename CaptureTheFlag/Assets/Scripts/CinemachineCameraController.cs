using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour {
    public Transform target; // The target (player) object to rotate around
    public float sensitivity = 3f; // Sensitivity of the mouse movement
    public float distance = 5f; // Distance from the target

    private float currentX = 0f;
    private float currentY = 0f;


    private void LateUpdate() {
        // Update the camera rotation based on mouse input
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, -80f, 80f);

        // Calculate the new camera position and rotation
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rot = Quaternion.Euler(currentY, currentX, 0);
        Vector3 newPos = target.position + rot * dir;

        // Update the camera position and rotation
        transform.position = newPos;
        transform.rotation = rot;
    }
}
