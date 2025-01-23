using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance; // Singleton instance for easy access

    [Header("Settings")]
    public string gameVersion = "1.0"; // Game version for matchmaking
    public string defaultRoomName = "Room"; // Default room name for quick join

    [Header("Prefabs")]
    public GameObject playerPrefab; // Player prefab to spawn in the game scene

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the NetworkManager across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate managers
        }
    }

    private void Start()
    {
        ConnectToPhoton();
    }

    /// <summary>
    /// Connect to Photon Servers
    /// </summary>
    public void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connecting to Photon...");
        }
    }

    /// <summary>
    /// Create a room with the specified name.
    /// </summary>
    public void CreateRoom(string roomName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 10 });
            Debug.Log($"Creating Room: {roomName}");
        }
        else
        {
            Debug.LogError("Cannot create room. Not connected to Photon.");
        }
    }

    /// <summary>
    /// Join a room with the specified name.
    /// </summary>
    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(roomName);
            Debug.Log($"Joining Room: {roomName}");
        }
        else
        {
            Debug.LogError("Cannot join room. Not connected to Photon.");
        }
    }

    /// <summary>
    /// Quick join: joins any available room or creates a new one.
    /// </summary>
    public void QuickJoin()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Attempting to join a random room...");
        }
        else
        {
            Debug.LogError("Cannot quick join. Not connected to Photon.");
        }
    }

    /// <summary>
    /// Spawns the player when the game starts.
    /// </summary>
    public void SpawnPlayer(Vector3 spawnPosition)
    {
        if (playerPrefab != null && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
            Debug.Log("Player spawned at position: " + spawnPosition);
        }
        else
        {
            Debug.LogError("Player prefab is not set or not connected to Photon.");
        }
    }

    // Callbacks for Photon events
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server. Joining Lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby. Ready to create or join a room.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("No random room available. Creating a new room...");
        CreateRoom(defaultRoomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Room creation failed: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Room joining failed: {message}");
    }
}
