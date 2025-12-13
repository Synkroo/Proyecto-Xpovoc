using TMPro;
using UnityEngine;

public class DescripcionItem : MonoBehaviour
{
    public TextMeshProUGUI tmpHijo; // El tooltip
    private RectTransform rect;

    void Start()
    {
        if (tmpHijo != null)
            tmpHijo.gameObject.SetActive(false);

        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (tmpHijo == null || rect == null) return;

        Vector2 mousePos = Input.mousePosition;

        // Comprueba si el mouse está sobre el rectángulo del Button
        if (RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos, null))
        {
            tmpHijo.gameObject.SetActive(true);
        }
        else
        {
            tmpHijo.gameObject.SetActive(false);
        }
    }
}