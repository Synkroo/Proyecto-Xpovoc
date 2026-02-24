using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AddStat", menuName = "Effects/AddStat")]
public class AddStat : IEffect
{
    [Header("Stats a aumentar")]
    public List<StatValue> statsToIncrease = new();

    [Header("Stats a reducir")]
    public List<StatValue> statsToDecrease = new();

    // Aplicar efecto a BaseEntity (combate)
    public override void Apply(BaseEntity target)
    {
        if (target == null) return;

        CharacterStats stats = StatsManager.Instance.characters
            .Find(c => c.name == target.entityName);

        if (stats == null)
        {
            Debug.LogWarning($"No se encontró stats para {target.entityName}");
            return;
        }

        Apply(stats);
        Debug.Log($"Se aplicó {name} a {target.entityName} (BaseEntity)");
    }

    // Aplicar efecto a CharacterStats (fuera de combate)
    public override void Apply(CharacterStats stats)
    {
        if (stats == null) return;

        foreach (var change in statsToIncrease)
        {
            switch (change.stat)
            {
                case StatsEnum.Health:
                    stats.health = Mathf.Min(stats.health + change.value, stats.maxHealth);
                    break;
                case StatsEnum.Mana:
                    stats.mana = Mathf.Min(stats.mana + change.value, stats.maxMana);
                    break;
                case StatsEnum.Speed:
                    stats.speed += change.value;
                    break;
            }
        }

        foreach (var change in statsToDecrease)
        {
            switch (change.stat)
            {
                case StatsEnum.Health:
                    stats.health = Mathf.Max(stats.health - change.value, 0);
                    break;
                case StatsEnum.Mana:
                    stats.mana = Mathf.Max(stats.mana - change.value, 0);
                    break;
                case StatsEnum.Speed:
                    stats.speed -= change.value;
                    break;
            }
        }

        Debug.Log($"Se aplicó {name} a {stats.name} (CharacterStats)");
    }
}
