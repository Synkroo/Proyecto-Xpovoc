using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Effect")]
public class ItemEffect : ScriptableObject
{
    public List<StatModifier> modifiers;

    [Header("Uso en combate")]
    public bool CanUseInCombat = true;
    public bool CanApply(BaseEntity target)
    {
        foreach (var mod in modifiers)
        {
            if (mod.stat == StatsEnum.Health && mod.value > 0)
            {
                if (target.GetStat(StatsEnum.Health) >= target.GetStat(StatsEnum.MaxHealth))
                    return false;
            }
        }

        return true;
    }
}
