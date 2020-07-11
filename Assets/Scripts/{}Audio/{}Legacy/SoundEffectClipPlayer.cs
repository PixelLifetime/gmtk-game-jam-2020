using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectClipPlayer : MonoBehaviour
{
	public void Play(AudioClip audioClip)
	{
		SoundEffectsAudioSource.Instance.Play(audioClip: audioClip);
	}
}