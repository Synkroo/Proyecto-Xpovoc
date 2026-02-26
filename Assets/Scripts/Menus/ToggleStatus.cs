using UnityEngine;

public class ToggleStatus : MonoBehaviour
{
    public static ToggleStatus Instance;



    public GameObject StatusUI;
    public GameObject objectToHideWhileOpen;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenStatus()
    {
        UIStateManager.Instance.ChangeState(UIStateManager.UIState.Status);
    }

    public void OpenStatus_Internal()
    {
        if (StatusUI == null) return;

        StatusUI.SetActive(true);

        if (objectToHideWhileOpen != null)
            objectToHideWhileOpen.SetActive(false);
    }

    public void CloseStatus_Internal()
    {
        if (StatusUI == null) return;

        StatusUI.SetActive(false);

        if (objectToHideWhileOpen != null)
            objectToHideWhileOpen.SetActive(true);
    }
}