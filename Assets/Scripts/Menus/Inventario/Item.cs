using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [TextArea(2, 4)]
    public string description;

    public ItemEffect effect;
}
