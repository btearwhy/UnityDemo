//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Demo/Input/InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""id"": ""bb0dd247-00c1-4bfb-8daf-ffb69ce21ddb"",
            ""actions"": [
                {
                    ""name"": ""CameraControl"",
                    ""type"": ""Value"",
                    ""id"": ""b101f708-e165-4248-94f4-76e9b568fcaa"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""36edc8ac-f4b4-4af4-a549-ac5d4ddfc94d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""d9dd149a-4933-4275-9a79-4f7720575a44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Tap"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Absorb"",
                    ""type"": ""Button"",
                    ""id"": ""00f518c8-43db-4590-9a9b-bd4bac4d6c53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Tap"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""a989076c-23af-47e5-aa7b-aff09b017c5d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""49453465-2e71-4663-9669-0b490cebb1b6"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraControl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""b10d5bf1-c6e5-4e5b-ab36-58033830b942"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b080542a-9d04-4759-9213-07225d73e438"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c3e01c24-7c61-48bf-9cef-5217ed7bb7ee"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0d1cbaba-7705-41a9-a520-68b87f94e4a9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9a8b21b1-afdf-458c-9ddb-81f3476d5e37"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1388d644-55be-4249-82c9-987f99a495c1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ebe685c-e70b-43d6-b556-e610d8885357"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f3bc9dcb-847a-4f71-b3b0-f96b533be7e2"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Absorb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9607bd2c-4d48-4fef-b43f-383019da9ec6"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Mobile"",
            ""id"": ""44cf42b4-7ad8-46f1-84f0-ea024b0b64eb"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""49c4b8c1-fa5d-47fe-b519-0c9908ab46fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""eb317e49-be8d-42db-99a6-1610db66c9c6"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keyboard and Mouse
        m_KeyboardandMouse = asset.FindActionMap("Keyboard and Mouse", throwIfNotFound: true);
        m_KeyboardandMouse_CameraControl = m_KeyboardandMouse.FindAction("CameraControl", throwIfNotFound: true);
        m_KeyboardandMouse_Movement = m_KeyboardandMouse.FindAction("Movement", throwIfNotFound: true);
        m_KeyboardandMouse_Attack = m_KeyboardandMouse.FindAction("Attack", throwIfNotFound: true);
        m_KeyboardandMouse_Absorb = m_KeyboardandMouse.FindAction("Absorb", throwIfNotFound: true);
        m_KeyboardandMouse_Aim = m_KeyboardandMouse.FindAction("Aim", throwIfNotFound: true);
        // Mobile
        m_Mobile = asset.FindActionMap("Mobile", throwIfNotFound: true);
        m_Mobile_Newaction = m_Mobile.FindAction("New action", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Keyboard and Mouse
    private readonly InputActionMap m_KeyboardandMouse;
    private List<IKeyboardandMouseActions> m_KeyboardandMouseActionsCallbackInterfaces = new List<IKeyboardandMouseActions>();
    private readonly InputAction m_KeyboardandMouse_CameraControl;
    private readonly InputAction m_KeyboardandMouse_Movement;
    private readonly InputAction m_KeyboardandMouse_Attack;
    private readonly InputAction m_KeyboardandMouse_Absorb;
    private readonly InputAction m_KeyboardandMouse_Aim;
    public struct KeyboardandMouseActions
    {
        private @InputActions m_Wrapper;
        public KeyboardandMouseActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraControl => m_Wrapper.m_KeyboardandMouse_CameraControl;
        public InputAction @Movement => m_Wrapper.m_KeyboardandMouse_Movement;
        public InputAction @Attack => m_Wrapper.m_KeyboardandMouse_Attack;
        public InputAction @Absorb => m_Wrapper.m_KeyboardandMouse_Absorb;
        public InputAction @Aim => m_Wrapper.m_KeyboardandMouse_Aim;
        public InputActionMap Get() { return m_Wrapper.m_KeyboardandMouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardandMouseActions set) { return set.Get(); }
        public void AddCallbacks(IKeyboardandMouseActions instance)
        {
            if (instance == null || m_Wrapper.m_KeyboardandMouseActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_KeyboardandMouseActionsCallbackInterfaces.Add(instance);
            @CameraControl.started += instance.OnCameraControl;
            @CameraControl.performed += instance.OnCameraControl;
            @CameraControl.canceled += instance.OnCameraControl;
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Absorb.started += instance.OnAbsorb;
            @Absorb.performed += instance.OnAbsorb;
            @Absorb.canceled += instance.OnAbsorb;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
        }

        private void UnregisterCallbacks(IKeyboardandMouseActions instance)
        {
            @CameraControl.started -= instance.OnCameraControl;
            @CameraControl.performed -= instance.OnCameraControl;
            @CameraControl.canceled -= instance.OnCameraControl;
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Absorb.started -= instance.OnAbsorb;
            @Absorb.performed -= instance.OnAbsorb;
            @Absorb.canceled -= instance.OnAbsorb;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
        }

        public void RemoveCallbacks(IKeyboardandMouseActions instance)
        {
            if (m_Wrapper.m_KeyboardandMouseActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IKeyboardandMouseActions instance)
        {
            foreach (var item in m_Wrapper.m_KeyboardandMouseActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_KeyboardandMouseActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public KeyboardandMouseActions @KeyboardandMouse => new KeyboardandMouseActions(this);

    // Mobile
    private readonly InputActionMap m_Mobile;
    private List<IMobileActions> m_MobileActionsCallbackInterfaces = new List<IMobileActions>();
    private readonly InputAction m_Mobile_Newaction;
    public struct MobileActions
    {
        private @InputActions m_Wrapper;
        public MobileActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Mobile_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Mobile; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MobileActions set) { return set.Get(); }
        public void AddCallbacks(IMobileActions instance)
        {
            if (instance == null || m_Wrapper.m_MobileActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MobileActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        private void UnregisterCallbacks(IMobileActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        public void RemoveCallbacks(IMobileActions instance)
        {
            if (m_Wrapper.m_MobileActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMobileActions instance)
        {
            foreach (var item in m_Wrapper.m_MobileActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MobileActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MobileActions @Mobile => new MobileActions(this);
    public interface IKeyboardandMouseActions
    {
        void OnCameraControl(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnAbsorb(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
    public interface IMobileActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
