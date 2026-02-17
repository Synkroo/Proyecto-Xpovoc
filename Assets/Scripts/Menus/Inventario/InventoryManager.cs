using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<Item, int> items = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(Item item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return;

        if (items.ContainsKey(item))
            items[item] += amount;
        else
            items[item] = amount;
    }

    public bool UseItem(Item item, BaseEntity target)
    {
        if (item == null || target == null)
            return false;

        if (!items.ContainsKey(item))
            return false;

        if (!item.effect.CanApply(target))
            return false;

        items[item]--;

        if (items[item] <= 0)
            items.Remove(item);

        return true;
    }
}
