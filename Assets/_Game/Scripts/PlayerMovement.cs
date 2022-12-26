using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 2.5f, runSpeed = 5f;

    private float currentSpeed;
    private bool isRunning;
    private Vector2 movement;

    private PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += Move_performed;
        playerInput.Player.Move.canceled += Move_performed;
        playerInput.Player.Run.performed += Run_performed;
        playerInput.Player.Run.canceled += Run_performed;
    }

    private void Update()
    {
        transform.position += (Vector3)movement * Time.deltaTime * currentSpeed;
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
        playerInput.Player.Move.performed -= Move_performed;
        playerInput.Player.Move.canceled -= Move_performed;
        playerInput.Player.Run.performed -= Run_performed;
        playerInput.Player.Run.canceled -= Run_performed;
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        movement = input.ReadValue<Vector2>();
        currentSpeed = isRunning ? runSpeed : walkSpeed;
    }

    private void Run_performed(UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        isRunning = input.ReadValue<float>() >= 0.5f;
        currentSpeed = isRunning ? runSpeed : walkSpeed;
    }
}
