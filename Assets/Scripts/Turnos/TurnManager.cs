using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private readonly Dictionary<BaseEntity, float> initiative = new Dictionary<BaseEntity, float>();

    public static TurnManager Instance;

    private Queue<BaseEntity> turnQueue = new Queue<BaseEntity>();

    private List<BaseEntity> allEntities;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeInitiative();
        GenerateTurnOrder();
        StartCoroutine(TurnLoop());
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

    void GenerateTurnOrder()
    {
        allEntities = new List<BaseEntity>(FindObjectsByType<BaseEntity>(FindObjectsSortMode.None));

        var aliveInCombat = allEntities
            .Where(e => e.inCombat && e.GetStat(StatsEnum.Health) > 0)
            .ToList();

        List<BaseEntity> order = new List<BaseEntity>();

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

    IEnumerator TurnLoop()
    {
        while (true)
        {
            BaseEntity current = GetNextEntity();

            yield return BattleSystem.Instance.ExecuteTurn(current);

            UpdateTimeline();
            yield return new WaitForSeconds(0.25f);
        }
    }

    BaseEntity GetNextEntity()
    {
        if (turnQueue.Count == 0)
            GenerateTurnOrder();

        return turnQueue.Dequeue();
    }

    void UpdateTimeline()
    {
        UI_Timeline.Instance.UpdateTimeline(turnQueue.Take(TurnConstants.TurnsViewed).ToList());

    }
}
