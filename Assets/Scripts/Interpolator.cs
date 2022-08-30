using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _timeElapsed = 0f;
    [SerializeField] private float _timeToReachTarget = 0.05f;
    [SerializeField] private float _movementThreshold = 0.05f;

    private readonly List<TransformUpdate> _futureTransformUpdates = new List<TransformUpdate>();
    private float _squareMovementThreshold;
    private TransformUpdate to;
    private TransformUpdate from;
    private TransformUpdate previous;
    #endregion

    private void Start()
    {
        _squareMovementThreshold = _movementThreshold * _movementThreshold;
        to = new TransformUpdate(NetworkManager.NetworkManagerInstance.ServerTick, transform.position);
        from = new TransformUpdate(NetworkManager.NetworkManagerInstance.InterpolationTick, transform.position);
        previous = new TransformUpdate(NetworkManager.NetworkManagerInstance.InterpolationTick, transform.position);
    }

    private void Update()
    {
        for (int i = 0; i < _futureTransformUpdates.Count; i++)
        {
            if(NetworkManager.NetworkManagerInstance.ServerTick >-_futureTransformUpdates[i].Tick)
            {
                previous = to;
                to = _futureTransformUpdates[i];
                from = new TransformUpdate(NetworkManager.NetworkManagerInstance.InterpolationTick, transform.position);

                _futureTransformUpdates.RemoveAt(i);
                i--;
                _timeElapsed = 0f;
                _timeToReachTarget = (to.Tick - from.Tick) * Time.fixedDeltaTime;
            }
        }

        _timeElapsed += Time.deltaTime;
        InterpolatePosition(_timeElapsed / _timeToReachTarget);
    }

    private void InterpolatePosition(float lerpAmount)
    {
        if((to.Position - previous.Position).sqrMagnitude < _squareMovementThreshold)
        {
            if (to.Position != from.Position)
                transform.position = Vector3.Lerp(from.Position, to.Position, lerpAmount);

            return;
        }

        transform.position = Vector3.LerpUnclamped(from.Position, to.Position, lerpAmount);
    }

    public void NewUpdate(ushort tick, Vector3 position)
    {
        if (tick <= NetworkManager.NetworkManagerInstance.InterpolationTick)
            return;

        for (int i = 0; i < _futureTransformUpdates.Count; i++)
        {
            if(tick < _futureTransformUpdates[i].Tick)
            {
                _futureTransformUpdates.Insert(i, new TransformUpdate(tick, position));
                return;
            }
        }

        _futureTransformUpdates.Add(new TransformUpdate(tick, position));
    }
}
