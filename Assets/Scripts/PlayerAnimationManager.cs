using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _playerMoveSpeed;

    private float _sprintThreshold;
    private Vector3 _lastPosition;

    private void Start()
    {
        _sprintThreshold = _playerMoveSpeed * 1.5f * Time.fixedDeltaTime;
    }

    public void AnimateBasedOnSpeed()
    {
        _lastPosition.y = transform.position.y;
        float distanceMoved = Vector3.Distance(transform.position, _lastPosition);
        _animator.SetBool("IsMoving", distanceMoved > 0.01f);
        _animator.SetBool("IsSprinting", distanceMoved > _sprintThreshold);

        _lastPosition = transform.position;
    }
}
