using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistory : MonoBehaviour
{
    public static SceneHistory Instance { get; private set; }

    // Guarda nombres de escenas en orden; el último es la actual
    private List<string> history = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            history.Add(SceneManager.GetActiveScene().name);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.Log("Se destruyó un SceneHistory duplicado");
            Destroy(gameObject);
        }
    }


    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Añadimos la escena recién cargada a la historia si no es la misma
        if (history.Count == 0 || history[history.Count - 1] != scene.name)
            history.Add(scene.name);
    }

    public string GetPreviousSceneName()
    {
        if (history.Count >= 2)
            return history[history.Count - 2];
        return null;
    }

    public bool LoadPreviousScene()
    {
        string prev = GetPreviousSceneName();
        if (!string.IsNullOrEmpty(prev))
        {
            // Eliminamos la actual de la historia para no repetir al volver otra vez
            history.RemoveAt(history.Count - 1);
            SceneManager.LoadScene(prev);
            return true;
        }
        return false;
    }

    // Útil si quieres limpiar el historial en algún punto
    public void ClearHistory()
    {
        history.Clear();
        history.Add(SceneManager.GetActiveScene().name);
    }
}
