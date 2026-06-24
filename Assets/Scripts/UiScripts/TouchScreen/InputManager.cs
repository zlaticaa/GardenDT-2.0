using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private TouchSystem touchSystem;

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent onStartTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent onEndTouch;

    public delegate void DragEvent(Vector2 position);
    public event DragEvent onDrag;

    private bool isDragging;

    private void Awake()
    {
        touchSystem = new TouchSystem();
    }

    private void OnEnable()
    {
        touchSystem.Enable();
    }

    private void OnDisable()
    {
        touchSystem.Disable();
    }

    private void Start()
    {
        touchSystem.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchSystem.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
        touchSystem.Touch.TouchPosition.performed += ctx => Drag(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        isDragging = true;

        Vector2 pos = touchSystem.Touch.TouchPosition.ReadValue<Vector2>();

        Debug.Log($"[INPUT] TOUCH END Position: {pos}");

        onStartTouch?.Invoke(pos, (float)context.startTime);
    }

    private void Drag(InputAction.CallbackContext context)
    {
        if (!isDragging) return;

        Vector2 pos = context.ReadValue<Vector2>();

        Debug.Log($"[INPUT] TOUCH END  Position: {pos}");

        onDrag?.Invoke(pos);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        isDragging = false;

        Vector2 pos = touchSystem.Touch.TouchPosition.ReadValue<Vector2>();

        Debug.Log($"[INPUT] TOUCH END  Position: {pos}");


        onEndTouch?.Invoke(pos, (float)context.time);
    }
}