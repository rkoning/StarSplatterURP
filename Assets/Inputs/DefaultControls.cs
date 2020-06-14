// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/DefaultControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DefaultControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultControls"",
    ""maps"": [
        {
            ""name"": ""FlightControls"",
            ""id"": ""f0a71a19-9199-49a6-b8c0-99c1a8e0f282"",
            ""actions"": [
                {
                    ""name"": ""Pitch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ee7c28ca-b92c-4589-8869-9c2920a60ae3"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fa6dfad1-bb38-4df7-bbe4-b0a8d4fa2e58"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0c301bc2-d223-404d-88a5-5f218680c55b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""688d465e-96af-4454-992f-63d3e31b52fd"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Throttle"",
                    ""type"": ""Value"",
                    ""id"": ""2fedc3ff-42eb-4f33-9c92-9f649a56c255"",
                    ""expectedControlType"": """",
                    ""processors"": ""Clamp(max=1)"",
                    ""interactions"": ""Hold(duration=0.1)""
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Value"",
                    ""id"": ""79328a7c-1bc5-4030-9e98-e4954675b6ce"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold,Press(behavior=1)""
                },
                {
                    ""name"": ""YawLeft"",
                    ""type"": ""Button"",
                    ""id"": ""51987c9d-f7a5-4f17-b201-038551c4dfe9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""YawRight"",
                    ""type"": ""Button"",
                    ""id"": ""0e3c264b-8f7f-4d72-b6d0-34bdd5c2c635"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Primary"",
                    ""type"": ""Button"",
                    ""id"": ""fd9d7ff9-1575-49a2-b29a-6f519dca843e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Secondary"",
                    ""type"": ""Button"",
                    ""id"": ""db993f53-288c-47b7-a91a-fd29ea96bbaa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Equipment"",
                    ""type"": ""Button"",
                    ""id"": ""0c5c2754-43f3-4d92-8f49-912f6fa03ce9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Special"",
                    ""type"": ""Button"",
                    ""id"": ""d995a2b0-5951-4e41-a6f5-f64b5d959870"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""LookReset"",
                    ""type"": ""Button"",
                    ""id"": ""0034b16e-6229-4209-8dd3-86474ae0d418"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0b753552-a818-4e28-8e90-f841b9d245d7"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a466bd79-d187-425e-a5aa-ba979f37b21f"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8c670d1-8b11-4437-a1a7-9ac652f42604"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": ""Invert,Clamp(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56a0312a-66f0-4e2c-9136-53e4eee746cf"",
                    ""path"": ""<Mouse>/scroll/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8dd26ff5-7f84-4dff-b873-9b67ce79a99d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YawLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2df95831-096e-412d-8fd3-da6e1348b4e0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YawLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""481df8b7-3679-47f0-9c87-8adaf070fcb2"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YawRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb077cf0-71d1-4955-ad66-b079517430fe"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YawRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07253d5a-8ef8-4208-8def-6cd72b79500b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9894a17f-eeb1-4a16-a0f4-acde592f564d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd938b59-42a7-419d-9500-a4b7317cd486"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb619ee6-b9be-4447-b834-8970204f2600"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df236501-e0ed-418e-87a8-d2d7d2e8e7d6"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equipment"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f72be94-a83c-47f7-8db0-49fd8c4a569f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equipment"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a99e1802-5acb-4aa7-aa37-8a469bc5a3cd"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88f285c0-0e16-435b-9016-0f6279997d29"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdfde2bf-da73-4944-88df-45f503df393e"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookReset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e881094-9ded-438f-a0c2-1d818103a991"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": ""Tap,Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""RIghtTrigger"",
                    ""id"": ""baea5bf2-4ada-4f8a-a20e-f31c4a9f297b"",
                    ""path"": ""1DAxis(minValue=0,whichSideWins=1)"",
                    ""interactions"": ""Hold,Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throttle"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""dd3e20cb-cb6f-4302-a51d-e3b98d8669e7"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""40d0e52e-148d-4f14-b838-2301158f4ff4"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftTrigger"",
                    ""id"": ""2d5a9573-ce27-48a7-b7f1-f0f39a3eec99"",
                    ""path"": ""1DAxis(minValue=0,whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9b48fad4-3b90-43bf-b650-8384c7517700"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""65faa6f1-5ecb-4fbb-89f0-36b81fcda588"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""31e7d831-1725-4d34-a856-97a1c8c3db57"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd82bbc1-4194-472f-83a9-40ce9a6a7061"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // FlightControls
        m_FlightControls = asset.FindActionMap("FlightControls", throwIfNotFound: true);
        m_FlightControls_Pitch = m_FlightControls.FindAction("Pitch", throwIfNotFound: true);
        m_FlightControls_LookX = m_FlightControls.FindAction("LookX", throwIfNotFound: true);
        m_FlightControls_LookY = m_FlightControls.FindAction("LookY", throwIfNotFound: true);
        m_FlightControls_Roll = m_FlightControls.FindAction("Roll", throwIfNotFound: true);
        m_FlightControls_Throttle = m_FlightControls.FindAction("Throttle", throwIfNotFound: true);
        m_FlightControls_Brake = m_FlightControls.FindAction("Brake", throwIfNotFound: true);
        m_FlightControls_YawLeft = m_FlightControls.FindAction("YawLeft", throwIfNotFound: true);
        m_FlightControls_YawRight = m_FlightControls.FindAction("YawRight", throwIfNotFound: true);
        m_FlightControls_Primary = m_FlightControls.FindAction("Primary", throwIfNotFound: true);
        m_FlightControls_Secondary = m_FlightControls.FindAction("Secondary", throwIfNotFound: true);
        m_FlightControls_Equipment = m_FlightControls.FindAction("Equipment", throwIfNotFound: true);
        m_FlightControls_Special = m_FlightControls.FindAction("Special", throwIfNotFound: true);
        m_FlightControls_LookReset = m_FlightControls.FindAction("LookReset", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // FlightControls
    private readonly InputActionMap m_FlightControls;
    private IFlightControlsActions m_FlightControlsActionsCallbackInterface;
    private readonly InputAction m_FlightControls_Pitch;
    private readonly InputAction m_FlightControls_LookX;
    private readonly InputAction m_FlightControls_LookY;
    private readonly InputAction m_FlightControls_Roll;
    private readonly InputAction m_FlightControls_Throttle;
    private readonly InputAction m_FlightControls_Brake;
    private readonly InputAction m_FlightControls_YawLeft;
    private readonly InputAction m_FlightControls_YawRight;
    private readonly InputAction m_FlightControls_Primary;
    private readonly InputAction m_FlightControls_Secondary;
    private readonly InputAction m_FlightControls_Equipment;
    private readonly InputAction m_FlightControls_Special;
    private readonly InputAction m_FlightControls_LookReset;
    public struct FlightControlsActions
    {
        private @DefaultControls m_Wrapper;
        public FlightControlsActions(@DefaultControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pitch => m_Wrapper.m_FlightControls_Pitch;
        public InputAction @LookX => m_Wrapper.m_FlightControls_LookX;
        public InputAction @LookY => m_Wrapper.m_FlightControls_LookY;
        public InputAction @Roll => m_Wrapper.m_FlightControls_Roll;
        public InputAction @Throttle => m_Wrapper.m_FlightControls_Throttle;
        public InputAction @Brake => m_Wrapper.m_FlightControls_Brake;
        public InputAction @YawLeft => m_Wrapper.m_FlightControls_YawLeft;
        public InputAction @YawRight => m_Wrapper.m_FlightControls_YawRight;
        public InputAction @Primary => m_Wrapper.m_FlightControls_Primary;
        public InputAction @Secondary => m_Wrapper.m_FlightControls_Secondary;
        public InputAction @Equipment => m_Wrapper.m_FlightControls_Equipment;
        public InputAction @Special => m_Wrapper.m_FlightControls_Special;
        public InputAction @LookReset => m_Wrapper.m_FlightControls_LookReset;
        public InputActionMap Get() { return m_Wrapper.m_FlightControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FlightControlsActions set) { return set.Get(); }
        public void SetCallbacks(IFlightControlsActions instance)
        {
            if (m_Wrapper.m_FlightControlsActionsCallbackInterface != null)
            {
                @Pitch.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnPitch;
                @Pitch.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnPitch;
                @Pitch.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnPitch;
                @LookX.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookX;
                @LookX.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookX;
                @LookX.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookX;
                @LookY.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookY;
                @LookY.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookY;
                @LookY.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookY;
                @Roll.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnRoll;
                @Throttle.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnThrottle;
                @Throttle.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnThrottle;
                @Throttle.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnThrottle;
                @Brake.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnBrake;
                @Brake.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnBrake;
                @Brake.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnBrake;
                @YawLeft.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnYawLeft;
                @YawLeft.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnYawLeft;
                @YawLeft.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnYawLeft;
                @YawRight.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnYawRight;
                @YawRight.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnYawRight;
                @YawRight.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnYawRight;
                @Primary.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnPrimary;
                @Primary.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnPrimary;
                @Primary.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnPrimary;
                @Secondary.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnSecondary;
                @Secondary.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnSecondary;
                @Secondary.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnSecondary;
                @Equipment.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnEquipment;
                @Equipment.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnEquipment;
                @Equipment.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnEquipment;
                @Special.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnSpecial;
                @Special.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnSpecial;
                @Special.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnSpecial;
                @LookReset.started -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookReset;
                @LookReset.performed -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookReset;
                @LookReset.canceled -= m_Wrapper.m_FlightControlsActionsCallbackInterface.OnLookReset;
            }
            m_Wrapper.m_FlightControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pitch.started += instance.OnPitch;
                @Pitch.performed += instance.OnPitch;
                @Pitch.canceled += instance.OnPitch;
                @LookX.started += instance.OnLookX;
                @LookX.performed += instance.OnLookX;
                @LookX.canceled += instance.OnLookX;
                @LookY.started += instance.OnLookY;
                @LookY.performed += instance.OnLookY;
                @LookY.canceled += instance.OnLookY;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Throttle.started += instance.OnThrottle;
                @Throttle.performed += instance.OnThrottle;
                @Throttle.canceled += instance.OnThrottle;
                @Brake.started += instance.OnBrake;
                @Brake.performed += instance.OnBrake;
                @Brake.canceled += instance.OnBrake;
                @YawLeft.started += instance.OnYawLeft;
                @YawLeft.performed += instance.OnYawLeft;
                @YawLeft.canceled += instance.OnYawLeft;
                @YawRight.started += instance.OnYawRight;
                @YawRight.performed += instance.OnYawRight;
                @YawRight.canceled += instance.OnYawRight;
                @Primary.started += instance.OnPrimary;
                @Primary.performed += instance.OnPrimary;
                @Primary.canceled += instance.OnPrimary;
                @Secondary.started += instance.OnSecondary;
                @Secondary.performed += instance.OnSecondary;
                @Secondary.canceled += instance.OnSecondary;
                @Equipment.started += instance.OnEquipment;
                @Equipment.performed += instance.OnEquipment;
                @Equipment.canceled += instance.OnEquipment;
                @Special.started += instance.OnSpecial;
                @Special.performed += instance.OnSpecial;
                @Special.canceled += instance.OnSpecial;
                @LookReset.started += instance.OnLookReset;
                @LookReset.performed += instance.OnLookReset;
                @LookReset.canceled += instance.OnLookReset;
            }
        }
    }
    public FlightControlsActions @FlightControls => new FlightControlsActions(this);
    public interface IFlightControlsActions
    {
        void OnPitch(InputAction.CallbackContext context);
        void OnLookX(InputAction.CallbackContext context);
        void OnLookY(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnThrottle(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnYawLeft(InputAction.CallbackContext context);
        void OnYawRight(InputAction.CallbackContext context);
        void OnPrimary(InputAction.CallbackContext context);
        void OnSecondary(InputAction.CallbackContext context);
        void OnEquipment(InputAction.CallbackContext context);
        void OnSpecial(InputAction.CallbackContext context);
        void OnLookReset(InputAction.CallbackContext context);
    }
}
