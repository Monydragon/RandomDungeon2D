using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 2.5f, runSpeed = 5f;

    private float currentSpeed;
    private bool isRunning;
    private InputManager inputManager;
    private Vector2 movement;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        movement = inputManager.GetPlayerMovement();
        isRunning = inputManager.GetPlayerRun();
        currentSpeed = isRunning ? runSpeed : walkSpeed;
        transform.position += (Vector3)movement * Time.deltaTime * currentSpeed;
    }
}
