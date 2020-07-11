using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GripEffect : MonoBehaviour
{
    [SerializeField]
    private float _gripTreshold;
    [SerializeField]
    private bool _inversed;

    private bool _underThreshold;
    private void Start()
    {
        _underThreshold = GripManager.Instance.GetGrip() < _gripTreshold;
        ExecuteEffect();
    }
    void Update()
    {
        if (GripManager.Instance.GetGrip() < _gripTreshold && !_underThreshold)
        {
            _underThreshold = true;
            ExecuteEffect();
        } 
        else if (GripManager.Instance.GetGrip() >= _gripTreshold && _underThreshold)
        {
            _underThreshold = false;
            ExecuteEffect();
        }
    }
    private void ExecuteEffect()
    {
        if ((_underThreshold && !_inversed) || (!_underThreshold && _inversed))
        {
            DoEffect();
        }
        else
        {
            UndoEffect();
        }
    }
    protected abstract void DoEffect();
    protected abstract void UndoEffect();
}
