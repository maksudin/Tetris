//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Input/PieceActions.inputactions
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

public partial class @PieceActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PieceActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PieceActions"",
    ""maps"": [
        {
            ""name"": ""PieceMap"",
            ""id"": ""c2f89c52-de76-41cb-8ecd-16cc6bb7ea7e"",
            ""actions"": [
                {
                    ""name"": ""Horizontal Movement"",
                    ""type"": ""Value"",
                    ""id"": ""df6f1442-97ec-4a1d-af82-c4e592e8ad46"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Button"",
                    ""id"": ""1141be6e-ef28-4a29-afb0-0478573d1efc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""285bbfbf-654a-4fe4-8f0b-746b0cc7af37"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""23adf809-9b97-4d1e-b333-df3589247ea1"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""71c1c442-1f59-4b11-8709-4edf57fe36a3"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0705db3f-cf28-4cf0-b21e-9457869ba9a0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ba38e98d-ad6c-4509-82fb-0bb1e46c53ae"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b968d6cd-ad38-4392-b6d9-f1d7b40c116b"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PieceMap
        m_PieceMap = asset.FindActionMap("PieceMap", throwIfNotFound: true);
        m_PieceMap_HorizontalMovement = m_PieceMap.FindAction("Horizontal Movement", throwIfNotFound: true);
        m_PieceMap_Rotation = m_PieceMap.FindAction("Rotation", throwIfNotFound: true);
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

    // PieceMap
    private readonly InputActionMap m_PieceMap;
    private IPieceMapActions m_PieceMapActionsCallbackInterface;
    private readonly InputAction m_PieceMap_HorizontalMovement;
    private readonly InputAction m_PieceMap_Rotation;
    public struct PieceMapActions
    {
        private @PieceActions m_Wrapper;
        public PieceMapActions(@PieceActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalMovement => m_Wrapper.m_PieceMap_HorizontalMovement;
        public InputAction @Rotation => m_Wrapper.m_PieceMap_Rotation;
        public InputActionMap Get() { return m_Wrapper.m_PieceMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PieceMapActions set) { return set.Get(); }
        public void SetCallbacks(IPieceMapActions instance)
        {
            if (m_Wrapper.m_PieceMapActionsCallbackInterface != null)
            {
                @HorizontalMovement.started -= m_Wrapper.m_PieceMapActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.performed -= m_Wrapper.m_PieceMapActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.canceled -= m_Wrapper.m_PieceMapActionsCallbackInterface.OnHorizontalMovement;
                @Rotation.started -= m_Wrapper.m_PieceMapActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_PieceMapActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_PieceMapActionsCallbackInterface.OnRotation;
            }
            m_Wrapper.m_PieceMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalMovement.started += instance.OnHorizontalMovement;
                @HorizontalMovement.performed += instance.OnHorizontalMovement;
                @HorizontalMovement.canceled += instance.OnHorizontalMovement;
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
            }
        }
    }
    public PieceMapActions @PieceMap => new PieceMapActions(this);
    public interface IPieceMapActions
    {
        void OnHorizontalMovement(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
    }
}
