using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowSetup : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        // Подписываемся на поиск игрока
        Invoke(nameof(FindPlayer), 0.1f);
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
            Debug.Log("Камера привязана к игроку!");
        }
        else
        {
            // Если игрок ещё не создан — пробуем снова через 0.2 секунды
            Invoke(nameof(FindPlayer), 0.2f);
        }
    }
}