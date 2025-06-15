using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f, rotationSpeed = 5f, junmpForse = 10f, gravity = -30f;

    private CharacterController characterController;
    private float rotationY, verticalVelocity;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 movementVector)
    {
        Vector3 cameraForward = CameraController.instance.GetCameraForward();
        Vector3 cameraRight = CameraController.instance.GetCameraRight();

        Vector3 moveDirection = cameraForward * movementVector.y + cameraRight * movementVector.x;

        if (movementVector != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

        Vector3 move = moveDirection * movementSpeed * Time.deltaTime;

        characterController.Move(move);
        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);

        float inputMagnitude = movementVector.magnitude;
        animator.SetFloat("Speed", inputMagnitude);
        animator.SetBool("IsMoving", inputMagnitude > 0.1f);
        animator.SetBool("IsGrounded", characterController.isGrounded);

        // Проверка падения
        if (!characterController.isGrounded && verticalVelocity < -1f)
        {
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsFalling", false);
        }
    }
   

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = junmpForse;
            animator.SetTrigger("IsJumping");
        }
        
    }
}
