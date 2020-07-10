using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool : MonoBehaviourSingleton<ObjectPool>
{
	[Serializable]
	public struct InitializationData
	{
		[SerializeField] private GameObject _originalInstance;
		public GameObject _OriginalInstance => this._originalInstance;

		[SerializeField] private int _initialPoolCapacity;
		public int _InitialPoolCapacity => this._initialPoolCapacity;
	}
	
	public class Pool
	{
		private GameObject _originalInstance;

		public int InitialCapacity { get; }
		public int InstanceId { get; }

		private Queue<IPoolable> _poolables;

		public Pipeline<GameObject, GameObject> ReleasePipeline { get; private set; } = new Pipeline<GameObject, GameObject>();
		public Pipeline<GameObject, GameObject> AcquirePipeline { get; private set; } = new Pipeline<GameObject, GameObject>();

		/// <summary>
		/// Populates poolables queue with new instances of `_originalInstance`.
		/// </summary>
		/// <param name="quantity">Quantity of new instances.</param>
		private void Populate(int quantity)
		{
			for (int b = 0; b < quantity; b++)
			{
				IPoolable poolable = Object.Instantiate(original: this._originalInstance).GetComponent<IPoolable>();

				poolable.Pool = this;

				poolable.OnRelease();

				this.ReleasePipeline.Execute(value: poolable.GameObject);

				this._poolables.Enqueue(item: poolable);
			}
		}

		/// <summary>
		/// "Releases"[adds object back to the queue] object back to the pool.
		/// Calls `OnRelease` of the `poolable`.
		/// </summary>
		/// <param name="poolable"></param>
		internal void Release(IPoolable poolable)
		{
			poolable.OnRelease();

			this.ReleasePipeline.Execute(value: poolable.GameObject);

			this._poolables.Enqueue(item: poolable);
		}

		/// <summary>
		/// "Aquires"[dequeues object from the queue] object from the pool.
		/// Calls `OnAquire` of the `poolable`.
		/// </summary>
		/// <returns></returns>
		internal IPoolable Aquire()
		{
			if (this._poolables.Count == 0)
				this.Populate(quantity: this.InitialCapacity);

			IPoolable poolable = this._poolables.Dequeue();

			poolable.OnAquire();

			this.AcquirePipeline.Execute(value: poolable.GameObject);

			return poolable;
		}

		private void Configure()
		{
			this.ReleasePipeline.Add(
				middleware: new ActivationMiddleware(active: false)
			);
			this.ReleasePipeline.Add(
				middleware: new PositionMiddleware(position: Vector3.zero)
			);

			this.AcquirePipeline.Add(
				middleware: new ActivationMiddleware(active: true)
			);
		}

		public Pool(GameObject originalInstance, int initialCapacity)
		{
			this._originalInstance = originalInstance;

#if UNITY_EDITOR
			if (this._originalInstance.GetComponent<IPoolable>() == null)
				Debug.LogError("Object must implement `IPoolable` in order to be added to the ObjectPool.");
#endif

			this.InitialCapacity = initialCapacity;
			this.InstanceId = this._originalInstance.GetInstanceID();

			this._poolables = new Queue<IPoolable>(capacity: this.InitialCapacity);

			this.Populate(quantity: this.InitialCapacity);

			ObjectPool.Instance._instanceId_pool.Add(
				key: this.InstanceId,
				value: this
			);
		}

		public Pool(GameObject originalInstance)
			: this(originalInstance: originalInstance, initialCapacity: ObjectPool.Instance._defaultInitialPoolCapacity)
		{
		}
	}

	/// <summary>
	/// "Releases" object back to its respective pool.
	/// </summary>
	/// <param name="poolable">Object to release back to the pool.</param>
	public void Release(IPoolable poolable) => poolable.Pool.Release(poolable: poolable);

	/// <summary>
	/// "Releases" object back to its respective pool.
	/// </summary>
	/// <param name="gameObject">GameObject to release back to the pool.</param>
	public void Release(GameObject gameObject)
	{
		IPoolable poolable = gameObject.GetComponent<IPoolable>();

		poolable.Pool.Release(poolable: poolable);
	}

	/// <summary>
	/// "Aquires" object from its respective pool.
	/// Creates a new `Pool` and takes `poolable.GameObject` as its original instance in case the `Pool` is null.
	/// Pools using original instance id.
	/// Recommended to pass `poolable` that already has a pool unless you want to create a new one and use this one as original instance.
	/// </summary>
	/// <param name="poolable">Original instance or poolable with pool.</param>
	/// <returns>GameObject instance of aquired poolable.</returns>
	public GameObject Aquire(IPoolable poolable)
	{
		if (poolable.Pool == null)
			poolable.Pool = new Pool(originalInstance: poolable.GameObject);

		return poolable.Pool.Aquire().GameObject;
	}

	/// <summary>
	/// "Aquires" object from its respective pool.
	/// Creates a new `Pool` and takes `gameObject` as its original instance in case the `Pool` is null.
	/// Pools using original instance id.
	/// Recommended to pass `poolable` that already has a pool unless you want to create a new one and use this one as original instance.
	/// </summary>
	/// <param name="gameObject">Original instance or poolable with pool.</param>
	/// <returns>GameObject instance of aquired poolable.</returns>
	public GameObject Aquire(GameObject gameObject) => this.Aquire(poolable: gameObject.GetComponent<IPoolable>());

	/// <summary>
	/// "Aquires" component from its respective pool and.
	/// Creates a new `Pool` and takes `poolable.GameObject` as its original instance in case the `Pool` is null.
	/// Pools using original instance id.
	/// Recommended to pass `poolable` that already has a pool unless you want to create a new one and use this one as original instance.
	/// </summary>
	/// <typeparam name="TObject">Component type.</typeparam>
	/// <param name="poolable">Original instance or poolable with pool.</param>
	/// <returns>Component instance of aquired poolable.</returns>
	public TObject Aquire<TObject>(IPoolable poolable) => this.Aquire(poolable: poolable).GetComponent<TObject>();

	/// <summary>
	/// "Aquires" component from its respective pool and.
	/// Creates a new `Pool` and takes `poolable.GameObject` as its original instance in case the `Pool` is null.
	/// Pools using original instance id.
	/// Recommended to pass `poolable` that already has a pool unless you want to create a new one and use this one as original instance.
	/// </summary>
	/// <typeparam name="TObject">Component type.</typeparam>
	/// <param name="gameObject">Original instance or poolable with pool.</param>
	/// <returns>Component instance of aquired poolable.</returns>
	public TObject Aquire<TObject>(GameObject gameObject) => this.Aquire(gameObject: gameObject).GetComponent<TObject>();

	[SerializeField] private int _defaultInitialPoolCapacity;
	public int _DefaultInitialPoolSize => this._defaultInitialPoolCapacity;

	private Dictionary<int, Pool> _instanceId_pool;

	[SerializeField] private InitializationData[] _initializationData;
	public InitializationData[] _InitializationData => this._initializationData;

	public void Initialize(InitializationData[] initializationData)
	{
		this._instanceId_pool = new Dictionary<int, Pool>();

		for (int a = 0; a < initializationData.Length; a++)
		{
			Pool pool = new Pool(
				originalInstance: initializationData[a]._OriginalInstance,
				initialCapacity: initializationData[a]._InitialPoolCapacity
			);
		}
	}

	protected override void Awake()
	{
		base.Awake();

		this.Initialize(initializationData: this._initializationData);
	}
}
