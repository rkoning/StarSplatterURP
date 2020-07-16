using UnityEngine;

public class LocationContext : MonoBehaviour
{
    public Transform scaled;
    public Transform real;
    public float scale;

    public GameObject enterTrigger;
    public GameObject exitTrigger;

    private void Start() {
        scale = OriginManager.skyboxScale;
        enterTrigger.SetActive(true);
        exitTrigger.SetActive(false);    
    }

    public void LoadLocation() {
        real.gameObject.SetActive(true);
        scaled.gameObject.SetActive(false);
        OriginManager.instance.EnterLocation(this);
        enterTrigger.SetActive(false);
        exitTrigger.SetActive(true);
    }
    
    public void UnloadLocation() {
        real.gameObject.SetActive(false);
        scaled.gameObject.SetActive(true);
        OriginManager.instance.ExitLocation(this);
        enterTrigger.SetActive(true);
        exitTrigger.SetActive(false);
    }

    public Vector3 GetScaledPoint(Vector3 realPoint) {
        Vector3 relative = real.InverseTransformPoint(realPoint); // realPoint's position relative to the real root
        return (scaled.position + scaled.rotation * relative / scale);
    }

    public Quaternion GetScaledRotation(Transform t) {
        var relative = Quaternion.Inverse(real.rotation) * t.rotation;
        return scaled.rotation * relative;
    }
}
