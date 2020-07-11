using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrip : MonoBehaviour
{
    private List<GripLight> _currentLights;
    void Start()
    {
        _currentLights = new List<GripLight>();
    }

    private void FixedUpdate()
    {
        var increment = 0f;
        foreach(var light in _currentLights)
        {
            increment = Mathf.Max(increment, light.GetGripIncrement(transform));
        }
        GripManager.Instance.IncrementGrip(increment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var light = other.gameObject.GetComponent<GripLight>();
        if (light != null)
        {
            _currentLights.Add(light);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var light = other.gameObject.GetComponent<GripLight>();
        if (light != null)
        {
            _currentLights.Remove(light);
        }
    }
}
