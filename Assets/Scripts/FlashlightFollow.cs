using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightFollow : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private Transform flashlightPivot;

    [Header("Настройки")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private bool smoothRotation = true;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (flashlightPivot == null)
            flashlightPivot = transform;
    }

    void Update()
    {
        if (mainCamera == null) return;
        if (Mouse.current == null) return; // мыши нет — выходим

        // 1. Позиция мыши (новый Input System)
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // 2. Направление от фонаря к мыши
        Vector2 direction = (mouseWorldPos - flashlightPivot.position).normalized;

        // 3. Угол
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. Поворот
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90f);

        if (smoothRotation)
        {
            flashlightPivot.rotation = Quaternion.Lerp(
                flashlightPivot.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
        else
        {
            flashlightPivot.rotation = targetRotation;
        }
    }
}