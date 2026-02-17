using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI descriptionText;

    [Header("Root Background")]
    public Image slotBackground;

    private Color normalColor = Color.white;
    private Color hoverColor = Color.blue;

    public void Setup(Item entry, int amount)
    {
        if (entry == null || entry == null) return;
        if (icon == null || nameText == null || amountText == null || descriptionText == null) return;

        icon.sprite = entry.icon;
        nameText.text = entry.itemName;
        amountText.text = "x" + amount;
        descriptionText.text = entry.description;

        if (slotBackground != null)
            normalColor = slotBackground.color;
    }

    public void SetColor(Color color)
    {
        if (slotBackground != null)
            slotBackground.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetColor(normalColor);
    }
}
