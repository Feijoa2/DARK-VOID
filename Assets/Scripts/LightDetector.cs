using UnityEngine;

public class LightDetector : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private Transform flashlight;

    [Header("Параметры света")]
    [SerializeField] private float lightRange = 6f;
    [SerializeField] private float lightAngle = 60f;
    [SerializeField] private LayerMask wallMask;

    private Transform cachedFlashlight;

    void Start()
    {
        // Кэшируем фонарь один раз
        if (flashlight != null)
            cachedFlashlight = flashlight;
        else
        {
            GameObject fl = GameObject.FindGameObjectWithTag("Flashlight");
            if (fl != null)
                cachedFlashlight = fl.transform;
        }
    }

    public bool IsInLight()
    {
        if (cachedFlashlight == null) return false;

        Vector2 toTarget = transform.position - cachedFlashlight.position;
        float distance = toTarget.magnitude;

        if (distance > lightRange) return false;

        Vector2 forward = cachedFlashlight.up;
        float angle = Vector2.Angle(forward, toTarget);
        if (angle > lightAngle / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(
            cachedFlashlight.position,
            toTarget.normalized,
            distance,
            wallMask
        );

        return hit.collider == null;
    }
}
