using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAudioSource : AudioSourceSingleton<MasterAudioSource>
{
	private const string _GLOBAL_VOLUME_KEY = "masterGlobalVolume";

	[SerializeField] private AudioPlayerPrefsConfiguration _audioPlayerPrefsConfiguration;

	protected override string volumeKey => this._audioPlayerPrefsConfiguration._MasterAudioSourceVolumeKey;

	public void SetGlobalVolume(float volume)
	{
		AudioListener.volume = volume;

		PlayerPrefs.SetFloat(MasterAudioSource._GLOBAL_VOLUME_KEY, AudioListener.volume);
	}
}
