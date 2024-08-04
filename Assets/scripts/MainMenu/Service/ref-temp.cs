using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class NetworkCheck : MonoBehaviour
{
    public NetworkManager networkManager;
    public GameObject waitingRoomUI;
    public GameObject startGameButton;
    public int port = 7777;

    private void Start()
    {
        startGameButton.GetComponent<Button>().onClick.AddListener(OnStartGameButtonClicked);
    }

    private void OnStartGameButtonClicked()
    {
        CheckServer();
    }

    private void CheckServer()
    {
        if (IsPortInUse(port))
        {
            // Start as client
            networkManager.StartClient();
        }
        else
        {
            // Start as host
            networkManager.StartHost();
        }

        // Show the waiting room UI
        waitingRoomUI.SetActive(true);
        startGameButton.SetActive(false);

        // Listen for client connections
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private bool IsPortInUse(int port)
    {
        bool isAvailable = true;

        // Check if the port is in use
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.Stop();
        }
        catch (SocketException)
        {
            isAvailable = false;
        }

        return !isAvailable;
    }

    private void OnClientConnected(ulong clientId)
    {
        // Check if there are both a host and a client connected
        if (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            // Load the game scene
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
