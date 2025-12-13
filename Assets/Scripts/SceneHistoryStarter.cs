using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SceneHistoryStarter : MonoBehaviour
{
    void Awake()
    {
        // Si no existe el SceneHistory global, lo creamos
        if (SceneHistory.Instance == null)
        {
            GameObject obj = new GameObject("SceneHistory");
            obj.AddComponent<SceneHistory>();
        }
    }
}
