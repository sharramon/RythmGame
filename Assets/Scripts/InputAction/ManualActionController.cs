using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;


public class ManualActionController : MonoBehaviour
{
    float m_ButtonPressPoint = 0.3f;
    private void Update()
    {
        if(ReadValue(gripValue.action) > 0.1)
        {
            Debug.Log(ReadValue(gripValue.action));
            Debug.Log(gripValue.action.ReadValue<float>());
        }
        if (ReadValue(thumbTouch.action) > 0)
            Debug.Log(ReadValue(thumbTouch.action));
        if(triggerValue.action.ReadValue<float>() > 0.1)
            Debug.Log(triggerValue.action.ReadValue<float>());
    }

    private void OnEnable()
    {
        EnableAllActions();
    }
    private void OnDisable()
    {
        DisableAllActions();
    }

    [SerializeField]
    InputActionProperty m_TriggerPress;
    public InputActionProperty triggerPress
    {
        get => m_TriggerPress;
        set => SetInputActionProperty(ref m_TriggerPress, value);
    }

    [SerializeField]
    InputActionProperty m_TriggerValue;
    public InputActionProperty triggerValue
    {
        get => m_TriggerValue;
        set => SetInputActionProperty(ref m_TriggerValue, value);
    }

    [SerializeField]
    InputActionProperty m_TriggerTouch;
    public InputActionProperty triggerTouch
    {
        get => m_TriggerTouch;
        set => SetInputActionProperty(ref m_TriggerTouch, value);
    }

    [SerializeField]
    InputActionProperty m_GripPress;
    public InputActionProperty gripPress
    {
        get => m_GripPress;
        set => SetInputActionProperty(ref m_GripPress, value);
    }

    [SerializeField]
    InputActionProperty m_GripValue;
    public InputActionProperty gripValue
    {
        get => m_GripValue;
        set => SetInputActionProperty(ref m_GripValue, value);
    }

    [SerializeField]
    InputActionProperty m_PrimaryButtonTouch;
    public InputActionProperty primaryButtonTouch
    {
        get => m_PrimaryButtonTouch;
        set => SetInputActionProperty(ref m_PrimaryButtonTouch, value);
    }

    [SerializeField]
    InputActionProperty m_PrimaryButton;
    public InputActionProperty primaryButton
    {
        get => m_PrimaryButton;
        set => SetInputActionProperty(ref m_PrimaryButton, value);
    }

    [SerializeField]
    InputActionProperty m_SecondaryButtonTouch;
    public InputActionProperty secondaryButtonTouch
    {
        get => m_SecondaryButtonTouch;
        set => SetInputActionProperty(ref m_SecondaryButtonTouch, value);
    }

    [SerializeField]
    InputActionProperty m_SecondaryButton;
    public InputActionProperty secondaryButton
    {
        get => m_SecondaryButton;
        set => SetInputActionProperty(ref m_SecondaryButton, value);
    }

    [SerializeField]
    InputActionProperty m_ThumbStickTouch;
    public InputActionProperty thumbStickTouch
    {
        get => m_ThumbStickTouch;
        set => SetInputActionProperty(ref m_ThumbStickTouch, value);
    }

    [SerializeField]
    InputActionProperty m_ThumbStickPressed;
    public InputActionProperty thumbStickPressed
    {
        get => m_ThumbStickPressed;
        set => SetInputActionProperty(ref m_ThumbStickPressed, value);
    }

    [SerializeField]
    InputActionProperty m_ThumbStick;
    public InputActionProperty thumbStick
    {
        get => m_ThumbStick;
        set => SetInputActionProperty(ref m_ThumbStick, value);
    }

    [SerializeField]
    InputActionProperty m_ThumbTouch;
    public InputActionProperty thumbTouch
    {
        get => m_ThumbTouch;
        set => SetInputActionProperty(ref m_ThumbTouch, value);
    }

    bool m_HasCheckedDisabledTrackingInputReferenceActions;
    bool m_HasCheckedDisabledInputReferenceActions;

    /// <summary>
    /// Reads and returns the given action value.
    /// Unity automatically calls this method during <see cref="UpdateInput"/> to determine
    /// the amount or strength of the interaction state this frame.
    /// </summary>
    /// <param name="action">The action to read the value from.</param>
    /// <returns>Returns the action value. If the action is <see langword="null"/> returns the default <see langword="float"/> value (<c>0f</c>).</returns>
    /// <seealso cref="InteractionState.value"/>
    private float ReadValue(InputAction action)
    {
        if (action == null)
            return default;

        if (action.activeControl is AxisControl)
            return action.ReadValue<float>();

        if (action.activeControl is Vector2Control)
            return action.ReadValue<Vector2>().magnitude;

        return IsPressed(action) ? 1f : 0f;
    }

    protected virtual bool IsPressed(InputAction action)
    {
        if (action == null)
            return false;

#if INPUT_SYSTEM_1_1_OR_NEWER || INPUT_SYSTEM_1_1_PREVIEW // 1.1.0-preview.2 or newer, including pre-release
                return action.phase == InputActionPhase.Performed;
#else
        if (action.activeControl is ButtonControl buttonControl)
            return buttonControl.isPressed;

        if (action.activeControl is AxisControl)
            return action.ReadValue<float>() >= m_ButtonPressPoint;

        return action.triggered || action.phase == InputActionPhase.Performed;
#endif
    }

    void EnableAllActions()
    {
        m_TriggerPress.action.Enable();
        m_TriggerValue.action.Enable();
        m_TriggerTouch.action.Enable();
        m_GripPress.action.Enable();
        m_GripValue.action.Enable();
        m_PrimaryButtonTouch.action.Enable();
        m_PrimaryButton.action.Enable();
        m_SecondaryButtonTouch.action.Enable();
        m_SecondaryButton.action.Enable();
        m_ThumbStickTouch.action.Enable();
        m_ThumbStickPressed.action.Enable();
        m_ThumbStick.action.Enable();
        m_ThumbTouch.action.Enable();
    }

    void DisableAllActions()
    {
        m_TriggerPress.action.Disable();
        m_TriggerValue.action.Disable();
        m_TriggerTouch.action.Disable();
        m_GripPress.action.Disable();
        m_GripValue.action.Disable();
        m_PrimaryButtonTouch.action.Disable();
        m_PrimaryButton.action.Disable();
        m_SecondaryButtonTouch.action.Disable();
        m_SecondaryButton.action.Disable();
        m_ThumbStickTouch.action.Disable();
        m_ThumbStickPressed.action.Disable();
        m_ThumbStick.action.Disable();
        m_ThumbTouch.action.Disable();
    }

    void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
    {
        if (Application.isPlaying)
            property.DisableDirectAction();

        property = value;

        if (Application.isPlaying && isActiveAndEnabled)
            property.EnableDirectAction();
    }

    static bool IsDisabledReferenceAction(InputActionProperty property) =>
            property.reference != null && property.reference.action != null && !property.reference.action.enabled;
}

