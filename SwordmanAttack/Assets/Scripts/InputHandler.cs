using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;   // Твой скрипт с Move() и Jump()

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private PlayerInput   playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Берём Actions из того же InputActionAsset, что и PlayerInput
        var map = playerInput.actions.FindActionMap("Player");

        moveAction = map.FindAction("Move");
        lookAction = map.FindAction("Look");
        jumpAction = map.FindAction("Jump");

        // Включаем их, чтобы они заработали
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();

        // Привязываем jump
        jumpAction.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        jumpAction.performed -= OnJumpPerformed;

        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        // Читаем вектор и передаём в твой контроллер
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        playerController.Move(moveVector);

        // Если нужен look:
        // Vector2 lookVector = lookAction.ReadValue<Vector2>();
        // playerController.Look(lookVector);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        playerController.Jump();
    }
}