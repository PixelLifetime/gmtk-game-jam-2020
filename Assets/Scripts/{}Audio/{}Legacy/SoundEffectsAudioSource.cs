using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsAudioSource : AudioSourceSingleton<SoundEffectsAudioSource>
{
	//private const string _VOLUME_KEY = "soundEffectsVolume";

	[SerializeField] private AudioPlayerPrefsConfiguration _audioPlayerPrefsConfiguration;

	protected override string volumeKey => this._audioPlayerPrefsConfiguration._SoundEffectsAudioSourceVolumeKey;
}
