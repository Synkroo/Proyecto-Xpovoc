public class BasicAttack : BattleAction
{
    private IEffect effect;

    public BasicAttack(IEffect effect)
    {
        this.effect = effect;
    }

    public override bool Execute(BaseEntity attacker)
    {
        TargetingManager.Instance.StartTargeting(target =>
        {
            BaseEntity enemy = target as BaseEntity;
            if (enemy == null || !enemy.isEnemy)
                return;

            effect.Apply(enemy);

            TargetingManager.Instance.StopTargeting();
            TurnManager.Instance.NextTurn();
        });

        return false;
    }
}
