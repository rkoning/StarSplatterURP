using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    private ButtonState yawRight;
    public bool YawRightDown { get { return yawRight.Down; } }
    public bool YawRightHeld { get { return yawRight.Held; } }
    public bool YawRightUp { get { return yawRight.Up; } }
    public bool YawRightReleased { get { return yawRight.Released; } }
    public bool YawRightInput { get { return yawRight.Down || yawRight.Held || primaryFire.Released; } }

    private ButtonState yawLeft;
    public bool YawLeftDown { get { return yawLeft.Down; } }
    public bool YawLeftHeld { get { return yawLeft.Held; } }
    public bool YawLeftUp { get { return yawLeft.Up; } }
    public bool YawLeftReleased { get { return yawLeft.Released; } }
    public bool YawLeftInput { get { return yawLeft.Down || yawLeft.Held || yawLeft.Released; } }


    private int yawInput;
    public int YawInput { get { return yawInput; } }

    private float brakeInput;
    public float BrakeInput { get { return brakeInput; } }

    private float rollInput;
    public float RollInput { get { return rollInput; } }

    private float pitchInput;
    public float PitchInput { get { return pitchInput; } }

    private float throttleInput;
    public float ThrottleInput { get { return throttleInput; } }

    private ButtonState primaryFire;
    
    public bool PrimaryFireDown { get { return primaryFire.Down; } }
    public bool PrimaryFireHeld { get { return primaryFire.Held; } }
    public bool PrimaryFireUp { get { return primaryFire.Up; } }
    public bool PrimaryFireReleased { get { return primaryFire.Released; } }
    public bool PrimaryFireInput { get { return primaryFire.Down || primaryFire.Held || primaryFire.Released; } }

    private ButtonState secondaryFire;
    
    public bool SecondaryFireDown { get { return secondaryFire.Down; } }
    public bool SecondaryFireHeld { get { return secondaryFire.Held; } }
    public bool SecondaryFireUp { get { return secondaryFire.Up; } }
    public bool SecondaryFireReleased { get { return secondaryFire.Released; } }
    public bool SecondaryFireInput { get { return secondaryFire.Down || secondaryFire.Held || secondaryFire.Released; } }

    private ButtonState interact;

    public bool InteractDown { get { return interact.Down; } }
    public bool InteractHeld { get { return interact.Held; } }
    public bool InteractUp { get { return interact.Up; } }
    public bool InteractReleased { get { return interact.Released; } }
    public bool InteractInput { get { return interact.Down || interact.Held || interact.Released; } }


    private ButtonState equipment;
    public bool EquipmentDown { get { return equipment.Down; } }
    public bool EquipmentHeld { get { return equipment.Held; } }
    public bool EquipmentUp { get { return equipment.Up; } }
    public bool EquipmentReleased { get { return equipment.Released; } }
    public bool EquipmentInput { get { return equipment.Down || equipment.Held || equipment.Released; } }

    private ButtonState special;
    public bool SpecialDown { get { return special.Down; } }
    public bool SpecialHeld { get { return special.Held; } }
    public bool SpecialUp { get { return special.Up; } }
    public bool SpecialReleased { get { return special.Released; } }
    public bool SpecialInput { get { return special.Down || special.Held || special.Released; } }

    private float lookXInput;
    public float LookXInput { get { return lookXInput; } }
    private float lookYInput;
    public float LookYInput { get { return lookYInput; } }

    private ButtonState lookReset;
    public bool LookResetDown { get { return lookReset.Down; } }
    public bool LookResetHeld { get { return lookReset.Held; } }
    public bool LookResetUp { get { return lookReset.Up; } }
    public bool LookResetReleased { get { return lookReset.Released; } }
    public bool LookResetInput { get { return lookReset.Down || lookReset.Held || lookReset.Released; } }


    private DefaultControls inputActions;
    private void Awake() {
        inputActions = new DefaultControls();
        primaryFire = new ButtonState(inputActions.FlightControls.Primary);
        secondaryFire = new ButtonState(inputActions.FlightControls.Secondary);
        equipment = new ButtonState(inputActions.FlightControls.Equipment);
        special = new ButtonState(inputActions.FlightControls.Special);

        
        yawRight = new ButtonState(inputActions.FlightControls.YawRight);
        yawLeft = new ButtonState(inputActions.FlightControls.YawLeft);

        lookReset = new ButtonState(inputActions.FlightControls.LookReset);
    }   

    private void OnEnable() {
        inputActions.Enable();
    }   

    private void OnDisable() {
        inputActions.Disable();
    }

    void Update() {
        if (YawRightDown) {
            yawInput = 1;
        } else if (YawLeftDown) {
            yawInput = -1;
        } else {
            yawInput = 0;
        }

        rollInput = inputActions.FlightControls.Roll.ReadValue<float>();
        pitchInput = inputActions.FlightControls.Pitch.ReadValue<float>();
        throttleInput = inputActions.FlightControls.Throttle.ReadValue<float>();
        brakeInput = inputActions.FlightControls.Brake.ReadValue<float>();
        lookXInput = inputActions.FlightControls.LookX.ReadValue<float>();
        lookYInput = inputActions.FlightControls.LookY.ReadValue<float>();
    }

    private float ClampFloat(float min, float max, float inputValue) {
        if (inputValue < min) {
            return min;
        } else if (inputValue > max) {
            return max;
        } else {
            return inputValue;
        }
    }

    private class ButtonState {
        private InputAction action;
        private bool down;
        private bool held;
        private bool up;
        private bool released;
        
        private float holdStart;
        private float releasedAt;


        private bool last;

        public bool Down { get { return down; } }
        public bool Held { get { return held; } }
        public float HoldStart { get { return holdStart; } }
        
        public bool Up { get { return up; } }
        public bool Released { get { return released; } }
        public float ReleasedAt { get { return releasedAt; } }
        


        public ButtonState(InputAction action) {
            this.action = action;

            this.action.started += (ctx) => {
                down = true;
                up = false;
                held = true;
                holdStart = Time.fixedTime;
                released = false;
            };

            this.action.canceled += (ctx) => {
                down = false;
                up = true;
                held = false;
                holdStart = Mathf.Infinity;
                releasedAt = Time.fixedTime;
                released = true;
            };
        }
    }
}
