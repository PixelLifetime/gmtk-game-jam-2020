using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Audio PlayerPrefs Configuration]", menuName = "Configurations/[Audio PlayerPrefs Configuration]", order = 0)]
public class AudioPlayerPrefsConfiguration : ScriptableObject
{
	[SerializeField] private string _masterAudioSourceVolumeKey = "masterVolume";
	public string _MasterAudioSourceVolumeKey => this._masterAudioSourceVolumeKey;

	[SerializeField] private string _soundEffectsAudioSourceVolumeKey = "musicVolume";
	public string _SoundEffectsAudioSourceVolumeKey => this._soundEffectsAudioSourceVolumeKey;

	[SerializeField] private string _musicAudioSourceVolumeKey = "soundEffectsVolume";
	public string _MusicAudioSourceVolumeKey => this._musicAudioSourceVolumeKey;
}
