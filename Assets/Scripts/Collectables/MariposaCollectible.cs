using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class MariposaCollectible : MonoBehaviour
{
    [Tooltip("Tag used to identify player objects.")]
    public string playerTag = "Player";

    [Tooltip("Optional sound played when collected.")]
    public AudioClip collectSfx;

    [Tooltip("Volume for the collection sound.")]
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private void Reset()
    {
        Collider c = GetComponent<Collider>();
        if (c != null)
            c.isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Collider c = GetComponent<Collider>();
        if (c != null)
            c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag(playerTag) || other.transform.root.CompareTag(playerTag);
        Debug.Log($"[MariposaCollectible] Trigger by '{other.gameObject.name}' (tag='{other.gameObject.tag}'), isPlayer={isPlayer}");

        if (!isPlayer)
            return;

        if (collectSfx != null)
            AudioSource.PlayClipAtPoint(collectSfx, transform.position, sfxVolume);

        MinigameManager.Instance?.CollectOne();
        Destroy(gameObject);
    }
}
