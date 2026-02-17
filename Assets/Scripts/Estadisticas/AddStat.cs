using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AddStat", menuName = "Effects/AddStat")]
public class AddStat : IEffect
{
    [Header("Stats a aumentar")]
    public List<StatValue> statsToIncrease = new();

    [Header("Stats a reducir")]
    public List<StatValue> statsToDecrease = new();

    public override void Apply(BaseEntity target)
    {
        if (target == null) return;

        foreach (var change in statsToIncrease)
        {
            if (target.HasStat(change.stat))
                target.AddStat(change.stat, change.value);
        }

        foreach (var change in statsToDecrease)
        {
            if (target.HasStat(change.stat))
                target.AddStat(change.stat, -change.value);
        }
    }
}
