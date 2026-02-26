using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    private Item selectedItem;
    public Item SelectedItemForTargeting => selectedItem;

    public bool isCombatInventory;

    [Header("Inventory Panels")]
    public GameObject normalInventoryPanel;
    public Transform normalItemsParent;

    [Header("Combat Inventory Panels (scene-dependent)")]
    public GameObject combatInventoryPanel;
    public Transform combatItemsParent;

    [Header("Item Slot Prefabs")]
    public ItemSlot normalItemSlotPrefab;
    public ItemSlot combatItemSlotPrefab;

    [Header("Main Menu References")]
    public GameObject personajesPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Combate")
        {
            isCombatInventory = true;
            combatInventoryPanel = GameObject.Find("CombatInventoryPanel");
            if (combatInventoryPanel != null)
                combatItemsParent = combatInventoryPanel.transform.Find("Scroll View/Viewport/Content");
        }
        else
        {
            isCombatInventory = false;
        }
    }

    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
            return;
    }

    public void ToggleNormalInventory()
    {
        if (normalInventoryPanel == null) return;

        bool isActive = normalInventoryPanel.activeSelf;

        if (!isActive)
        {
            if (MainMenuManager.Instance != null)

                normalInventoryPanel.SetActive(true);

            if (personajesPanel != null)
                personajesPanel.SetActive(false);

            RefreshNormal();
        }
        else
        {
            CloseNormalInventory();
        }
    }

    public void OpenCombatInventory()
    {
        if (combatInventoryPanel == null)
        {
            combatInventoryPanel = GameObject.Find("CombatInventoryPanel");
            if (combatInventoryPanel != null)
                combatItemsParent = combatInventoryPanel.transform.Find("Scroll View/Viewport/Content");
        }

        if (combatInventoryPanel == null) return;

        combatInventoryPanel.SetActive(true);
        RefreshCombat();
    }

    public void RefreshAll()
    {
        if (normalInventoryPanel != null && normalInventoryPanel.activeSelf)
            RefreshNormal();

        if (combatInventoryPanel != null && combatInventoryPanel.activeSelf)
            RefreshCombat();
    }

    private void RefreshNormal()
    {
        if (normalItemsParent == null || normalItemSlotPrefab == null) return;

        foreach (Transform child in normalItemsParent)
            Destroy(child.gameObject);

        foreach (var entry in InventoryManager.Instance.items)
        {
            ItemSlot slot = Instantiate(normalItemSlotPrefab, normalItemsParent, false);
            slot.Setup(entry.Key, entry.Value);

            var button = slot.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
                button.onClick.AddListener(() => OnItemClicked(entry.Key));
        }
    }

    private void RefreshCombat()
    {
        if (combatItemsParent == null || combatItemSlotPrefab == null) return;

        foreach (Transform child in combatItemsParent)
            Destroy(child.gameObject);

        foreach (var entry in InventoryManager.Instance.items)
        {
            ItemSlot slot = Instantiate(combatItemSlotPrefab, combatItemsParent, false);
            slot.Setup(entry.Key, entry.Value);

            var button = slot.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
                button.onClick.AddListener(() => OnItemClicked(entry.Key));
        }
    }

    private void OnItemClicked(Item item)
    {
        selectedItem = item;

        if (isCombatInventory)
        {
            UseItemAction action = new UseItemAction(selectedItem);
            TurnManager.Instance.ExecuteAction(action);

            RefreshAll();
            if (combatInventoryPanel != null)
                combatInventoryPanel.SetActive(false);
        }
        else
        {
            UITargetingManager.Instance.StartTargeting(
                OnTargetSelected
            );
        }
    }

    private void OnTargetSelected(ITargetable targetable)
    {
        CharacterStats stats = StatsManager.Instance.characters
            .Find(c => c.name == targetable.TargetName);

        if (stats != null && selectedItem != null)
        {
            selectedItem.effect.Apply(stats);

            InventoryManager.Instance.UseItem(selectedItem, null);
            selectedItem = null;

            RefreshAll();
        }

        UITargetingManager.Instance.StopTargeting();
    }
    public bool IsNormalInventoryOpen()
    {
        return normalInventoryPanel != null && normalInventoryPanel.activeSelf;
    }

    public void CloseNormalInventory()
    {
        if (normalInventoryPanel != null)
            normalInventoryPanel.SetActive(false);

        if (personajesPanel != null)
            personajesPanel.SetActive(true);
    }

    public void OpenInventory()
    {
        UIStateManager.Instance.ChangeState(UIStateManager.UIState.Inventory);
    }

    public void OpenInventory_Internal()
    {
        ToggleNormalInventory();
    }

    public void CloseInventory_Internal()
    {
        if (IsNormalInventoryOpen())
            CloseNormalInventory();
    }
}
