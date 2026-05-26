using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Action<InputAction.CallbackContext> MoveCameraEvent;
    [SerializeField] private Vector2 moveCameraInput;
    public Vector2 MoveCameraInput => moveCameraInput;
    
    public Action<InputAction.CallbackContext> MouseLBEvent;
    [SerializeField] private bool mouse;
    public bool Mouse => mouse;
    
    public Action<InputAction.CallbackContext> MouseScrollUpEvent;
    [SerializeField] private bool scrollingUp;
    public bool ScrollingUp => scrollingUp;
    
    public Action<InputAction.CallbackContext> MouseScrollDownEvent;
    [SerializeField] private bool scrollingDown;
    public bool ScrollingDown => scrollingDown;
    
    public void MovementCameraInput(InputAction.CallbackContext input)
    {
        moveCameraInput = input.ReadValue<Vector2>();
        MoveCameraEvent?.Invoke(input);
    }
    
    public void MouseLB(InputAction.CallbackContext input)
    {
        if (input.started) mouse = true;
        if (input.canceled) mouse = false;
        MouseLBEvent?.Invoke(input);
    }
    
    public void MouseSUp(InputAction.CallbackContext input)
    {
        if (input.started) scrollingUp = true;
        if (input.canceled) scrollingUp = false;
        MouseScrollUpEvent?.Invoke(input);
    }
    
    public void MouseSDown(InputAction.CallbackContext input)
    {
        if (input.started) scrollingDown = true;
        if (input.canceled) scrollingDown = false;
        MouseScrollDownEvent?.Invoke(input);
    }
}