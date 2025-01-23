using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomUIManager : MonoBehaviourPunCallbacks
{
    public InputField createRoomInputField;  // Input field for creating a room
    public InputField joinRoomInputField;    // Input field for joining a room
    public Text statusText;                  // Text to show connection status or errors

    void Start()
    {
        // Connect to Photon if not already connected
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            UpdateStatus("Connecting to Photon...");
        }
    }

    // Method to create a room
    public void CreateRoom()
    {
        if (createRoomInputField == null)
        {
            Debug.LogError("CreateRoomInputField is not assigned in the Inspector.");
            return;
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            string roomName = createRoomInputField.text;
            if (!string.IsNullOrEmpty(roomName))
            {
                PhotonNetwork.CreateRoom(roomName);
                UpdateStatus($"Creating Room: {roomName}...");
            }
            else
            {
                UpdateStatus("Room name cannot be empty.");
            }
        }
        else
        {
            UpdateStatus("Not connected to Photon.");
        }
    }

    // Method to join a room by name
    public void JoinRoom()
    {
        if (joinRoomInputField == null)
        {
            Debug.LogError("JoinRoomInputField is not assigned in the Inspector.");
            return;
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            string roomName = joinRoomInputField.text;
            if (!string.IsNullOrEmpty(roomName))
            {
                PhotonNetwork.JoinRoom(roomName);  // Try to join the room with the entered name
                UpdateStatus($"Joining Room: {roomName}...");
            }
            else
            {
                UpdateStatus("Room name cannot be empty.");
            }
        }
        else
        {
            UpdateStatus("Not connected to Photon.");
        }
    }

    // Method to join a random room
    public void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomRoom();  // Try to join a random room
            UpdateStatus("Joining a random room...");
        }
        else
        {
            UpdateStatus("Not connected to Photon.");
        }
    }

    // Method to update the status text
    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log(message);
    }

    // Callback for when the player successfully joins a room
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully.");
        PhotonNetwork.LoadLevel("SampleScene");  // Load the game scene after joining the room
    }

    // Callback for when the player fails to join a room
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
        UpdateStatus("Failed to join the room. Please try again.");
    }
}
