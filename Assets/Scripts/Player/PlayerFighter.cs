using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerFighter : Ship
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

    public ParticleSystem speedIndicator;
    public float maxSpeedScale = 3f;

    [Header("Audio")]
    public AudioSource boostSound;
    public AudioSource engineSound;
    public float minEnginePitch;
    public float maxEnginePitch;
    public float minEngineVolume;
    public float maxEngineVolume;

    private bool active;
    private bool docking;

    private float throttleInput;

    public PlayerInput playerInput;
    public Player player;

    public float aimAssistRadius = 5f;
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


        speedIndicator.transform.localScale = Vector3.one 
            * Mathf.Clamp01(rb.velocity.sqrMagnitude / Mathf.Pow(boostCutoffs[BoostLevel], 2))
            * maxSpeedScale;

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

        if (boostLevel > 0 && rb.velocity.sqrMagnitude < Mathf.Pow(boostCutoffs[BoostLevel - 1], 2)) {
            boostLevel -= 1;
        }

        /**
         * Weapons
         */

        if (playerInput.PrimaryFireInput || playerInput.SecondaryFireInput || playerInput.EquipmentInput) {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, aimAssistRadius, transform.forward, out hit)) {
                var h = hit.collider.GetComponent<Health>();
                if (h != null) {
                    if (primary) {
                        AimWeaponAt(primary.transform, hit.point);
                    }
                    if (secondary) {
                        AimWeaponAt(secondary.transform, hit.point);
                    }
                    if (equipment) {
                        AimWeaponAt(equipment.transform, hit.point);
                    }
                }
            } else {
                if (primary) {
                    primary.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                }
                if (secondary) {
                    secondary.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                }
                if (equipment) {
                    equipment.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                }
            }

            if (primary) {
                if (playerInput.PrimaryFireDown) {
                    primary.Fire();
                } else if (playerInput.PrimaryFireHeld) {
                    primary.Hold();
                } else if (playerInput.PrimaryFireReleased) {
                    primary.Release();
                }
            }

            if (playerInput.SecondaryFireInput && secondary) {
                if (playerInput.SecondaryFireDown) {
                    secondary.Fire();
                } else if (playerInput.SecondaryFireHeld) {
                    secondary.Hold();
                } else if (playerInput.SecondaryFireReleased) {
                    secondary.Release();
                }
            }

            if (playerInput.EquipmentInput && equipment) {
                if (playerInput.EquipmentDown) {
                    equipment.Fire();
                } else if (playerInput.EquipmentHeld) {
                    equipment.Hold();
                } else if (playerInput.EquipmentReleased) {
                    equipment.Release();
                }
            }
        }
    }

    private void AimWeaponAt(Transform weaponTransform, Vector3 point) {
        Vector3 direction = (point - weaponTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        weaponTransform.rotation = lookRotation;
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
