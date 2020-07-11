using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePlayerArchiveTests : MonoBehaviour
{
	[SerializeField] private AudioClipArchive _audioClipArchive;
	public AudioClipArchive _AudioClipArchive => this._audioClipArchive;

	[SerializeField] private AudioSourcePlayer _audioSourcePlayer;
	public AudioSourcePlayer _AudioSourcePlayer => this._audioSourcePlayer;

	private IEnumerator Start()
	{
		yield return new WaitForSecondsRealtime(1.0f);

		while (true)
		{
			AudioClip audioClip = this._audioClipArchive.Random();

			AudioSourceController audioSourceController = this._audioSourcePlayer.Play(audioClip: audioClip);

			yield return new WaitForSecondsRealtime(audioClip.length + 0.5f);

			ObjectPool.Instance.Release(audioSourceController);
		}
	}
}
