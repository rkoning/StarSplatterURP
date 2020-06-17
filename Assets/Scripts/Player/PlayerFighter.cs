using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerFighter : MonoBehaviour
{
    [Header("Physics Forces")]
    public float yawTorque;
    public float rollTorque;
    public float pitchTorque;
    public float baseThrustForce;
    public float brakeForce;
    public float thrustForce;

    [Header("Boost")]
    public float[] boostCutoffs;
    public float boostForce;
    private int boostLevel = 0;
    public int BoostLevel {
        get { return boostLevel; }
        set { boostLevel = value; }
    }
    public float boostDelay;
    private float nextBoost;

    private bool boostChanging;
    public bool BoostChanging {
        get { return boostChanging; }
        set { boostChanging = value; }
    }

    [Header("Particle Effects")]
    public ParticleSystem[] boostStartEffects;
    public ParticleSystem[] postBoostEffects;


    [Header("Audio")]
    public AudioSource boostSound;
    public AudioSource engineSound;
    public float minEnginePitch;
    public float maxEnginePitch;
    public float minEngineVolume;
    public float maxEngineVolume;


    private Transform dockTransform;
    public bool IsDocked {
        get { return dockTransform != null; }
    }

    private bool active;
    private bool docking;
    private bool docked;

    private Rigidbody rb;
    public Rigidbody Rigidbody {
        get { return rb; }
    }

    private float throttleInput;

    public PlayerInput playerInput;
    public Player player;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate() {
        if (docked && dockTransform) {
            transform.position = Vector3.Lerp(transform.position, dockTransform.position, Time.fixedDeltaTime * 400f);
            transform.rotation = Quaternion.Lerp(transform.rotation, dockTransform.rotation, Time.fixedDeltaTime * 200f);
            return;
        }

        if (docking) {
            if ((transform.position - dockTransform.position).sqrMagnitude > 2f) {
                transform.position = Vector3.Lerp(transform.position, dockTransform.position, Time.fixedDeltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, dockTransform.rotation, Time.fixedDeltaTime);
            } else {
                docking = false;
                docked = true;
            }
            return;
        }

        float brake = playerInput.BrakeInput;
        if (brake > 0) {
            rb.AddForce((baseThrustForce - brakeForce) * transform.forward);
        } else {
            float force = ((thrustForce + boostForce * BoostLevel) * playerInput.ThrottleInput);
            rb.AddForce((baseThrustForce + force) * transform.forward);
        }

        Vector3 yaw = (yawTorque) * playerInput.YawInput * transform.up;
        Vector3 pitch = (pitchTorque) * playerInput.PitchInput * transform.right;
        Vector3 roll = (rollTorque) * playerInput.RollInput * transform.forward;
        // Vector addition is SLOW so do this instead
        Vector3 torque = new Vector3(
            yaw.x + pitch.x + roll.x,
            yaw.y + pitch.y + roll.y,
            yaw.z + pitch.z + roll.z
        );
        
        rb.AddTorque(torque);

        if (CanBoost()) {
            float now = Time.fixedTime;
            if (!boostChanging) {
                StartAllParticles(boostStartEffects);
                if (playerInput.ThrottleInput < 0.4f) {
                    // if player reduces input to < 0.4 then boost is readied
                    boostChanging = true;
                }
            } else {
                if (playerInput.ThrottleInput > 0.9f) {
                    boostChanging = false;
                    Boost();
                    player.camera.AddStress(1);
                }
            }
        }
    }

    public bool CanBoost() {
        if (rb) {
            return boostLevel < boostCutoffs.Length && rb.velocity.sqrMagnitude > Mathf.Pow(boostCutoffs[boostLevel], 2) && Time.fixedTime > nextBoost;
        } else {
            return false;
        }
    }

    public void Boost() {
        rb.AddForce(boostForce * transform.forward);
        nextBoost = Time.fixedTime + boostDelay;
        boostLevel++;
        StartAllParticles(postBoostEffects);
        if (boostSound && engineSound) {
            boostSound.Play();
            engineSound.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, ((float) boostLevel / (float) boostCutoffs.Length));
            engineSound.volume = Mathf.Lerp(minEngineVolume, maxEngineVolume, ((float) boostLevel / (float) boostCutoffs.Length));
        }
    }

    public void StartAllParticles(ParticleSystem[] particleGroup) {
        foreach (ParticleSystem p in particleGroup) {
            if (!p.isPlaying) {
                p.Play();
            }
        }
    }

    public void StopAllParticles(ParticleSystem[] particleGroup) {
        foreach (ParticleSystem p in particleGroup) {
            if (p.isPlaying) {
                p.Stop();
            }            
        }
    }

    /// <summary>
    /// Sets the dockTransform of the ship, this will cause the ship to follow the given transform 
    /// </summary>
    /// <param name="dockTransform"></param>
    public void Dock(Transform dockTransform) {
        docking = true;
        active = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.isKinematic = true;
        this.dockTransform = dockTransform;
    }

    public void Undock() {
        this.dockTransform = null;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = false;
        active = true;
        docking = false;
        docked = false;
    }
}
