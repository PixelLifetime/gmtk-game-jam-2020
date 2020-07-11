using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrip : MonoBehaviour
{
    [SerializeField]
    private float _maxGrip;
    [SerializeField]
    private float _gripDecrement;
    private List<GripLight> _currentLights;
    private float _grip;
    void Start()
    {
        _grip = _maxGrip;
        _currentLights = new List<GripLight>();
    }

    private void FixedUpdate()
    {
        if (_currentLights.Count == 0)
        {
            _grip = Mathf.Max(_grip - _gripDecrement, 0f);
            Debug.Log("My grip is going down");
        }
        else
        {
            var increment = 0f;
            foreach(var light in _currentLights)
            {
                increment = Mathf.Max(increment, light.GetGripIncrement(transform));
            }
            Debug.Log("My grip is going up by " + increment);
            _grip = Mathf.Min(_grip + increment, _maxGrip);
        }
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
