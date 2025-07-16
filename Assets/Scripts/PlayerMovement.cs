using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float platformWidth = 7f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private SpeedMoveManager speedManager;

    [Header("Dash Settings")]
    public float dashDistance = 3f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    private bool isDashing = false;

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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 touchPos = Vector2.zero;

            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
                float screenMid = Screen.width / 2f;

                if (touchPos.x < screenMid)
                    DashLeft();
                else
                    DashRight();
            }
            else
            {
                if (Keyboard.current.aKey.isPressed)
                    DashLeft();
                else if (Keyboard.current.dKey.isPressed)
                    DashRight();
            }
        }
    }

    void DashLeft()
    {
        if (isDashing) return;
        Debug.Log("left");

        float targetX = Mathf.Clamp(rb.position.x - dashDistance, -platformWidth, platformWidth);
        DoTweenDashToX(targetX, true);
    }

    void DashRight()
    {
        if (isDashing) return;
        Debug.Log("right");
        float targetX = Mathf.Clamp(rb.position.x + dashDistance, -platformWidth, platformWidth);
        DoTweenDashToX(targetX, false);
    }

    void DoTweenDashToX(float targetX, bool isLeft)
    {
        if (isDashing) return;
        isDashing = true;


        Tween moveTween = rb.DOMoveX(targetX, dashDuration)
         .SetEase(Ease.OutQuad);
        float targetZRotation = rb.transform.eulerAngles.z + (isLeft ? 90f : -90f);

        Tween rotateTween = rb.transform.DORotate(new Vector3(0, 0, targetZRotation), dashDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad);

        Sequence seq = DOTween.Sequence();
        seq.Append(moveTween);
        seq.Join(rotateTween);
        seq.OnComplete(() => isDashing = false);
    }
}
