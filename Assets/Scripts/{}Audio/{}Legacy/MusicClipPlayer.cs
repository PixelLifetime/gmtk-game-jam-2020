using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClipPlayer : MonoBehaviour
{
	public void Play(AudioClip audioClip)
	{
		MusicAudioSource.Instance.Play(audioClip: audioClip);
	}

	[SerializeField] private AudioClip _audioClip;
	public AudioClip _AudioClip => this._audioClip;

	private void Awake()
	{
		MusicAudioSource.Instance.Play(this._audioClip);
	}
}
