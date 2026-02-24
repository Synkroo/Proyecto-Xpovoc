using UnityEngine;

public class EntityInitializer : MonoBehaviour
{
    public BaseEntity entity;
    public int characterIndex;
    public StatBar statBar;

    void Awake()
    {
        if (entity == null) entity = GetComponent<BaseEntity>();
        var stats = StatsManager.Instance.characters[characterIndex];

        // Stats actuales
        entity.stats[StatsEnum.Health] = stats.health;
        entity.stats[StatsEnum.Mana] = stats.mana;
        entity.stats[StatsEnum.Speed] = stats.speed;

        // Stats máximos
        entity.stats[StatsEnum.MaxHealth] = stats.maxHealth;
        entity.stats[StatsEnum.MaxMana] = stats.maxMana;

        entity.entityName = stats.name;

        if (statBar != null)
            statBar.bar.SetBar(stats.health, stats.maxHealth);
    }
}
