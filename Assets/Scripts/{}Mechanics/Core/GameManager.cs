using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [HideInInspector]
    public UnityEvent OnKeyCollected;
    private bool _hasCollectedKey;

    private void Start()
    {
        OnKeyCollected = new UnityEvent();
    }
    public void SetKeyCollected()
    {
        _hasCollectedKey = true;
        OnKeyCollected.Invoke();
    }

    public void EndLevel()
    {
        if (_hasCollectedKey)
        {
            Debug.Log("End level");
            // Just a quick reset for the moment
            GripManager.Instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
