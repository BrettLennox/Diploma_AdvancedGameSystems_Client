using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using System;

public enum ClientToServerId
{
    name = 1,
}

public class NetworkManager : MonoBehaviour
{
    /*
    We want to make sure there is only one instance of this
    We are creating a private static instance of our NetworkManager
    and a public static Property to control the instance.
     */
    #region Variables
    private static NetworkManager _networkManagerInstance;
    [SerializeField] private ushort s_port;
    [SerializeField] private string s_ip;
    #endregion
    #region Properties
    public static NetworkManager NetworkManagerInstance
    {
        //Property Read is public by default and readys the instance
        get => _networkManagerInstance;
        private set
        {
            //Property private write sets the instance to the value if the instance is null
            if (_networkManagerInstance == null)
            {
                _networkManagerInstance = value;
            }
            //Property checks for already existing NetworkManagers and if the instance doesn't match it destroys it :)
            else if (_networkManagerInstance != value)
            {
                Debug.LogWarning($"{nameof(NetworkManager)} instance already exists, destroy duplicate!");
                Destroy(value);
            }
        }
    }
    public Client GameClient { get; private set; }
    #endregion

    private void Awake()
    {
        //When the object that this script is attached to is activated in the game, set the instance to this and check to see if instance is already set
        NetworkManagerInstance = this;
    }

    private void Start()
    {
        //Logs what the network is doing
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        //Create new Client
        GameClient = new Client();
        //Connect
        GameClient.Connected += DidConnect;
        //Connection Failed
        GameClient.ConnectionFailed += FailedToConnect;
        //Disconnect
        GameClient.Disconnected += DidDisconnect;
    }
    #region Events
    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.UIManagerInstance.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.UIManagerInstance.BackToMain();
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.UIManagerInstance.BackToMain();
    }
    #endregion

    private void FixedUpdate()
    {
        GameClient.Tick();
    }

    private void OnApplicationQuit()
    {
        GameClient.Disconnect();
    }

    public void Connect()
    {
        //Connect to server
        GameClient.Connect($"{s_ip}:{s_port}");
    }
}
