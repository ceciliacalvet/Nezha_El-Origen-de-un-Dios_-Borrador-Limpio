using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    [Tooltip("Scene name to load when Play is pressed. Set this to a scene included in Build Settings.")]
    public string playSceneName = "Limbo";

    public void Jugar()
    {
        if (string.IsNullOrEmpty(playSceneName))
        {
            Debug.LogWarning("[Menu] playSceneName is empty. Set a scene name in the Inspector.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(playSceneName))
        {
            Debug.LogWarning($"[Menu] Scene '{playSceneName}' is not in Build Settings or cannot be loaded. Add it via File->Build Settings.");
            return;
        }

        Debug.Log($"[Menu] Loading scene '{playSceneName}'.");
        SceneManager.LoadScene(playSceneName);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
        // Stop play mode in the editor so the button appears to work while testing
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
