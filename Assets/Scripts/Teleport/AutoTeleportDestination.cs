using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable

/// <summary>
/// Automatically sets the destination scene for teleport triggers based on the current scene.
/// This script should be placed on each teleport trigger GameObject.
/// </summary>
public class AutoTeleportDestination : MonoBehaviour
{
    private void Start()
    {
        SetupTeleportDestination();
    }

    private void SetupTeleportDestination()
    {
        TeleportTrigger? teleport = GetComponent<TeleportTrigger>();
        if (teleport == null)
        {
            Debug.LogWarning($"[AutoTeleportDestination] No TeleportTrigger found on '{gameObject.name}'.");
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;
        string destination = GetDestinationScene(currentScene);

        if (!string.IsNullOrEmpty(destination))
        {
            teleport.destinationScene = destination;
            Debug.Log($"[AutoTeleportDestination] Set '{gameObject.name}' to teleport from '{currentScene}' to '{destination}'.");
        }
        else
        {
            Debug.LogWarning($"[AutoTeleportDestination] No destination mapped for scene '{currentScene}'.");
        }
    }

    private string GetDestinationScene(string currentScene)
    {
        return currentScene switch
        {
            "Bosque" => "Playa",
            "Playa" => "Playa Dragon",
            "Playa Dragon" => "Limbo",
            "Mar" => "Limbo",
            "Palacio Dragon" => "Final Boss",
            "Final Boss" => "Creditos Finales",
            _ => ""
        };
    }
}
