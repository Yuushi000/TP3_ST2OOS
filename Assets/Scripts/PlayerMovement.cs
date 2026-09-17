using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    public float moveSpeed = 6f;
    public Transform cameraTransform;
    public float rotationSpeed = 10f;

    [Header("Saut")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (animator != null)
            animator.SetBool("IsGrounded", isGrounded);

        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

            if (animator != null)
                animator.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null || cameraTransform == null) return;

        float moveX = 0f;
        float moveZ = 0f;

        if (keyboard.wKey.isPressed) moveZ = 1f;
        if (keyboard.sKey.isPressed) moveZ = -1f;
        if (keyboard.aKey.isPressed) moveX = -1f;
        if (keyboard.dKey.isPressed) moveX = 1f;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveZ + camRight * moveX).normalized;
        Vector3 targetVelocity = moveDirection * moveSpeed;

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        // Fait tourner le Player pour qu'il regarde dans la direction du deplacement
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Met a jour la vitesse pour l'Animator (0 = immobile, 1+ = en mouvement)
        if (animator != null)
        {
            float currentSpeed = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
            animator.SetFloat("Speed", currentSpeed);
        }
    }
}