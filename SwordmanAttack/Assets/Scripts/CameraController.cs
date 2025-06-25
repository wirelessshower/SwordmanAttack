using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;


    [SerializeField] private Transform followTarget; // Игрок
    [SerializeField] private Vector3 offset = new Vector3(0, 3, -5); // Смещение камеры
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float minYAngle = -30f, maxYAngle = 60f;

    private float _currentYaw = 0f; // Горизонтальный поворот
    private float _currentPitch = 10f; // Вертикальный поворот (начальное положение)

    private void Awake() {
        if (instance == null)
            instance = this;
        else 
           Destroy(gameObject);
    }

    void LateUpdate()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 lookInput = Mouse.current.delta.ReadValue();
            _currentYaw += lookInput.x * rotationSpeed * Time.deltaTime;
            _currentPitch -= lookInput.y * rotationSpeed * Time.deltaTime;
            _currentPitch = Mathf.Clamp(_currentPitch, minYAngle, maxYAngle);
            Cursor.visible = false;
        }
        else
            Cursor.visible = true;

        Quaternion rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0);
        Vector3 desiredPosition = followTarget.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(followTarget);
    }
    
    public Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0; // Чтобы движение не было вверх/вниз
        return forward.normalized;
    }

    public Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right.y = 0;
        return right.normalized;
    }
}
