using UnityEngine;

public abstract class IEffect : ScriptableObject
{
    public virtual bool CanApply(BaseEntity target) => true;
    public abstract void Apply(BaseEntity target);
}

