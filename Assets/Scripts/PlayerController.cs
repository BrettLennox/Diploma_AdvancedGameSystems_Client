using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _camTransform;

    private bool[] inputs;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new bool[6];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            inputs[0] = true;
        
        if (Input.GetKey(KeyCode.S))
            inputs[1] = true;
        
        if (Input.GetKey(KeyCode.A))
            inputs[2] = true;
        
        if (Input.GetKey(KeyCode.D))
            inputs[3] = true;
        
        if (Input.GetKey(KeyCode.Space))
            inputs[4] = true;
        
        if (Input.GetKey(KeyCode.LeftShift))
            inputs[5] = true;
    }

    private void FixedUpdate()
    {
        SendInput();

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = false;
        }
    }

    #region Messages

    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.input);
        message.AddBools(inputs, false);
        message.AddVector3(_camTransform.forward);
        NetworkManager.NetworkManagerInstance.GameClient.Send(message);
    }
    

    #endregion
}
