using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float platformWidth = 7f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private SpeedMoveManager speedManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedManager = FindFirstObjectByType<SpeedMoveManager>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (speedManager == null) return;

        float sidewaysForce = speedManager.GetCurrentSpeed();

        rb.AddForce(new Vector3(movementInput.x * sidewaysForce * Time.deltaTime, 0), ForceMode.VelocityChange);

        // Обмеження по межах платформи
        Vector3 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, -platformWidth, platformWidth);
        rb.position = pos;
    }
}
