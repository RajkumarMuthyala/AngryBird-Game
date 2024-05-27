using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    private InputAction _mousePositioinAction;
    private InputAction _mouseAction;
    public static Vector2 MousePosition;
    public static bool WasLeftMouseButtonPressed;
    public static bool WasLeftMouseButtonReleased;
    public static bool IsLeftMousePressed;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _mousePositioinAction =  PlayerInput.actions["MousePosition"];
        _mouseAction = PlayerInput.actions["Mouse"];

    }

    private void Update()
    {
        MousePosition = _mousePositioinAction.ReadValue<Vector2>();

        WasLeftMouseButtonPressed = _mouseAction.WasPressedThisFrame();
        WasLeftMouseButtonReleased = _mouseAction.WasReleasedThisFrame();
        IsLeftMousePressed = _mouseAction.IsPressed();
    }
}
