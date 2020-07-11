using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour, IPoolable
{
	[SerializeField] private AudioSourceControlGroup _audioSourceControlGroup;
	public AudioSourceControlGroup _AudioSourceControlGroup => this._audioSourceControlGroup;

	public AudioSource AudioSource { get; private set; }

	public float Volume { get; set; }

	public void Play(AudioClip audioClip)
	{
		this.AudioSource.volume = this.Volume * this._audioSourceControlGroup.Volume;
		this.AudioSource.clip = audioClip;

		this.AudioSource.Play();
	}

	public void Play(AudioClip audioClip, float volume)
	{
		this.Volume = volume;
		this.AudioSource.volume = this.Volume * this._audioSourceControlGroup.Volume;
		this.AudioSource.clip = audioClip;

		this.AudioSource.Play();
	}

	public void PlayDelayed(AudioClip audioClip, float delay)
	{
		this.AudioSource.volume = this.Volume * this._audioSourceControlGroup.Volume;
		this.AudioSource.clip = audioClip;

		this.AudioSource.PlayDelayed(delay: delay);
	}

	public void Pause() => this.AudioSource.Pause();
	public void Resume() => this.AudioSource.UnPause();
	public void Stop() => this.AudioSource.Stop();

	public GameObject GameObject => this.gameObject;

	public ObjectPool.Pool Pool { get; set; }

	public void OnRelease()
	{
	}

	public void OnAquire()
	{
	}

	private void Awake()
	{
		this.AudioSource = this.GetComponent<AudioSource>();

		this.Volume = this.AudioSource.volume;
	}
}
