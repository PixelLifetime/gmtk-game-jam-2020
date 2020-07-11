using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityEventFloat : UnityEvent<float> { }

[CreateAssetMenu(fileName = "[Audio Source Control Group]", menuName = "[Audio]/[Audio Source Control Group]")]
public class AudioSourceControlGroup : ScriptableObject
{
	[SerializeField] private UnityEventFloat _onVolumeChange;
	public UnityEventFloat OnVolumeChange => this._onVolumeChange;

	[Range(0.0f, 1.0f)]
	[SerializeField] private float _volume = 1.0f;
	public float Volume
	{
		get => this._volume;
		set
		{
			this._volume = Mathf.Clamp01(value: value);

			this._onVolumeChange.Invoke(arg0: this._volume);
		}
	}
}
