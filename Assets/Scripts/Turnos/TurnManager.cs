using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private readonly Dictionary<BaseEntity, float> initiative = new Dictionary<BaseEntity, float>();

    public static TurnManager Instance;

    private Queue<BaseEntity> turnQueue = new Queue<BaseEntity>();

    private List<BaseEntity> allEntities;

    public BaseEntity current;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeInitiative();
        GenerateTurnOrder(new List<BaseEntity>());
        NextTurn();
    }

    void InitializeInitiative()
    {
        allEntities = new List<BaseEntity>(FindObjectsByType<BaseEntity>(FindObjectsSortMode.None));

        var aliveInCombat = allEntities
            .Where(e => e.inCombat && e.GetStat(StatsEnum.Health) > 0)
            .ToList();

        foreach (BaseEntity entity in aliveInCombat)
        {
            initiative[entity] = 0;
        }
    }

    void GenerateTurnOrder(List<BaseEntity> order)
    {
        allEntities = new List<BaseEntity>(FindObjectsByType<BaseEntity>(FindObjectsSortMode.None));

        var aliveInCombat = allEntities
            .Where(e => e.inCombat && e.GetStat(StatsEnum.Health) > 0)
            .ToList();

        for (int i = 0; i < TurnConstants.TurnsGenerated; i++)
        {
            foreach (BaseEntity entity in aliveInCombat)
            {
                initiative[entity] += entity.GetStat(StatsEnum.Speed) * Random.Range(TurnConstants.LowRNGinitiative, TurnConstants.HighRNGinitiative);
            }

            BaseEntity selected = initiative
                .OrderByDescending(entityInitiative => entityInitiative.Value)
                .First().Key;

            order.Add(selected);
            initiative[selected] -= TurnConstants.InitiativeThreshHold;
        }

        turnQueue = new Queue<BaseEntity>(order);

        UpdateTimeline();
    }

    void NextTurn()
    {
        current = GetNextEntity();

        UI_BattleManager.Instance.ShowActions();

        UpdateTimeline();
    }

    BaseEntity GetNextEntity()
    {
        if (turnQueue.Count < 7)
            GenerateTurnOrder(turnQueue.ToList());

        return turnQueue.Dequeue();
    }

    public void ExecuteAction(IBattleAction action)
    {
        bool consumesTurn = action.Execute(current);
        if (!consumesTurn)
        {
            return;
        }

        NextTurn();
    }

    void UpdateTimeline()
    {
        UI_BattleManager.Instance.UpdateTimeline(turnQueue.Take(TurnConstants.TurnsViewed).ToList());

    }
}
