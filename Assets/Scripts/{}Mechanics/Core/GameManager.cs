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

	[SerializeField] private SceneOperator _sceneOperator;

	private void Start()
    {
        if (OnKeyCollected == null)
        {
            OnKeyCollected = new UnityEvent();
        }
    }
    public void SetKeyCollected()
    {
        _hasCollectedKey = true;
        OnKeyCollected.Invoke();
    }

    public void Lose()
    {
        // Do something
        Debug.Log("Lose");
        // Just a quick reset for the moment
        GripManager.Instance.Reset();

		this._sceneOperator.ReloadScene();
    }

    public void EndLevel()
    {
        if (_hasCollectedKey)
        {
            Debug.Log("End level");
            // Just a quick reset for the moment
            GripManager.Instance.Reset();

			this._sceneOperator.LoadNextScene();
        }
    }
}
