using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    private PlayerInputActions playerInput;

    public PlayerInputActions PlayerInput => playerInput;
    public static InputManager Instance => instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerInput.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerLook()
    {
        return playerInput.Player.Look.ReadValue<Vector2>();
    }

    public bool GetPlayerFire()
    {
        return playerInput.Player.Fire.ReadValue<float>() >= 0.5;
    }

    public bool GetPlayerRun()
    {
        return playerInput.Player.Run.ReadValue<float>() >= 0.5;
    }
}
