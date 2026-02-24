using UnityEngine;

public class StatBar : MonoBehaviour
{
    public Bar bar;
    public int characterIndex;
    public StatsEnum stat;

    void Update()
    {
        var stats = StatsManager.Instance.characters[characterIndex];

        float current = 0f;
        float max = 0f;

        switch (stat)
        {
            case StatsEnum.Health:
                current = stats.health;
                max = stats.maxHealth;
                break;
            case StatsEnum.Mana:
                current = stats.mana;
                max = stats.maxMana;
                break;
        }

        bar.SetBar(current, max);

    }
}
