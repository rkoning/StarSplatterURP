using UnityEngine;
using UnityEngine.SceneManagement;
public class FloatingOrigin : MonoBehaviour
{
    public Transform viewer;
    
    public float threshold;
    
    public Vector3 truePosition;

    private float defaultSleepThreshold = 0.14f;

    public LayerMask scaledLayers;
    
    public float scale;

    public LayerMask ignoredLayers;

    ParticleSystem.Particle[] parts = null;

    private void FixedUpdate() {
        if (viewer.position.sqrMagnitude > threshold * threshold) {
            Vector3 viewerPosition = viewer.position;
            truePosition.x += viewer.position.x;
            truePosition.y += viewer.position.y;
            truePosition.z += viewer.position.z;


            for (int i = 0, count = SceneManager.sceneCount; i < count; i++) {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.IsValid()) {
                    GameObject[] sceneRoots = loadedScene.GetRootGameObjects();
                    for (int j = 0, goCount = sceneRoots.Length; j < goCount; j++) {
                        if (scaledLayers == (scaledLayers | 1 << sceneRoots[j].layer)) {
                            sceneRoots[j].transform.position -= viewerPosition / scale;
                        } else if (ignoredLayers == (ignoredLayers | 1 << sceneRoots[j].layer)) { 
                            continue;
                        } else {
                            sceneRoots[j].transform.position -= viewerPosition;
                        }
                    } 
                }
            }

            var objects = FindObjectsOfType<Rigidbody>();
            foreach (Rigidbody rb in objects) {
                if (rb.transform.position.sqrMagnitude > threshold * threshold) {
                    rb.sleepThreshold = float.MaxValue;
                } else {
                    rb.sleepThreshold = defaultSleepThreshold;
                }
            }

            var particles = FindObjectsOfType<ParticleSystem>();
            foreach (ParticleSystem ps in particles) {
                if (ps.main.simulationSpace != ParticleSystemSimulationSpace.World)
                    continue;

                int particleCount = ps.main.maxParticles;
                if (particleCount <= 0)
                    continue;

                bool wasPaused = ps.isPaused;
                bool wasPlaying = ps.isPlaying;

                if (!wasPaused) {
                    ps.Pause();
                }

                if (parts != null || parts.Length < particleCount) {
                    parts = new ParticleSystem.Particle[particleCount];
                }

                int num = ps.GetParticles(parts);
                for (int i = 0; i < num; i++) {
                    parts[i].position -= viewerPosition;
                }

                ps.SetParticles(parts, num);

                if (wasPlaying)
                    ps.Play();
            }
        }
    }

    public void SetPosition(Transform target, Vector3 position) {
        var rb = target.GetComponent<Rigidbody>(); 
        if (rb) {
            if (rb.transform.position.sqrMagnitude > threshold * threshold) {
                rb.sleepThreshold = float.MaxValue;
            } else {
                rb.sleepThreshold = defaultSleepThreshold;
            }
        }
        target.position = position;
    }

}
