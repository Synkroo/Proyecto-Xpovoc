using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Submenus")]
    public GameObject[] subMenus;

    public static MainMenuManager Instance;

    public GameObject mainMenuPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }
    }

    public void CloseAllSubMenus()
    {
        foreach (GameObject menu in subMenus)
        {
            if (menu != null)
                menu.SetActive(false);
        }

        if (InventoryUIManager.Instance != null &&
            InventoryUIManager.Instance.personajesPanel != null)
        {
            InventoryUIManager.Instance.personajesPanel.SetActive(true);
        }
    }

    public void HandleEscape()
    {
        if (UIStateManager.Instance.CurrentState != UIStateManager.UIState.None)
        {
            UIStateManager.Instance.CloseAll();
            return;
        }

        mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);
    }

    public void OpenInventoryFromMenu()
    {
        InventoryUIManager.Instance.CloseNormalInventory();

        InventoryUIManager.Instance.ToggleNormalInventory();
    }

    public void OnMainMenuButtonPressed()
    {
        InventoryUIManager.Instance.CloseNormalInventory();
    }
}