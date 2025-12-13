using TMPro;
using UnityEngine;

public class EnemyHealthColor : MonoBehaviour
{
    public TMP_Text healthTMP;
    public BaseEntity entity;

    void Update()
    {
        float healthPercent = entity.GetStat(StatsEnum.Health) / entity.GetStat(StatsEnum.MaxHealth) * 100f;

        if (healthPercent >= 75f)
            healthTMP.color = Color.black;
        else if (healthPercent >= 50f)
            healthTMP.color = Color.yellow;
        else if (healthPercent >= 25f)
            healthTMP.color = new Color(1f, 0.64f, 0f); // naranja
        else
            healthTMP.color = Color.red;

        healthTMP.text = entity.entityName; // muestra el nombre del enemigo
    }
}
