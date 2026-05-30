using UnityEngine;
using UnityEngine.SceneManagement;

public class FragmentoAlmaCollector : MonoBehaviour
{
    [Tooltip("Scene name loaded when Fragmento de Alma 1 is collected.")]
    public string sceneForFragmento1 = "Bosque";

    [Tooltip("Scene name loaded when Fragmento de Alma 2 is collected.")]
    public string sceneForFragmento2 = "Mar";

    [Tooltip("Scene name loaded when Fragmento de Alma 3 is collected.")]
    public string sceneForFragmento3 = "Palacio Dragon";

    private void Awake()
    {
        Collider collider3D = GetComponent<Collider>();
        Collider2D collider2D = GetComponent<Collider2D>();

        if (collider3D != null)
        {
            if (!collider3D.isTrigger)
            {
                collider3D.isTrigger = true;
                Debug.Log($"[FragmentoAlmaCollector] Set '{gameObject.name}' 3D collider to Trigger.");
            }

            if (GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                Debug.Log($"[FragmentoAlmaCollector] Added kinematic Rigidbody to '{gameObject.name}'.");
            }
        }
        else if (collider2D != null)
        {
            if (!collider2D.isTrigger)
            {
                collider2D.isTrigger = true;
                Debug.Log($"[FragmentoAlmaCollector] Set '{gameObject.name}' 2D collider to Trigger.");
            }

            if (GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rb2d = gameObject.AddComponent<Rigidbody2D>();
                rb2d.bodyType = RigidbodyType2D.Kinematic;
                rb2d.gravityScale = 0f;
                Debug.Log($"[FragmentoAlmaCollector] Added kinematic Rigidbody2D to '{gameObject.name}'.");
            }
        }
        else
        {
            Debug.LogWarning($"[FragmentoAlmaCollector] '{gameObject.name}' has no Collider or Collider2D. Please add one.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryCollectFragment(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryCollectFragment(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollectFragment(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryCollectFragment(collision.gameObject);
    }

    private void TryCollectFragment(GameObject other)
    {
        string name = other.name.Trim();
        string sceneToLoad = null;
        int fragmentNumber = 0;

        if (name == "Fragmento de Alma 1")
        {
            sceneToLoad = sceneForFragmento1;
            fragmentNumber = 1;
        }
        else if (name == "Fragmento de Alma 2")
        {
            sceneToLoad = sceneForFragmento2;
            fragmentNumber = 2;
        }
        else if (name == "Fragmento de Alma 3")
        {
            sceneToLoad = sceneForFragmento3;
            fragmentNumber = 3;
        }

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log($"[FragmentoAlmaCollector] Contacted '{other.name}', but it is not a Fragmento de Alma.");
            return;
        }

        Debug.Log($"[FragmentoAlmaCollector] Collected '{name}', loading scene '{sceneToLoad}'.");
        
        // Save progression so the next respawn shows the next fragment
        PlayerPrefs.SetInt("CurrentFragmentNumber", fragmentNumber + 1);
        PlayerPrefs.Save();
        
        Destroy(other);
        SceneManager.LoadScene(sceneToLoad);
    }
}
