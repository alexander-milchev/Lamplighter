using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput instance;
    public event EventHandler OnJump;
    public event EventHandler OnDash;
    public event EventHandler OnLantern;
    public event EventHandler OnIntensityUp;
    public event EventHandler CancelIntensityUp;
    public event EventHandler OnIntensityDown;
    public event EventHandler CancelIntensityDown;
    public event EventHandler OnEscape;
    public event EventHandler OnEndLevel;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        SingletonPattern();
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        inputActions.Player.Jump.performed += Jump_Performed;
        inputActions.Player.Dash.performed += Dash_Performed;
        inputActions.Player.Lantern.performed += Lantern_Performed;
        inputActions.Player.IntensityUp.performed += IntensityUpPerformed;
        inputActions.Player.IntensityUp.canceled += IntensityUpCancelled;
        inputActions.Player.IntensityDown.performed += IntensityDownPerformed;
        inputActions.Player.IntensityDown.canceled += IntensityDownCancelled;
        inputActions.Player.Escape.performed += EscapePerformed;
        inputActions.Player.Lantern.performed += EndLevelPerformed;
    }

    private void SingletonPattern()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        inputActions.Disable();

        inputActions.Player.Jump.performed -= Jump_Performed;
        inputActions.Player.Dash.performed -= Dash_Performed;
        inputActions.Player.Lantern.performed -= Lantern_Performed;
        inputActions.Player.IntensityUp.performed -= IntensityUpPerformed;
        inputActions.Player.IntensityUp.canceled -= IntensityUpCancelled;
        inputActions.Player.IntensityDown.performed -= IntensityDownPerformed;
        inputActions.Player.IntensityDown.canceled -= IntensityDownCancelled;
        inputActions.Player.Escape.performed -= EscapePerformed;
    }

    private void Jump_Performed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_Performed(InputAction.CallbackContext context)
    {
        OnDash?.Invoke(this, EventArgs.Empty);
    }
    private void Lantern_Performed(InputAction.CallbackContext context)
    {
        OnLantern?.Invoke(this, EventArgs.Empty);
    }

    private void IntensityUpPerformed(InputAction.CallbackContext context)
    {
        OnIntensityUp?.Invoke(this, EventArgs.Empty);
    }

    private void IntensityUpCancelled(InputAction.CallbackContext context)
    {
        CancelIntensityUp?.Invoke(this, EventArgs.Empty);
    }

    private void IntensityDownPerformed(InputAction.CallbackContext context)
    {
        OnIntensityDown?.Invoke(this, EventArgs.Empty);
    }

    private void IntensityDownCancelled(InputAction.CallbackContext context)
    {
        CancelIntensityDown?.Invoke(this, EventArgs.Empty);
    }

    private void EscapePerformed(InputAction.CallbackContext context)
    {
        OnEscape?.Invoke(this, EventArgs.Empty);
    }

    private void EndLevelPerformed(InputAction.CallbackContext context)
    {
        OnEndLevel?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMoveVector()
    {
        Vector2 moveVector = inputActions.Player.Move.ReadValue<Vector2>();
        return moveVector;
    }
}