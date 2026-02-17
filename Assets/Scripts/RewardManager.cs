using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    private HashSet<string> grantedRewards = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsRewardGranted(string rewardId)
    {
        if (string.IsNullOrEmpty(rewardId))
            return false;

        return grantedRewards.Contains(rewardId);
    }

    public void MarkRewardGranted(string rewardId)
    {
        if (string.IsNullOrEmpty(rewardId))
            return;

        grantedRewards.Add(rewardId);
    }
}
