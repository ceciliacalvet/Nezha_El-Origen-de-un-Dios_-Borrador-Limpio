using UnityEngine;

#nullable enable

/// <summary>
/// WASD movement controller for Nezha Skin 3.
/// Uses IsMoving to switch between idle and swim animations,
/// and adds a floating bob effect when enabled.
/// </summary>
[AddComponentMenu("Player/Nezha Skin 3 Swim Controller")]
public class NezhaSkin3SwimController : MonoBehaviour
{
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;

    [Tooltip("Choose the movement plane for this controller.")]
    public MovementPlane movementPlane = MovementPlane.XZ;

    [Tooltip("Use world space for movement instead of local object space.")]
    public bool useWorldSpace = true;

    [Tooltip("Flip the sprite or root object horizontally when moving left or right.")]
    public bool flipOnHorizontalMovement = true;

    [Tooltip("Enable floating motion for Skin 3.")]
    public bool enableFloating = true;

    [Tooltip("Floating amplitude in world units.")]
    public float floatingAmplitude = 0.1f;

    [Tooltip("Floating frequency in cycles per second.")]
    public float floatingFrequency = 1f;

    [Tooltip("Invert horizontal input (A/D).")]
    public bool invertX = false;

    [Tooltip("Invert vertical input (W/S).")]
    public bool invertY = false;

    private Transform moveTransform = null!;
    private Transform flipTarget = null!;
    private Vector3 originalScale;
    private Animator? animator;
    private float baselineY;
    private float floatingTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.applyRootMotion = true;
        }

        moveTransform = transform.root;

        // Auto-detect 2D rigs and use the XY movement plane if needed.
        if (movementPlane == MovementPlane.XZ && (GetComponent<SpriteRenderer>() != null || GetComponent<Collider2D>() != null))
        {
            movementPlane = MovementPlane.XY;
        }

        baselineY = moveTransform.position.y;

        var sr = GetComponentInChildren<SpriteRenderer>();
        flipTarget = (sr != null) ? sr.transform : transform;
        originalScale = flipTarget.localScale;
    }

    private void Update()
    {
        Vector3 direction = GetInputDirection();
        bool isMoving = direction.sqrMagnitude > 0f;

        if (isMoving)
        {
            direction.Normalize();
            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            if (useWorldSpace)
            {
                moveTransform.position += movement;
            }
            else
            {
                moveTransform.localPosition += movement;
            }

            if (flipOnHorizontalMovement)
            {
                HandleFacing(direction);
            }
        }

        if (enableFloating)
        {
            floatingTime += Time.deltaTime;
            float floatOffset = Mathf.Sin(floatingTime * floatingFrequency * Mathf.PI * 2f) * floatingAmplitude;
            Vector3 position = moveTransform.position;
            position.y = baselineY + floatOffset;
            moveTransform.position = position;
        }
    }

    private Vector3 GetInputDirection()
    {
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.W))
            z -= 1f;
        if (Input.GetKey(KeyCode.S))
            z += 1f;
        if (Input.GetKey(KeyCode.D))
            x -= 1f;
        if (Input.GetKey(KeyCode.A))
            x += 1f;

        if (invertX) x = -x;
        if (invertY) z = -z;

        return movementPlane == MovementPlane.XZ ? new Vector3(x, 0f, z) : new Vector3(x, z, 0f);
    }

    private void HandleFacing(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            float sign = direction.x > 0f ? 1f : -1f;
            Vector3 scale = originalScale;
            scale.x = Mathf.Abs(scale.x) * sign;
            if (flipTarget != null)
            {
                flipTarget.localScale = scale;
            }
        }
    }
}
