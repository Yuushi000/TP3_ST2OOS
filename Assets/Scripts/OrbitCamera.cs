using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    [Header("Cibles")]
    public Transform target;       // CameraTarget (au niveau de la tete du Player)
    public Transform playerBody;   // La capsule Robert (pour la faire tourner)

    [Header("Distance / Zoom")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;

    [Header("Rotation")]
    public float rotationSpeed = 3f;
    public float minVerticalAngle = -20f; 
    public float maxVerticalAngle = 60f;  

    private float yaw = 0f;
    private float pitch = 15f;

    void Start()
    {
        if (playerBody != null)
            yaw = playerBody.eulerAngles.y;
    }

    void LateUpdate()
    {
        var mouse = Mouse.current;
        if (mouse == null || target == null) return;

        // Zoom avec la molette
        float scroll = mouse.scroll.ReadValue().y;
        distance -= scroll * zoomSpeed * 0.15f;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);


        Vector2 mouseDelta = mouse.delta.ReadValue();

        bool rightHeld = mouse.rightButton.isPressed;
        bool leftHeld = mouse.leftButton.isPressed;

        if (rightHeld || leftHeld)
        {
            yaw += mouseDelta.x * rotationSpeed * 0.1f;
            pitch -= mouseDelta.y * rotationSpeed * 0.1f;
            pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);
        }

        
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);

        
        if (rightHeld && playerBody != null)
        {
            Vector3 flatForward = new Vector3(offset.x, 0f, offset.z).normalized;
            if (flatForward.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-flatForward, Vector3.up);
                playerBody.rotation = targetRotation;
            }
        }
        
    }
}