using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManager : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "FinalBoss")
        {
            GameObject spawnPoint = GameObject.Find("PlayerSpawn");
            GameObject player = GameObject.Find("Player"); // already exists

            if (spawnPoint != null && player != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogError("Spawn point or persistent player is missing.");
            }
        }
    }
}
