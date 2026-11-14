using UnityEngine;

/// <summary>
/// Ambient animation 1: makes an object slowly rotate and bob up and down.
/// Used for the floating shrine crystals.
/// </summary>
public class FloatingCrystal : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;   // degrees per second around the Y axis
    [SerializeField] private float bobHeight = 0.6f;      // how far it moves up and down (metres)
    [SerializeField] private float bobSpeed = 1.2f;       // how fast it bobs

    private Vector3 startPosition;
    private float phaseOffset;

    private void Start()
    {
        startPosition = transform.position;
        // Different start phase so several crystals do not move in perfect sync
        phaseOffset = (startPosition.x + startPosition.z) * 0.37f;
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);

        float offset = Mathf.Sin(Time.time * bobSpeed + phaseOffset) * bobHeight;
        transform.position = startPosition + Vector3.up * offset;
    }
}
