public class UseItemAction : BattleAction
{
    private Item item;

    public UseItemAction(Item item)
    {
        this.item = item;
    }

    public override bool Execute(BaseEntity user)
    {
        TargetingManager.Instance.StartTargeting(TargetingManager.TargetType.Ally, target =>
        {
            BaseEntity ally = target as BaseEntity;
            if (ally == null) return;

            bool used = InventoryManager.Instance.UseItem(item, ally);

            if (used)
            {
                TurnManager.Instance.NextTurn();
            }

            TargetingManager.Instance.StopTargeting();
        });

        return false;
    }
}
