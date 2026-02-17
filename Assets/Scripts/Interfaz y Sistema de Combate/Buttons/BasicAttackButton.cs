using UnityEngine;

public class BasicAttackButton : MonoBehaviour
{
    [Header("Efecto del ataque básico")]
    public IEffect effect;

    public void OnClick()
    {
        if (effect == null) return;
        BasicAttack attack = new BasicAttack(effect);
        TurnManager.Instance.ExecuteAction(attack);
    }
}
