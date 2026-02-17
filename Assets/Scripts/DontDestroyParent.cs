using UnityEngine;

public class DontDestroyParent : MonoBehaviour
{
    private static DontDestroyParent instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
