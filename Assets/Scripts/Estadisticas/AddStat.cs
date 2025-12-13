using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AddStat", menuName = "Effects/AddStat")]
public class AddStat : ScriptableObject
{
    [Header("Stats a aumentar")]
    public List<StatChange> statsToIncrease = new List<StatChange>();
    [Header("Stats a reducir")]
    public List<StatChange> statsToDecrease = new List<StatChange>();

    public void Apply(BaseEntity target)
    {
        if (target == null) return;

        // Aplicar aumentos
        foreach (var change in statsToIncrease)
        {
            if (target.HasStat(change.stat))
                target.AddStat(change.stat, change.amount);
        }

        // Aplicar reducciones
        foreach (var change in statsToDecrease)
        {
            if (target.HasStat(change.stat))
                target.AddStat(change.stat, -change.amount); // negativo
        }
    }
}
