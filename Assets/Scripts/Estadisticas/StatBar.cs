using UnityEngine;

public class StatBar : MonoBehaviour
{
    public Bar bar;
    public BaseEntity entity;
    public StatsEnum stat;

    void Update()
    {
        if (entity != null && entity.HasStat(stat))
        {
            float current = entity.GetStat(stat);
            float max = entity.GetStat(MaxStatFor(stat));
            bar.SetBar(current, max);
        }
    }

    private StatsEnum MaxStatFor(StatsEnum stat)
    {
        return stat switch
        {
            StatsEnum.Health => StatsEnum.MaxHealth,
            StatsEnum.Mana => StatsEnum.MaxMana,
            StatsEnum.Sync => StatsEnum.MaxSync,
            _ => stat
        };
    }
}
