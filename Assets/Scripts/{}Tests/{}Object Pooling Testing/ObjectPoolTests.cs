using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTests : MonoBehaviour
{
	[SerializeField] private GameObject _prefab;
	public GameObject _Prefab => this._prefab;

	private void Start()
	{
		for (int a = 0; a < 60; a++)
		{
			Poolable poolable = ObjectPool.Instance.Aquire<Poolable>(gameObject: this._prefab);

			Debug.Log($"Pool InstanceId: {poolable.Pool.InstanceId}");
			Debug.Log($"GameObject InstanceId: {poolable.GameObject.GetInstanceID()}");

			ObjectPool.Instance.Release(poolable: poolable);
		}

		for (int a = 0; a < 30; a++)
		{
			Poolable poolable = ObjectPool.Instance.Aquire<Poolable>(gameObject: this._prefab);

			Debug.Log($"Pool InstanceId: {poolable.Pool.InstanceId}");
			Debug.Log($"GameObject InstanceId: {poolable.GameObject.GetInstanceID()}");

			poolable.GameObject.transform.position = new Vector3(x: 235.3f, y: 0.0f, z: 32.3f);
		}
	}
}
