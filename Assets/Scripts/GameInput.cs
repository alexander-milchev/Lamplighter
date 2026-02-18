using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput instance;
    public event EventHandler OnJump;
    public event EventHandler OnDash;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        SingletonPattern();
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        inputActions.Player.Jump.performed += Jump_Performed;
        inputActions.Player.Dash.performed += Dash_Performed;
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

    private void Jump_Performed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_Performed(InputAction.CallbackContext context)
    {
        OnDash?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMoveVector()
    {
        Vector2 moveVector = inputActions.Player.Move.ReadValue<Vector2>();
        return moveVector;
    }
}