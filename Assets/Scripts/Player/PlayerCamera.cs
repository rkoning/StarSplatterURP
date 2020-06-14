using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, ICameraBehaviour
{
    
    [Header("Follow")]
    private Transform target;
    public Transform Target { set { target = value; } }

    public Vector3 offset;

    public float smoothTime;

    private bool boosting;
    public bool Boosting { set { boosting = value; } }

    private Vector3 velocity;
    public Camera Camera {
        get { return camera; }
    }

    public float minFOV;
    public float maxFOV;
    private float currentFOV;
    public float FOVChangeTime;

    private bool looking;

    public bool Looking {
        set { looking = value; }
    }

    private float lookX;

    private float lookY;

    private float lookReset;
    public float LookReset {
        set { lookReset = value; }
    }

    public Player player;

    private Vector3[] thirds;

    [SerializeField]
    private ParticleSystem speedLines;
    
    public float rotationSpeed;
    public float lookRotationSpeed;
    public float lookResetDelay;
    private float lookEnd = 0f;

    private Vector3 convergePoint;

    private ShakeableTransform cameraShake;

    private Rigidbody targetRb;
    private Vector3 lastTargetVelocity = new Vector3(-0.1f, -0.1f, 1f);

    private float x = 0f;
    private float y = 0f;

    new public Camera camera;

    private Transform lockTransform;

    private bool active;

    void Start() {
        currentFOV = minFOV;
        if (camera == null) {
            Debug.LogWarning("Warning: camera of " + name + " not set...");
        }
        
        cameraShake = GetComponentInChildren<ShakeableTransform>();
    }

    void FixedUpdate() {
        if (!active) {
            return;
        }

        if (lockTransform) {
            transform.position = lockTransform.position;
            transform.rotation = lockTransform.rotation;
            return;
        }

        if (target) {
            // get player look inputs
            lookY = player.input.LookYInput;
            lookX = player.input.LookXInput;
            
            // check if the player has hit the Reset button, or it has been an amount of time greater than the lookResetDelay since there was an input, revert to follow camera
            if (player.input.LookResetDown || (lookY < 0.1f && lookX < 0.1f && Time.fixedTime > lookEnd))
            {
                looking = false;
                x = 0;
                y = 0;
            }

            // if there is a look input, set looking to true and reset the lookEnd time.
            if (lookX != 0 || lookY != 0)
            {
                looking = true;
                lookEnd = Time.fixedTime + lookResetDelay;
            }

            if (looking)
            {
                // if the player is looking, add the input values to the x and y cordinates that will be added to the current rotation
                x += lookX * lookRotationSpeed;
                y += lookY * lookRotationSpeed;

                // multipling an euler to the target's current rotation will rotate the camera relative to the target.
                Quaternion rotation = target.rotation * Quaternion.Euler(y, x, 0);


                // Slerp the rotation for a smoothed camera
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);

                // position should not be lerped, as to always keep up with the target.
                Vector3 position = rotation * offset + target.position;
                transform.position = position;
            } else {
                x = 0f;
                y = 0f;

                // since we are looking slightyly to the side of the target, point the camera inwards a bit
                convergePoint = target.position + target.forward * 1000f;

                if (targetRb == null) {
                    targetRb = target.GetComponent<Rigidbody>();
                }
                
                // based on the target's relative velocity to itself, compute the target position of our camera to slightly lag behind the ship
                Vector3 targetVelocity = -target.InverseTransformDirection(targetRb.velocity.normalized);
                if (Mathf.Abs(targetVelocity.y) > 0.25f) {
                    lastTargetVelocity.y = Mathf.Lerp(lastTargetVelocity.y, targetVelocity.y, Time.fixedDeltaTime);
                }

                if (Mathf.Abs(targetVelocity.x) > 0.25f) {
                    lastTargetVelocity.x = Mathf.Lerp(lastTargetVelocity.x, targetVelocity.x, Time.fixedDeltaTime);
                }

                // SmoothDamp from our current position to the position with the desired offset.
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    target.position + target.forward * offset.z + target.up * offset.y * 1 + target.right * lastTargetVelocity.x * offset.x,
                    ref velocity,
                    smoothTime
                );

                // lerp the camera's rotation to point to the convergence point smoothly.
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(convergePoint - transform.position, target.up), Time.fixedDeltaTime * rotationSpeed);
            }

            if (boosting) {
                // speedLines.Play();
                currentFOV = Mathf.Lerp(currentFOV, maxFOV, Time.deltaTime / FOVChangeTime);
                camera.fieldOfView = currentFOV;
            } else {
                // speedLines.Stop();
                currentFOV = Mathf.Lerp(currentFOV, minFOV, Time.deltaTime / FOVChangeTime);
            }
            camera.fieldOfView = currentFOV;
        }
    }

    public void GrantControl(CameraController cameraController) {
        camera = cameraController.MainCamera;
        player = cameraController.Player;
        active = true;
    }
    
    public void RemoveControl() {
        active = false;
        camera = null;   
        player = null; 
    }

    public bool IsActiveCamera() {
        return active;
    }

    static float ClampAngle(float angle, float min, float max) {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp (angle, min, max);
    }
    public void AddStress(float stress) {
        cameraShake.InduceStress(stress);
    }

    public void LockToTransform(Transform t) {
        lockTransform = t;
    }

    public void Unlock() {
        lockTransform = null;
    }
}