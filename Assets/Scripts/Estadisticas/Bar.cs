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
        currentValue = Mathf.Clamp(current, 0, max);
        maxValue = max;
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (Front == null) return;
        float percent = currentValue / maxValue;
        Vector2 size = Front.sizeDelta;
        size.x = originalWidth * percent;
        Front.sizeDelta = size;
    }
}
