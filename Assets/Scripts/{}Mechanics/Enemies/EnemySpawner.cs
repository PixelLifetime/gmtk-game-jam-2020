using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    //private bool _isActive;
    private float _spawnRate; // Rate at which enemy spawner creates enemies
    private float _timeUntilNextSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timeUntilNextSpawn -= Time.deltaTime;
        if(_timeUntilNextSpawn <= 0)
        {
            SpawnEnemy();
            _timeUntilNextSpawn = _spawnRate;
        }

    }

    private void SpawnEnemy()
    {
        // Spawn enemy prefab at spawn location
    }
}
