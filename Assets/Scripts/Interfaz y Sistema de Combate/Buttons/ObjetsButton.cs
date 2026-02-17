using UnityEngine;
using UnityEngine.UI;

public class ObjetsButton : MonoBehaviour
{
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OpenCombatInventory);
        }
    }

    void OpenCombatInventory()
    {
        if (InventoryUIManager.Instance != null)
        {
            InventoryUIManager.Instance.OpenCombatInventory();
        }
    }
}