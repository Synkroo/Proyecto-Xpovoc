using UnityEngine;

public class ButtonApplyEffect : MonoBehaviour
{
    [Header("Efecto a aplicar")]
    public AddStat effect;          // tu ScriptableObject de efecto

    [Header("Personaje objetivo")]
    public BaseEntity target;       // BaseEntity del personaje o enemigo

    // Método que se llama desde el botón
    public void OnClick()
    {
        if (effect != null && target != null)
        {
            effect.Apply(target);
        }
    }
}
