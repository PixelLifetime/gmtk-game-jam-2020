using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripManager : MonoBehaviourSingleton<GripManager>
{
    [SerializeField]
    public float _maxGrip;

    [SerializeField]
    public float _gripDecrease;
    private float _grip;
    private void Start()
    {
        _grip = _maxGrip;
    }
    private void FixedUpdate()
    {
        DecrementGrip(_gripDecrease);
    }
    public void DecrementGrip(float gripDecrement)
    {
        _grip = Mathf.Max(_grip - gripDecrement, 0f);
    }
    public void IncrementGrip(float gripIncrement)
    {
        _grip = Mathf.Min(_grip + gripIncrement, _maxGrip);
    }
    public void Reset()
    {
        _grip = _maxGrip;
    }
    public float GetGrip()
    {
        return _grip;
    }
}
