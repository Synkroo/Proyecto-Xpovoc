using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    private Item selectedItem;
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
            combatInventoryPanel = GameObject.Find("CombatInventoryPanel");
            if (combatInventoryPanel != null)
                combatItemsParent = combatInventoryPanel.transform.Find("Scroll View/Viewport/Content");
        }
    }

    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
            return;

        if (!isCombatInventory && Input.GetKeyDown(KeyCode.I))
            ToggleNormalInventory();
    }

    public void ToggleNormalInventory()
    {
        if (normalInventoryPanel == null) return;

        bool isActive = normalInventoryPanel.activeSelf;
        normalInventoryPanel.SetActive(!isActive);

        if (!isActive)
            RefreshNormal();
    }

    public void OpenCombatInventory()
    {
        isCombatInventory = true;

        if (combatInventoryPanel == null)
        {
            combatInventoryPanel = GameObject.Find("CombatInventoryPanel");
            if (combatInventoryPanel != null)
                combatItemsParent = combatInventoryPanel.transform.Find("Scroll View/Viewport/Content");
        }

        if (combatInventoryPanel == null)
            return;

        combatInventoryPanel.SetActive(true);
        RefreshCombat();
    }

    public void RefreshAll()
    {
        RefreshNormal();
        RefreshCombat();
    }

    void RefreshNormal()
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

    void RefreshCombat()
    {
        if (combatItemsParent == null || combatItemSlotPrefab == null)
            return;

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
            TargetingManager.Instance.StartTargeting(
                TargetingManager.TargetType.Ally,
                OnTargetSelected
            );
        }
    }




    private void OnTargetSelected(ITargetable targetable)
    {
        BaseEntity target = targetable as BaseEntity;
        if (target == null)
        {
            TargetingManager.Instance.StopTargeting();
            return;
        }

        bool used = InventoryManager.Instance.UseItem(selectedItem, target);

        if (used)
        {
            selectedItem = null;

            RefreshAll();

            if (isCombatInventory)
            {
                if (combatInventoryPanel != null) combatInventoryPanel.SetActive(false);
                TurnManager.Instance.NextTurn();
            }
            else
            {
                if (normalInventoryPanel != null) normalInventoryPanel.SetActive(false);
            }
        }

        TargetingManager.Instance.StopTargeting();
    }

}
