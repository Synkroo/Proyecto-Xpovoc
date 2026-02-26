using UnityEngine;

public class ToggleMap : MonoBehaviour
{
    public static ToggleMap Instance;

    public GameObject Mapa;
    public GameObject objectToHideWhileMapOpen;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMap()
    {
        UIStateManager.Instance.ChangeState(UIStateManager.UIState.Map);
    }

    public void OpenMap_Internal()
    {
        if (Mapa == null) return;

        Mapa.SetActive(true);

        if (objectToHideWhileMapOpen != null)
            objectToHideWhileMapOpen.SetActive(false);
    }

    public void CloseMap_Internal()
    {
        if (Mapa == null) return;

        Mapa.SetActive(false);

        if (objectToHideWhileMapOpen != null)
            objectToHideWhileMapOpen.SetActive(true);
    }
}