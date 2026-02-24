using UnityEngine;

public abstract class IEffect : ScriptableObject
{
    // Método para combate
    public virtual void Apply(BaseEntity target)
    {
        Debug.LogWarning($"{name} no tiene implementación para BaseEntity.");
    }

    // Método para UI / fuera de combate
    public virtual void Apply(CharacterStats stats)
    {
        Debug.LogWarning($"{name} no tiene implementación para CharacterStats.");
    }
}
