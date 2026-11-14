using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Simple first-person fly explorer for the camera.
/// W/S = forward/back, A/D = left/right, Q/E = down/up.
/// Hold the RIGHT mouse button and move the mouse to look around (optional mouse look).
/// Left/Right arrow keys also turn the camera. Left Shift moves faster.
/// All movement is multiplied by Time.deltaTime so it is frame-rate independent.
/// </summary>
public class ExplorerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 12f;        // metres per second on the ground plane
    [SerializeField] private float verticalSpeed = 8f;     // metres per second for Q / E
    [SerializeField] private float sprintMultiplier = 2f;  // multiplier while Left Shift is held

    [Header("Turning")]
    [SerializeField] private float turnSpeed = 60f;        // degrees per second for arrow keys
    [SerializeField] private float mouseSensitivity = 0.15f; // degrees per pixel of mouse movement
    [SerializeField] private float maxLookAngle = 80f;     // how far up/down you can look

    [Header("Safety Limits")]
    [SerializeField] private Terrain terrain;              // used to stop the camera going under the ground
    [SerializeField] private float minHeightAboveGround = 1.8f;
    [SerializeField] private float maxHeight = 90f;
    [SerializeField] private float worldLimit = 185f;      // keeps the player inside the terrain edges

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        // Read keys: each one gives -1, 0 or +1
        float forwardInput = Axis(keyboard.wKey.isPressed, keyboard.sKey.isPressed);
        float sideInput = Axis(keyboard.dKey.isPressed, keyboard.aKey.isPressed);
        float verticalInput = Axis(keyboard.eKey.isPressed, keyboard.qKey.isPressed);
        float turnInput = Axis(keyboard.rightArrowKey.isPressed, keyboard.leftArrowKey.isPressed);

        float speed = moveSpeed;
        if (keyboard.leftShiftKey.isPressed)
        {
            speed *= sprintMultiplier;
        }

        // Move along the flat (horizontal) forward/right directions so looking down does not push us into the ground
        Vector3 flatForward = transform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();
        Vector3 flatRight = transform.right;
        flatRight.y = 0f;
        flatRight.Normalize();

        Vector3 move = (flatForward * forwardInput + flatRight * sideInput) * speed;
        move += Vector3.up * (verticalInput * verticalSpeed);

        transform.position += move * Time.deltaTime;
        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f, Space.World);

        MouseLook();
        KeepInsideWorld();
    }

    // Optional mouse look: only while the right mouse button is held,
    // so the keyboard-only explorer still works exactly as before.
    private void MouseLook()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || !mouse.rightButton.isPressed)
        {
            return;
        }

        // Mouse delta is already the movement since the last frame, so it does not need Time.deltaTime
        Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;

        Vector3 angles = transform.eulerAngles;
        float pitch = angles.x > 180f ? angles.x - 360f : angles.x;   // convert 0..360 to -180..180
        pitch = Mathf.Clamp(pitch - delta.y, -maxLookAngle, maxLookAngle);
        float yaw = angles.y + delta.x;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private static float Axis(bool positive, bool negative)
    {
        return (positive ? 1f : 0f) - (negative ? 1f : 0f);
    }

    private void KeepInsideWorld()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -worldLimit, worldLimit);
        pos.z = Mathf.Clamp(pos.z, -worldLimit, worldLimit);

        float minY = 0f;
        if (terrain != null)
        {
            minY = terrain.SampleHeight(pos) + terrain.transform.position.y + minHeightAboveGround;
        }
        pos.y = Mathf.Clamp(pos.y, minY, Mathf.Max(minY, maxHeight));

        transform.position = pos;
    }
}
