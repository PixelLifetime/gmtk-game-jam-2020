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

		AudioSourceController audioSourceController = this._audioSourcePlayer.Play(audioClip: this._audioClip);

		ObjectPool.Instance.Release(audioSourceController);
	}
}
