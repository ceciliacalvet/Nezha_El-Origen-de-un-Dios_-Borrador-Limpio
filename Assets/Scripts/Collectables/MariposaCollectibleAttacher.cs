using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Automatically attaches <see cref="MariposaCollectible"/> to all GameObjects tagged as "Mariposa".
/// This runs after each scene load so it works for scene transitions.
/// </summary>
public static class MariposaCollectibleAttacher
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeOnSceneLoad()
    {
        AttachCollectibleToTaggedMariposas();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AttachCollectibleToTaggedMariposas();
    }

    private static void AttachCollectibleToTaggedMariposas()
    {
        GameObject[] mariposas = GameObject.FindGameObjectsWithTag("Mariposa");
        foreach (GameObject mariposa in mariposas)
        {
            if (mariposa == null)
                continue;

            if (!mariposa.TryGetComponent<MariposaCollectible>(out _))
            {
                mariposa.AddComponent<MariposaCollectible>();
                Debug.Log($"[MariposaCollectibleAttacher] Added collectible to '{mariposa.name}'.");
            }
        }
    }
}
