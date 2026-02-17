using Unity.Cinemachine;
using UnityEngine;

public class CinemachineTargetSetter : MonoBehaviour
{
    void Start()
    {
        CinemachineCamera cam = GetComponent<CinemachineCamera>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            cam.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("Player no encontrado para Cinemachine");
        }
    }
}