using UnityEngine;
using UnityEngine.Events;

#nullable enable

/// <summary>
/// Simple singleton manager to track collectible butterflies for a minigame.
/// It auto-creates a persistent GameObject if not present in the scene.
/// </summary>
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; } = null!;

    [Tooltip("How many butterflies are required to complete the minigame.")]
    public int totalToCollect = 6;

    [Tooltip("Number of butterflies collected so far.")]
    [SerializeField]
    private int collected;

    public int Collected => collected;

    [Tooltip("Invoked each time a butterfly is collected.")]
    public UnityEvent onCollected = new UnityEvent();

    [Tooltip("Invoked when all butterflies are collected.")]
    public UnityEvent onAllCollected = new UnityEvent();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureInstanceExists()
    {
        if (Instance != null)
            return;

        var go = new GameObject("MinigameManager");
        Instance = go.AddComponent<MinigameManager>();
        DontDestroyOnLoad(go);
    }

    public void CollectOne()
    {
        collected++;
        Debug.Log($"[MinigameManager] Collected {collected}/{totalToCollect}");
        onCollected?.Invoke();

        if (collected >= totalToCollect)
        {
            Debug.Log("[MinigameManager] All butterflies collected — minigame complete.");
            onAllCollected?.Invoke();
        }
    }

    public void ResetMinigame()
    {
        collected = 0;
    }
}
