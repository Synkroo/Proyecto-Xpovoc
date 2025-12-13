using UnityEngine;

public class EntityInitializer : MonoBehaviour
{
    public BaseEntity entity;

    [Header("Tipo de entidad")]
    public bool isAlly;
    public bool isHelper;
    public bool isEnemy;

    [Header("Stats iniciales")]
    public StatValue[] stats; // Aquí sí funciona si StatValue está declarado correctamente

    void Awake()
    {
        if (entity == null) entity = GetComponent<BaseEntity>();

        foreach (var s in stats)
        {
            entity.stats[s.stat] = s.value;
        }

        // Copiar tipo de entidad
        entity.isAlly = isAlly;
        entity.isEnemy = isEnemy;
        entity.isHelper = isHelper;
    }

}
