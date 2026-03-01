using UnityEngine;
using UnityEngine.UI;

public class TargetableEntity : MonoBehaviour
{
    public Image image;
    private Color originalColor;

    public string characterName;

    private void Awake()
    {
        if (image != null)
            originalColor = image.color;
    }

    public void OnHoverEnter()
    {
        if (image != null)
            image.color = Color.red;
    }

    public void OnHoverExit()
    {
        if (image != null)
            image.color = originalColor;
    }

    public void OnSelected()
    {
        StatusUISwitcher.Instance?.Show(characterName);
    }
}