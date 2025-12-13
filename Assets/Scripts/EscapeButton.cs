using UnityEngine;

public class EscapeButton : MonoBehaviour
{
    public void OnClick_Huir()
    {
        if (SceneHistory.Instance != null)
        {
            bool ok = SceneHistory.Instance.LoadPreviousScene();

            if (!ok)
                Debug.LogWarning("SceneHistory no tiene una escena anterior. ¿Es esta la primera escena?");
        }
        else
        {
            Debug.LogError("No se encontró SceneHistory en la escena. Asegúrate de tenerlo en un GameObject con DontDestroyOnLoad.");
        }
    }
}