using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;

public class UIManager : MonoBehaviour
{
    /*
    We want to make sure there is only one instance of this
    We are creating a private static instance of our UIManager
    and a public static Property to control the instance.
     */
    #region Variables
    private static UIManager _uiManagerInstance;
    [Header("Connect")]
    [SerializeField] private GameObject _connectUI;
    [SerializeField] private InputField _usernameField;

    #endregion
    #region Properties
    public static UIManager UIManagerInstance
    {
        //Property Read is public by default and readys the instance
        get => _uiManagerInstance;
        private set
        {
            //Property private write sets the instance to the value if the instance is null
            if (_uiManagerInstance == null)
            {
                _uiManagerInstance = value;
            }
            //Property checks for already existing NetworkManagers and if the instance doesn't match it destroys it :)
            else if (_uiManagerInstance != value)
            {
                Debug.LogWarning($"{nameof(UIManager)} instance already exists, destroy duplicate!");
                Destroy(value);
            }
        }
    }
    #endregion

    private void Awake()
    {
        //Sets the singleton to this
        UIManagerInstance = this;
    }

    public void ConnectClicker()
    {
        _usernameField.interactable = false;
        _connectUI.SetActive(false);

        NetworkManager.NetworkManagerInstance.Connect();
    }

    public void BackToMain()
    {
        _usernameField.interactable = true;
        _connectUI.SetActive(true);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.name);
        message.AddString(_usernameField.text);
        NetworkManager.NetworkManagerInstance.GameClient.Send(message);
    }
}
