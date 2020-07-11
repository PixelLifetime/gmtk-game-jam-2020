using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerTests : MonoBehaviour
{
	[SerializeField] private AudioClipArchive _audioClipArchiveAmbience;
	public AudioClipArchive _AudioClipArchiveAmbience => this._audioClipArchiveAmbience;

	[SerializeField] private AudioClipArchive _audioClipArchiveMusic;
	public AudioClipArchive _AudioClipArchiveMusic => this._audioClipArchiveMusic;

	[SerializeField] private AudioClipArchive _audioClipArchiveSoundEffect;
	public AudioClipArchive _AudioClipArchiveSoundEffect => this._audioClipArchiveSoundEffect;

	private IEnumerator Start()
	{
		yield return new WaitForSecondsRealtime(1.0f);

		while (true)
		{
			AudioClip audioClip;
			AudioSourceController audioSourceController;

			// Ambience
			audioClip = this._audioClipArchiveAmbience.Random();
			audioSourceController = AudioPlayer.Instance.Play(audioClip: audioClip, type: AudioPlayer.Type.Ambience);

			yield return new WaitForSecondsRealtime(audioClip.length + 0.5f);

			ObjectPool.Instance.Release(audioSourceController);

			// Music
			audioClip = this._audioClipArchiveMusic.Random();
			audioSourceController = AudioPlayer.Instance.Play(audioClip: audioClip, type: AudioPlayer.Type.Music);

			yield return new WaitForSecondsRealtime(audioClip.length + 0.5f);

			ObjectPool.Instance.Release(audioSourceController);

			// Sound Effect
			audioClip = this._audioClipArchiveSoundEffect.Random();
			audioSourceController = AudioPlayer.Instance.Play(audioClip: audioClip, type: AudioPlayer.Type.SoundEffect);

			yield return new WaitForSecondsRealtime(audioClip.length + 0.5f);

			ObjectPool.Instance.Release(audioSourceController);
		}
	}
}
