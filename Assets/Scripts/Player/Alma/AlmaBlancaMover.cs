using UnityEngine;

public class AlmaBlancaMover : MonoBehaviour
{
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;

    [Tooltip("Vertical floating distance.")]
    public float floatAmplitude = 0.5f;

    [Tooltip("How fast the object floats up and down.")]
    public float floatFrequency = 1f;

    [Tooltip("Speed for manual up/down movement.")]
    public float verticalSpeed = 3f;

    private float startY;
    private float manualYOffset;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float verticalMove = 0f;
        if (Input.GetKey(KeyCode.W)) verticalMove += 1f;
        if (Input.GetKey(KeyCode.S)) verticalMove -= 1f;

        Vector3 direction = new Vector3(horizontal, 0f, 0f).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        manualYOffset += verticalMove * verticalSpeed * Time.deltaTime;

        float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 position = transform.position;
        position.y = startY + floatOffset + manualYOffset;
        transform.position = position;
    }
}
