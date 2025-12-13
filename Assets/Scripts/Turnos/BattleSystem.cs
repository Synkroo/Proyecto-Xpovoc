using System.Collections;
using System.Linq;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance;

    void Awake() => Instance = this;

    public IEnumerator ExecuteTurn(BaseEntity entity)
    {
        if (entity.GetStat(StatsEnum.Health) <= 0)
            yield break;

        if (entity.isAlly)
        {
            Acciones.Instance.Show(entity);

            yield return new WaitUntil(() => Acciones.Instance.actionSelected);
            yield return Acciones.Instance.PerformSelectedAction();
        }
        else if (entity.isEnemy)
        {
            var allies = FindObjectsByType<BaseEntity>(FindObjectsSortMode.None)
                            .Where(e => e.isAlly && e.GetStat(StatsEnum.Health) > 0)
                            .ToArray();

            if (allies.Length > 0)
            {
                var target = allies[Random.Range(0, allies.Length)];
                target.AddStat(StatsEnum.Health, -10); // Daño de ejemplo
                Debug.Log($"{entity.entityName} atacó a {target.entityName}");
            }

            yield return new WaitForSeconds(0.5f);
        }
        else if (entity.isHelper)
        {
            Debug.Log($"{entity.entityName} hace su acción de helper");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
