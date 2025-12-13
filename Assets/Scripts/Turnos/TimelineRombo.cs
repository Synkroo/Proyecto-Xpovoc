using UnityEngine;
using UnityEngine.UI;

public class TimelineRombo : MonoBehaviour
{
    [Header("Imagen del rombo")]
    public Image icon;

    [HideInInspector]
    public BaseEntity entity;

    public void SetEntity(BaseEntity e)
    {
        entity = e;

        icon.enabled = true;

        if (e.turnIcon != null)
        {
            icon.sprite = e.turnIcon;
        }
        else
        {
            SpriteRenderer sr = e.GetComponent<SpriteRenderer>();
            if (sr != null) icon.sprite = sr.sprite;
            else icon.sprite = null;
        }

    }

    public void Clear()
    {
        entity = null;

        if (icon != null)
        {
            icon.enabled = false;
            icon.sprite = null;
        }
    }
}
