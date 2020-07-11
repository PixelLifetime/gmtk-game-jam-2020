using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Audio Source Control Group]", menuName = "[Audio]/[Audio Source Control Group]")]
public class AudioSourceControlGroup : ScriptableObject
{
	public event Action<float> OnVolumeChange = delegate { };

	[Range(0.0f, 1.0f)]
	[SerializeField] private float _volume = 1.0f;
	public float Volume
	{
		get => this._volume;
		set
		{
			this._volume = Mathf.Clamp01(value: value);

			this.OnVolumeChange.Invoke(obj: this._volume);
		}
	}
}
