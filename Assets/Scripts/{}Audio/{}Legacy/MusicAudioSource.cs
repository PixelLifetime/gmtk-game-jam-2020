using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioSource : AudioSourceSingleton<MusicAudioSource>
{
	//private const string _VOLUME_KEY = "musicVolume";

	[SerializeField] private AudioPlayerPrefsConfiguration _audioPlayerPrefsConfiguration;

	protected override string volumeKey => this._audioPlayerPrefsConfiguration._MusicAudioSourceVolumeKey;
}
