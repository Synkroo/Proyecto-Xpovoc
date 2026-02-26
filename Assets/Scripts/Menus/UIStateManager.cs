using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    public static UIStateManager Instance;

    public enum UIState
    {
        None,
        Map,
        Inventory,
        Status
    }

    public UIState CurrentState = UIState.None;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeState(UIState newState)
    {
        if (CurrentState == newState) return;

        CloseAll();

        CurrentState = newState;

        switch (CurrentState)
        {
            case UIState.Map:
                ToggleMap.Instance?.OpenMap_Internal();
                break;

            case UIState.Inventory:
                InventoryUIManager.Instance?.OpenInventory_Internal();
                break;

            case UIState.Status:
                ToggleStatus.Instance?.OpenStatus_Internal();
                break;
        }
    }

    public void CloseAll()
    {
        ToggleMap.Instance?.CloseMap_Internal();
        ToggleStatus.Instance?.CloseStatus_Internal();
        InventoryUIManager.Instance?.CloseInventory_Internal();

        CurrentState = UIState.None;
    }
}