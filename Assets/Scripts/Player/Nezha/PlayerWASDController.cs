using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Adds WASD movement and optional jumping to any GameObject tagged as "Player".
/// This script is attached automatically at runtime after scene load.
/// </summary>
public class PlayerWASDController : MonoBehaviour
{
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;

    [Tooltip("Jump strength used when pressing Space.")]
    public float jumpForce = 5f;

    [Tooltip("Gravity acceleration used for transform-based jump fallback.")]
    public float gravity = -9.81f;

    [Tooltip("Choose the movement plane for this controller.")]
    public MovementPlane movementPlane = MovementPlane.XZ;

    [Tooltip("Use world space for movement instead of local object space.")]
    public bool useWorldSpace = true;

    [Tooltip("Invert horizontal input (A/D).")]
    public bool invertX = false;

    [Tooltip("Invert vertical input (W/S).")]
    public bool invertY = false;

    [Tooltip("Flip the sprite or root object horizontally when moving left or right.")]
    public bool flipOnHorizontalMovement = true;

    [Tooltip("Layers considered ground when using Rigidbody jump.")]
    public LayerMask groundLayers = ~0;

    [Tooltip("Distance below the object used to check whether it is grounded.")]
    public float groundedCheckDistance = 0.2f;

    private Rigidbody rb;
    private bool hasRigidbody;
    private float verticalVelocity;
    private float baselineY;
    private Transform moveTransform;
    private Transform flipTarget;
    private Vector3 originalScale;

    private void Awake()
    {
        hasRigidbody = TryGetComponent<Rigidbody>(out rb);
        // prefer moving the root transform when an Animator is present (prevents animation overwriting movement)
        moveTransform = GetComponent<Animator>() != null ? transform.root : transform;
        baselineY = moveTransform.position.y;

        // find a child SpriteRenderer to flip visually; fall back to the object itself
        var sr = GetComponentInChildren<SpriteRenderer>();
        flipTarget = (sr != null) ? sr.transform : transform;
        originalScale = flipTarget.localScale;
        // Auto-detect 2D sprite-based objects and switch movement to XY plane
        if (movementPlane == MovementPlane.XZ && GetComponent<SpriteRenderer>() != null)
        {
            movementPlane = MovementPlane.XY;
        }
    }

    private void Update()
    {
        Vector3 direction = GetInputDirection();
        if (direction.sqrMagnitude > 0f)
        {
            direction.Normalize();
            moveTransform.Translate(direction * moveSpeed * Time.deltaTime, useWorldSpace ? Space.World : Space.Self);
            if (flipOnHorizontalMovement)
                HandleFacing(direction);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            if (hasRigidbody)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
            else
            {
                verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        if (!hasRigidbody)
        {
            verticalVelocity += gravity * Time.deltaTime;
            Vector3 verticalMovement = Vector3.up * verticalVelocity * Time.deltaTime;
            moveTransform.Translate(verticalMovement, useWorldSpace ? Space.World : Space.Self);

            if (moveTransform.position.y <= baselineY)
            {
                Vector3 position = moveTransform.position;
                position.y = baselineY;
                moveTransform.position = position;
                verticalVelocity = 0f;
            }
        }
    }

    private bool IsGrounded()
    {
        if (hasRigidbody)
        {
            return Physics.Raycast(transform.position, Vector3.down, groundedCheckDistance + 0.1f, groundLayers);
        }

        return moveTransform.position.y <= baselineY + 0.01f;
    }

    private Vector3 GetInputDirection()
    {
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            z += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            z -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            x += 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            x -= 1f;

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
                flipTarget.localScale = scale;
        }
    }
}

public enum MovementPlane
{
    XZ,
    XY
}

/// <summary>
/// Automatically attaches <see cref="PlayerWASDController"/> to all GameObjects tagged as "Player".
/// This runs after each scene load so it works for scene transitions.
/// </summary>
public static class PlayerWASDControllerAttacher
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeOnSceneLoad()
    {
        AddControllersToTaggedPlayers();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddControllersToTaggedPlayers();
    }

    private static void AddControllersToTaggedPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player == null)
                continue;

            if (!player.TryGetComponent<PlayerWASDController>(out _))
            {
                player.AddComponent<PlayerWASDController>();
                Debug.Log($"[PlayerWASDControllerAttacher] Added WASD controller to '{player.name}'.");
            }
        }
    }
}
