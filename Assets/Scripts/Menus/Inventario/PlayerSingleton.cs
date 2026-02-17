using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;
    public BaseEntity entity;

    private void Awake()
    {
        Instance = this;
        if (entity == null) entity = GetComponent<BaseEntity>();
    }
}