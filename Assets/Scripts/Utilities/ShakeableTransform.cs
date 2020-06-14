using UnityEngine;

public class ShakeableTransform : MonoBehaviour
{
    [SerializeField]
    float frequency = 25;
    private float seed;

    [SerializeField]
    Vector3 maximumTranslationShake = Vector3.one * 0.5f;
    [SerializeField]
    Vector3 maximumAngularShake = Vector3.one * 2;
    
    [SerializeField]
    float recoverySpeed = 2.5f;

    [SerializeField]
    float traumaExponent = 2;

    private float trauma = 0;

    private void Awake() {
        seed = Random.value;
    }

    private void Update()
    {

        float shake = Mathf.Pow(trauma, traumaExponent);

        transform.localPosition = new Vector3(
            maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
        ) * shake;

        transform.localRotation = Quaternion.Euler(new Vector3(
            maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
        ) * shake);

        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
    }

    public void InduceStress(float stress) {
        trauma = Mathf.Clamp01(trauma + stress);
    }
}