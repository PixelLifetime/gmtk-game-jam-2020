using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripLight : MonoBehaviour
{

    [SerializeField]
    private float _maxGripIncrement = 10f;
    [SerializeField]
    private float _minimumReachRadius = 1f;
    [SerializeField]
    private float _maximumReachRadius = 2f;
    [SerializeField]
    private AnimationCurve _incrementMultiplierCurve = AnimationCurve.Linear(0,0,1,1);

    /// <summary>
    /// Gets how much the grip will increment, depending of the distance between the player and the light
    /// </summary>
    /// <param name="target">The player's position</param>
    /// <returns></returns>
    public float GetGripIncrement(Transform target)
    {
        float distanceToTarget = Vector2.Distance(target.position, transform.position);
        if (distanceToTarget <= _minimumReachRadius)
        {
            return _maxGripIncrement;
        }
        if (distanceToTarget >+ _maximumReachRadius)
        {
            return 0f;
        }
        return _incrementMultiplierCurve.Evaluate(
            (distanceToTarget - _maximumReachRadius) / (_minimumReachRadius - _maximumReachRadius)) 
            * _maxGripIncrement;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _minimumReachRadius);
        Gizmos.DrawWireSphere(transform.position, _maximumReachRadius);
    }
}
