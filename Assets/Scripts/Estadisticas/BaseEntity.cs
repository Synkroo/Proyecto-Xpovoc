using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public bool inCombat = true;
    public Sprite turnIcon;

    // Booleanos de tipo
    public bool isAlly;
    public bool isEnemy;
    public bool isHelper;

    public Dictionary<StatsEnum, float> stats = new Dictionary<StatsEnum, float>();
    public string entityName = "Enemy";

    public bool HasStat(StatsEnum stat) => stats.ContainsKey(stat);
    public float GetStat(StatsEnum stat) => HasStat(stat) ? stats[stat] : 0f;
    public void AddStat(StatsEnum stat, float amount)
    {
        if (!HasStat(stat)) return;
        stats[stat] += amount;
    }
    public void SetStat(StatsEnum stat, float value)
    {
        if (!HasStat(stat)) return;
        stats[stat] = value;
    }
}
