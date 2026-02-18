using UnityEngine;

public class EntityUIBinder : MonoBehaviour
{
    [Header("Entity y stats iniciales")]
    public BaseEntity entity;
    public EntityInitializer initializer;

    [Header("Componentes de UI")]
    private StatBar[] statBars;
    private EnemyHealthColor[] healthTexts;

    void Awake()
    {
        if (entity == null)
        {
            Debug.LogWarning("EntityUISetup: No se asignó BaseEntity.");
            return;
        }

        if (initializer != null)
        {
            foreach (var s in initializer.stats)
            {
                entity.stats[s.stat] = s.value;
            }
        }

        statBars = GetComponentsInChildren<StatBar>();
        healthTexts = GetComponentsInChildren<EnemyHealthColor>();

        foreach (var bar in statBars)
            bar.entity = entity;

        foreach (var text in healthTexts)
            text.entity = entity;
    }
}