using UnityEngine;

public abstract class IEffect : ScriptableObject
{
    // Aplicar el efecto sobre una entidad concreta
    public abstract void Apply(BaseEntity target);
}
