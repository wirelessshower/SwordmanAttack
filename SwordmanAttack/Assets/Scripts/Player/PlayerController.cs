using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotSpeed = 5f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravityForce = -30f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    private CharacterController cc;
    private Animator anim;

    private float vertVel;
    private float coyoteTimer;
    private float jumpBufferTimer;

   

    // Хэши аниматора
    private static readonly int hashSpeed = Animator.StringToHash("Speed");
    private static readonly int hashIsMoving = Animator.StringToHash("IsMoving");
    private static readonly int hashIsGround = Animator.StringToHash("IsGrounded");
    private static readonly int hashIsFalling = Animator.StringToHash("IsFalling");
    private static readonly int hashJumpTrig = Animator.StringToHash("IsJumping");

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        HandleJumpInput();
        Move(input);
    }

    private void HandleJumpInput()
    {
        // Обновляем таймеры
        if (cc.isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        // Прыжок, если есть буфер и мы всё ещё в coyote time
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
            PerformJump();
    }

    public void Jump()
    {
        // Можно сразу кинуть PerformJump или присвоить буфер
        if(cc.isGrounded)
            PerformJump();
    }
    
    private void PerformJump()
    {
        vertVel = Mathf.Sqrt(jumpHeight * -2f * gravityForce);
        anim.SetTrigger(hashJumpTrig);
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
    }

    public void Move(Vector2 input)
    {
        // Сброс вертикальной скорости при касании земли
        if (cc.isGrounded && vertVel < 0f)
            vertVel = -2f;

        // Горизонтальное направление в мировых координатах камеры
        Vector3 fwd = CameraController.instance.GetCameraForward();
        Vector3 right = CameraController.instance.GetCameraRight();
        Vector3 dir = (fwd * input.y + right * input.x).normalized;

        // Поворот персонажа
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                   targetRot,
                                                   rotSpeed * Time.deltaTime);
        }

        // Гравитация
        vertVel += gravityForce * Time.deltaTime;

        // Совмещаем движение
        Vector3 velocity = dir * moveSpeed + Vector3.up * vertVel;
        cc.Move(velocity * Time.deltaTime);

        // Анимация
        float speedVal = input.magnitude;
        bool grounded = cc.isGrounded;
        bool falling = !grounded && vertVel < -1f;

        anim.SetFloat(hashSpeed, speedVal);
        anim.SetBool(hashIsMoving, speedVal > 0.1f);
        anim.SetBool(hashIsGround, grounded);
        anim.SetBool(hashIsFalling, falling);
    }
    

}