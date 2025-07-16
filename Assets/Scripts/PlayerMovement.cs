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
    private bool dashTriggered = false;

    [Header("Swipe Settings")]
    private Vector2 swipeStartPos;
    public float minSwipeDistance = 100f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedManager = FindFirstObjectByType<SpeedMoveManager>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (Touchscreen.current != null && context.control.device is Touchscreen)
        {
            Vector2 touchPos = context.ReadValue<Vector2>();
            float screenMid = Screen.width / 2f;

            movementInput = new Vector2(touchPos.x < screenMid ? -1f : 1f, 0f);
        }
        else
        {
            movementInput = context.ReadValue<Vector2>();

        }
    }

    void FixedUpdate()
    {
        if (speedManager == null) return;
        float sidewaysForce = speedManager.GetCurrentSpeed();

        if (Touchscreen.current != null && !Touchscreen.current.primaryTouch.press.isPressed)
        {
            movementInput = Vector2.zero;
        }

        rb.AddForce(new Vector3(movementInput.x * sidewaysForce * Time.deltaTime, 0), ForceMode.VelocityChange);

        // Обмеження по межах платформи
        Vector3 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, -platformWidth, platformWidth);
        rb.position = pos;
    }

    void Update()
    {
        // Свайп для телефону
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                swipeStartPos = touch.position.ReadValue();
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                Vector2 endTouchPos = touch.position.ReadValue();
                float deltaX = endTouchPos.x - swipeStartPos.x;

                if (Mathf.Abs(deltaX) > minSwipeDistance)
                {
                    if (deltaX > 0)
                        DashRight();
                    else
                        DashLeft();
                }
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !dashTriggered)
        {
            dashTriggered = true;
            if (Keyboard.current.aKey.isPressed)
                DashLeft();
            else if (Keyboard.current.dKey.isPressed)
                DashRight();
        }
        else if (context.canceled)
        {
            dashTriggered = false;
        }
    }

    void DashLeft()
    {
        if (isDashing) return;
        float targetX = Mathf.Clamp(rb.position.x - dashDistance, -platformWidth, platformWidth);
        DoTweenDashToX(targetX, true);
    }

    void DashRight()
    {
        if (isDashing) return;
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