using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationContext : MonoBehaviour
{
    public string sceneName;
    private int buildIndex = -1;
    private AsyncOperation loading;

    public Transform scaled;
    public Transform real;
    public float scale;

    public GameObject enterTrigger;
    public GameObject exitTrigger;

    private void Start() {
        enterTrigger.SetActive(true);
        exitTrigger.SetActive(false);    
    }

    public void LoadLocation() {
        var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // load scene async
        loading.completed += (AsyncOperation op) => {
            if (buildIndex < 0) {
                buildIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
            }

            OriginManager.instance.EnterLocation(this);
            enterTrigger.SetActive(false);
            exitTrigger.SetActive(true);
        };
    }
    
    public void UnloadLocation() {
        OriginManager.instance.ExitLocation(this);
        SceneManager.UnloadSceneAsync(buildIndex);

        enterTrigger.SetActive(true);
        exitTrigger.SetActive(false);
    }

    public Vector3 GetScaledPoint(Vector3 realPoint) {
        Vector3 relative = real.InverseTransformPoint(realPoint); // realPoint's position relative to the real root
        return scaled.rotation *  (scaled.position + relative / scale);
    }

    public Quaternion GetScaledRotation(Transform t) {
        Vector3 direction = (real.forward - t.forward).normalized;
        Vector3 scaledDir = scaled.rotation * direction;
        // Debug.Log(scaledDir);
        return Quaternion.LookRotation(scaledDir);
    }
}
