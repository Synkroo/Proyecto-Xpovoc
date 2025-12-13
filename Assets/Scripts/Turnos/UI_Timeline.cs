using System.Collections.Generic;
using UnityEngine;

public class UI_Timeline : MonoBehaviour
{
    public static UI_Timeline Instance;

    public List<TimelineRombo> rombos;
    public Sprite interrogationIcon;

    void Awake() => Instance = this;

    public void UpdateTimeline(List<BaseEntity> order)
    {
        for (int i = 0; i < rombos.Count; i++)
        {
            if (i < order.Count)
            {
                rombos[i].SetEntity(order[i]);
            }
            else
            {
                rombos[i].Clear();
                rombos[i].icon.sprite = interrogationIcon;
                rombos[i].icon.enabled = true;
            }
        }
    }
}
