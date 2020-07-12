using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOperator : MonoBehaviour
{
	public void LoadNextScene()
	{
#if UNITY_EDITOR
		if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings)
			Debug.LogError("Trying to load next scene from scene with maximal build index.");
#endif

		SceneManager.LoadScene(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void LoadPreviousScene()
	{
#if UNITY_EDITOR
		if (SceneManager.GetActiveScene().buildIndex < 1)
			Debug.LogError("Trying to load previous scene from scene with build index of 0 or less.");
#endif

		SceneManager.LoadScene(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void LoadScene(int sceneBuildIndex) => SceneManager.LoadScene(sceneBuildIndex: sceneBuildIndex);
	public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName: sceneName);

	[SerializeField] private float _sceneLoadDelay = 0.4f;
	public float _SceneLoadDelay => this._sceneLoadDelay;

	private IEnumerator LoadSceneWithDelayProcess(int sceneBuildIndex)
	{
		yield return new WaitForSeconds(this._sceneLoadDelay);

		this.LoadScene(sceneBuildIndex: sceneBuildIndex);
	}

	public void LoadSceneWithDelay(int sceneBuildIndex) => 
		this.StartCoroutine(
			routine: this.LoadSceneWithDelayProcess(sceneBuildIndex: sceneBuildIndex)
		);

	private IEnumerator LoadNextSceneWithDelayProcess()
	{
		yield return new WaitForSeconds(this._sceneLoadDelay);

		this.LoadNextScene();
	}

	public void LoadNextSceneWithDelay() =>
		this.StartCoroutine(
			routine: this.LoadNextSceneWithDelayProcess()
		);
}
