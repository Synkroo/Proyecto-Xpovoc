using UnityEngine;

public class ButtonApplyEffect : MonoBehaviour
{
    [Header("Efecto a aplicar")]
    public AddStat effect;

    [Header("Personaje objetivo")]
    public BaseEntity target;

    public void OnClick()
    {
        if (effect != null && target != null)
        {
            effect.Apply(target);
        }
    }
}
