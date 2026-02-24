using UnityEngine;

public class Bar : MonoBehaviour
{
    public RectTransform Back;
    public RectTransform Front;

    private float currentValue;
    private float maxValue;
    private float originalWidth;

    void Start()
    {
        if (Front != null)
        {
            originalWidth = Front.sizeDelta.x;
            Front.pivot = new Vector2(0, 0.5f);
            Front.anchoredPosition = new Vector2(0, Front.anchoredPosition.y);
        }
        UpdateBar();
    }

    public void SetBar(float current, float max)
    {
        currentValue = current;
        maxValue = max;
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (Front == null)
        {
            Debug.LogWarning($"Bar Update falló: Front no asignado en {gameObject.name}");
            return;
        }

        float percent = maxValue > 0 ? Mathf.Clamp01(currentValue / maxValue) : 0f;
        Vector2 size = Front.sizeDelta;
        size.x = originalWidth * percent;
        Front.sizeDelta = size;
    }

}
