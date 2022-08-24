using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _sensitivity = 100f;
    [SerializeField] private float _clampAngle = 85f;

    private float _verticalRotation;
    private float _horizontalRotation;

    private void OnValidate()
    {
        if (_player == null)
            _player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        if (_player == null)
            _player = GetComponentInParent<Player>();
        
        _verticalRotation = transform.localEulerAngles.x;
        _horizontalRotation = _player.transform.eulerAngles.y;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleCursorMode();

        if (Cursor.lockState == CursorLockMode.Locked)
            Look();
        
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);
    }

    private void Look()
    {
        float mouseVertical = -Input.GetAxis("Mouse Y");
        float mouseHorizontal = Input.GetAxis("Mouse X");

        _verticalRotation += mouseVertical * _sensitivity * Time.deltaTime;
        _horizontalRotation += mouseHorizontal * _sensitivity * Time.deltaTime;

        _verticalRotation = Mathf.Clamp(_verticalRotation, -_clampAngle, _clampAngle);
        
        transform.localRotation = Quaternion.Euler(_verticalRotation, 0f,0f);
        _player.transform.rotation = Quaternion.Euler(0f, _horizontalRotation,0f);
    }

    private void ToggleCursorMode()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
