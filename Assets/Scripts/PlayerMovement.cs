using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float sidewaysForceBase = 50f;
    public float platformWidth = 7f;

    private Rigidbody rb;
    private Vector2 movementInput;
    public float currentSidewaysForce;
    private SpeedMoveManager speedManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        speedManager = FindFirstObjectByType<SpeedMoveManager>();
        if (speedManager == null)
        {
            Debug.LogError("SpeedManager not found!");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        float sidewaysForce = sidewaysForceBase;

        Vector3 force = new Vector3(movementInput.x * sidewaysForce * Time.fixedDeltaTime, 0, 0);
        rb.AddForce(force, ForceMode.VelocityChange);
    }
}
