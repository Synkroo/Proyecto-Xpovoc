public class UseItemAction : BattleAction
{
    private Item item;

    public UseItemAction(Item item)
    {
        this.item = item;
    }

    public override bool Execute(BaseEntity user)
    {
        CombatTargetingManager.Instance.StartTargeting(CombatTargetingManager.TargetType.Ally, target =>
        {
            BaseEntity ally = target as BaseEntity;
            if (ally == null) return;

            bool used = InventoryManager.Instance.UseItem(item, ally);

            if (used)
            {
                TurnManager.Instance.NextTurn();
            }

            CombatTargetingManager.Instance.StopTargeting();
        });

        return false;
    }
}
