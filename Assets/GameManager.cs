using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
        else
        {
            Debug.LogError("Not connected to a room. Returning to main menu.");
            PhotonNetwork.LoadLevel("MainMenu");
        }
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 2f, 0f);
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerPrefab is not assigned in GameManager.");
        }
    }
}
