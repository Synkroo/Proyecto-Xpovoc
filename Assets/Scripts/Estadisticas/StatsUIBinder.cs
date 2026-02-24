using UnityEngine;
using UnityEngine.UI;

public class StatsUIBinder : MonoBehaviour
{
    public int characterIndex;

    public Image healthFill;
    public Image manaFill;

    void Start()
    {
        var gm = StatsManager.Instance;
        var stats = gm.characters[characterIndex];

        healthFill.fillAmount = stats.maxHealth > 0
            ? stats.health / stats.maxHealth
            : 0f;

        manaFill.fillAmount = stats.maxMana > 0
            ? stats.mana / stats.maxMana
            : 0f;
    }
}
