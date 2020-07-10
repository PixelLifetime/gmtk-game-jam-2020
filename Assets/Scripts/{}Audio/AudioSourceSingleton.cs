using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioSourceSingleton<T> : MonoBehaviour
	where T : AudioSourceSingleton<T>
{
	[SerializeField] protected AudioSource audioSource;
	public AudioSource _AudioSource => this.audioSource;

	protected abstract string volumeKey { get; }

	public virtual void SetVolume(float volume)
	{
		this.audioSource.volume = volume;

		PlayerPrefs.SetFloat(this.volumeKey, this.audioSource.volume);
	}

	public virtual void Play(AudioClip audioClip)
	{
		this.audioSource.clip = audioClip;

		this.audioSource.Play();
	}

	protected virtual void Initialize()
	{
		this.audioSource.volume = PlayerPrefs.GetFloat(this.volumeKey, 1.0f);
	}

	public static T Instance { get; private set; }

	private void Awake()
	{
		this.Initialize();

		if (Instance == null)
		{
			Instance = this as T;

			Object.DontDestroyOnLoad(Instance);
		}
		else
		{
			Object.Destroy(this.gameObject);
		}
	}

#if UNITY_EDITOR
	private void Reset()
	{
		this.audioSource = this.GetComponent<AudioSource>();
	}
#endif
}
