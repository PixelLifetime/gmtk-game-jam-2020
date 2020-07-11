using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviourSingleton<AudioPlayer>
{
	public enum Type
	{
		Ambience,
		Music,
		SoundEffect
	}

	[SerializeField] private AudioSourcePlayer[] _audioSourcePlayers = new AudioSourcePlayer[System.Enum.GetNames(typeof(Type)).Length];
	public AudioSourcePlayer[] _AudioSourcePlayers => this._audioSourcePlayers;

	public AudioSourceController Play(AudioClip audioClip, Type type) =>
		this._audioSourcePlayers[(int)type].Play(audioClip: audioClip);

	public AudioSourceController Play(AudioClip audioClip, float volume, Type type) =>
		this._audioSourcePlayers[(int)type].Play(audioClip: audioClip, volume: volume);

	public AudioSourceController PlayDelayed(AudioClip audioClip, float delay, Type type) =>
		this._audioSourcePlayers[(int)type].PlayDelayed(audioClip: audioClip, delay: delay);
}
