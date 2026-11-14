using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement Settings")]
    [SerializeField] private float baseSpeed;
    private float finalSpeed;
    private float currentSpeedMultiplier = 1f;

    public float Speed
    {
        get { return finalSpeed; }
        set { finalSpeed = value; }
    }
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;
    private float moveHorizontal;
    private float moveVertical;
    private Rigidbody rb;
    private Transform camTransform;

    [Header ("Raycast Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        //Input 
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        CheckGround();

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (camTransform == null) return;

        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * moveVertical + camRight * moveHorizontal).normalized;
        finalSpeed = baseSpeed * currentSpeedMultiplier;
        rb.linearVelocity = new Vector3(movement.x * finalSpeed, rb.linearVelocity.y, movement.z * finalSpeed);

        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position;
        isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);

        Color ColorRay = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin, Vector3.down * rayDistance, ColorRay);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        currentSpeedMultiplier = multiplier;
        Debug.Log($"Velocidad actual: {baseSpeed * currentSpeedMultiplier}");
    }

    public void ResetSpeedMultiplier()
    {
        currentSpeedMultiplier = 1f;
        Debug.Log($"Velocidad actual: {baseSpeed * currentSpeedMultiplier}");
    }

}
