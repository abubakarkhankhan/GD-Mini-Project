using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Jumps the camera to one of three viewpoints when keys 1, 2 or 3 are pressed.
/// The viewpoints are empty GameObjects assigned in the Inspector.
/// </summary>
public class CameraViewpointSwitcher : MonoBehaviour
{
    [Header("What to move")]
    [SerializeField] private Transform cameraToMove;   // the Main Camera (the explorer)

    [Header("Viewpoints (empty GameObjects)")]
    [SerializeField] private Transform viewpoint1;     // forest entrance and path
    [SerializeField] private Transform viewpoint2;     // ancient ruins
    [SerializeField] private Transform viewpoint3;     // main shrine

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame)
        {
            GoTo(viewpoint1);
        }
        else if (keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame)
        {
            GoTo(viewpoint2);
        }
        else if (keyboard.digit3Key.wasPressedThisFrame || keyboard.numpad3Key.wasPressedThisFrame)
        {
            GoTo(viewpoint3);
        }
    }

    private void GoTo(Transform viewpoint)
    {
        if (viewpoint == null || cameraToMove == null)
        {
            Debug.LogWarning("CameraViewpointSwitcher: a viewpoint or the camera is not assigned in the Inspector.");
            return;
        }

        cameraToMove.SetPositionAndRotation(viewpoint.position, viewpoint.rotation);
    }
}
