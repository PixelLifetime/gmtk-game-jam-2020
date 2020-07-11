using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour, IPoolable
{
	public GameObject GameObject => throw new System.NotImplementedException();

	public ObjectPool.Pool Pool { get; set; }

	public void OnRelease()
	{
	}

	public void OnAquire()
	{
	}
}
