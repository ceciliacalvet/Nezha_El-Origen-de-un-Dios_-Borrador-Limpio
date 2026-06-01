using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable

/// <summary>
/// A trigger that teleports the player to a specified scene when they enter.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TeleportTrigger : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the player enters this trigger.")]
    public string destinationScene = "";

    [Tooltip("Tag used to identify player objects.")]
    public string playerTag = "Player";

    [Tooltip("Optional fade-out duration before loading the new scene (in seconds).")]
    public float transitionDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag(playerTag) || other.transform.root.CompareTag(playerTag);
        
        if (!isPlayer)
            return;

        if (string.IsNullOrEmpty(destinationScene))
        {
            Debug.LogWarning($"[TeleportTrigger] No destination scene set on '{gameObject.name}'.");
            return;
        }

        Debug.Log($"[TeleportTrigger] Teleporting '{other.gameObject.name}' to scene '{destinationScene}'.");
        LoadScene(destinationScene);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isPlayer = other.CompareTag(playerTag) || other.transform.root.CompareTag(playerTag);
        
        if (!isPlayer)
            return;

        if (string.IsNullOrEmpty(destinationScene))
        {
            Debug.LogWarning($"[TeleportTrigger] No destination scene set on '{gameObject.name}'.");
            return;
        }

        Debug.Log($"[TeleportTrigger] Teleporting '{other.gameObject.name}' to scene '{destinationScene}'.");
        LoadScene(destinationScene);
    }

    private void LoadScene(string sceneName)
    {
        // You can add fade-out logic here if desired
        SceneManager.LoadScene(sceneName);
    }
}
