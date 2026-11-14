using UnityEngine;

/// <summary>
/// Ambient animation 2: makes a Light flicker softly, like a magical flame or lantern.
/// Uses Perlin noise so the flicker looks natural instead of random.
/// </summary>
[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float baseIntensity = 2f;     // average brightness
    [SerializeField] private float flickerAmount = 0.8f;   // how much the brightness changes up and down
    [SerializeField] private float flickerSpeed = 2.5f;    // how quickly it flickers

    private Light targetLight;
    private float noiseSeed;

    private void Awake()
    {
        targetLight = GetComponent<Light>();
        noiseSeed = Random.value * 100f;   // each light flickers differently
    }

    private void Update()
    {
        float noise = Mathf.PerlinNoise(noiseSeed, Time.time * flickerSpeed);   // value from 0 to 1
        targetLight.intensity = baseIntensity + (noise - 0.5f) * 2f * flickerAmount;
    }
}
