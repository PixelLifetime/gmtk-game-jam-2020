using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePlayerTests : MonoBehaviour
{
	[SerializeField] private AudioClip _audioClip;
	public AudioClip _AudioClip => this._audioClip;

	[SerializeField] private AudioSourcePlayer _audioSourcePlayer;
	public AudioSourcePlayer _AudioSourcePlayer => this._audioSourcePlayer;

	private IEnumerator Start()
	{
		yield return new WaitForSecondsRealtime(1.0f);

		while (true)
		{
			AudioSourceController audioSourceController = this._audioSourcePlayer.Play(audioClip: this._audioClip);

			yield return new WaitForSecondsRealtime(this._audioClip.length + 0.5f);

			ObjectPool.Instance.Release(audioSourceController);
		}
	}
}
