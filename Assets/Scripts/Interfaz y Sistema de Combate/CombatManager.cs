using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        if (player != null)
        {
            player.SetCombatMode(true);
        }
    }

    public void ExitCombat()
    {
        if (player != null)
        {
            player.SetCombatMode(false);
        }
    }
}
