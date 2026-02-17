using UnityEngine;

public class EscapeButton : MonoBehaviour
{
    public void OnClick_Huir()
    {
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.canMove = true;
        }

        if (InventoryUIManager.Instance != null)
        {
            InventoryUIManager.Instance.isCombatInventory = false;
        }

        if (SceneHistory.Instance != null)
        {
            bool ok = SceneHistory.Instance.LoadPreviousScene();
            if (!ok)
                Debug.LogWarning("SceneHistory no tiene escena anterior");
        }
        else
        {
            Debug.LogError("SceneHistory no encontrado");
        }
    }
}
