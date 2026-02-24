using UnityEngine;
using UnityEngine.UI;

public class TargetableEntity : MonoBehaviour, ITargetable
{
    public Image image;
    private Color originalColor;

    public string characterName;

    void Awake()
    {
        if (image != null)
            originalColor = image.color;
    }

    public string TargetName => characterName;

    public void OnHoverEnter()
    {
        if (image != null)
            image.color = Color.red;
    }

    public void OnHoverExit()
    {
        if (image != null)
            image.color = originalColor;
    }

    public void OnSelected()
    {
        CharacterStats stats = StatsManager.Instance.characters
            .Find(c => c.name == TargetName);

        if (stats == null)
        {
            return;
        }

        Item selectedItem = InventoryUIManager.Instance.SelectedItemForTargeting;
        if (selectedItem != null)
        {
            selectedItem.effect.Apply(stats);
        }

        UITargetingManager.Instance.StopTargeting();
    }
}