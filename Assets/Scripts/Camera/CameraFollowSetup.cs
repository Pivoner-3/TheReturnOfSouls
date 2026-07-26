using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowSetup : MonoBehaviour
{
    public CinemachineCamera virtualCamera;

    void Start()
    {
        // Ищем игрока по тегу "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
        else
        {
            Debug.LogWarning("Камера не найдена или игрок не имеет тега Player!");
        }
    }
}