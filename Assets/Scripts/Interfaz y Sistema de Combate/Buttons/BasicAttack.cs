public class BasicAttack : BattleAction
{
    private IEffect effect;

    public BasicAttack(IEffect effect)
    {
        this.effect = effect;
    }

    public override bool Execute(BaseEntity attacker)
    {
        TargetingManager.Instance.StartTargeting(
            TargetingManager.TargetType.Enemy,
            target =>
            {
                BaseEntity enemy = target as BaseEntity;
                if (enemy == null) return;

                effect.Apply(enemy);
                TurnManager.Instance.NextTurn();
            }
        );

        return false;
    }
}
