using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardForce = 1000f;
    public float sidewaysForce = 500f;
    public float platformWidth = 7f; 

    private Rigidbody rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(movementInput.x * sidewaysForce * Time.deltaTime, 0, forwardForce * Time.deltaTime), ForceMode.VelocityChange);
    }
}
