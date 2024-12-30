using UnityEngine;
using UnityEngine.UI;  // Updated namespace for Text and UI elements
using TMPro; // TextMeshPro namespace
using Unity.Netcode;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button startButton;
    // public TMP_InputField playerNameInput; // InputField for player name
    // public Button submitNameButton;        // Button to submit the player name
    // public Transform playerListParent;    // Parent for the player list UI
    // public GameObject playerListItemPrefab; // Prefab for displaying each player's name

    private Dictionary<ulong, GameObject> playerListItems = new Dictionary<ulong, GameObject>();

    private void Start()
    {
        // Assign button listeners
        hostButton.onClick.AddListener(OnHostButtonClicked);
        clientButton.onClick.AddListener(OnClientButtonClicked);
        startButton.onClick.AddListener(OnStartButtonClicked);
        // submitNameButton.onClick.AddListener(OnSubmitNameButtonClicked);

        // Start button should only be visible to the host
        startButton.gameObject.SetActive(false);
        

        // Register network callbacks
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

        // Ensure player name input field is active
        // playerNameInput.gameObject.SetActive(false); // Initially hidden
        // submitNameButton.gameObject.SetActive(false); // Initially hidden
    }

    private void OnHostButtonClicked()
    {
        Debug.Log("Host button clicked!");
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started");
            hostButton.gameObject.SetActive(false);
            clientButton.gameObject.SetActive(false);

            // Show Start Button for the host
            startButton.gameObject.SetActive(true);

            // Show player name input field and submit button for the host
            // playerNameInput.gameObject.SetActive(true);
            // submitNameButton.gameObject.SetActive(true);
        }
    }

    private void OnClientButtonClicked()
    {
        Debug.Log("Client button clicked!");
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started");
            clientButton.gameObject.SetActive(false);
            hostButton.gameObject.SetActive(false);

            // Show player name input field and submit button for the client
            // playerNameInput.gameObject.SetActive(true);
            // submitNameButton.gameObject.SetActive(true);
        }
    }

    private void OnSubmitNameButtonClicked()
    {
        // Use player-provided name if set, else use default "Player {clientId}"
        // string playerName = playerNameInput.text.Length > 0 ? playerNameInput.text : $"Player {NetworkManager.Singleton.LocalClientId}";
        //
        // AddPlayerToList(NetworkManager.Singleton.LocalClientId, playerName);
        // playerNameInput.gameObject.SetActive(false); // Hide input field after submission
        // submitNameButton.gameObject.SetActive(false); // Hide submit button after submission
        hostButton.gameObject.SetActive(true);
        clientButton.gameObject.SetActive(true);
    }

    private void OnStartButtonClicked()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host clicked Start Game!");
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected.");

        // Use player-provided name if set, else use default "Player {clientId}"
        // string playerName = playerNameInput.text.Length > 0 ? playerNameInput.text : $"Player {clientId}";
        //
        // AddPlayerToList(clientId, playerName);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected.");
        // RemovePlayerFromList(clientId);
    }

    // private void AddPlayerToList(ulong clientId, string playerName)
    // {
    //     if (playerListItems.ContainsKey(clientId))
    //         return;
    //
    //     // Instantiate a new player list item
    //     GameObject playerItem = Instantiate(playerListItemPrefab, playerListParent);
    //     playerItem.GetComponent<Text>().text = playerName; // Use UnityEngine.UI.Text instead of TextMeshPro
    //     playerListItems[clientId] = playerItem;
    // }

    // private void RemovePlayerFromList(ulong clientId)
    // {
    //     if (playerListItems.TryGetValue(clientId, out GameObject playerItem))
    //     {
    //         Destroy(playerItem);
    //         playerListItems.Remove(clientId);
    //     }
    // }

    private void OnDestroy()
    {
        // Unregister callbacks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }
}