using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : GripEffect
{
    public enum Behavior { }

    [SerializeField]
    private GameObject[] _enemiesICanSpawn;
    [SerializeField]
    private Enemy.EnemyBehavior _enemyBehavior;
    [SerializeField]
    private float _spawnRate; // Rate at which enemy spawner creates enemies
    [SerializeField]
    private int _maxEnemiesICanSpawn;
    [SerializeField]
    private bool _killEnemiesOnRegainedGriip;

    private Node[] _nodeGrid;
    private GameObject _player;
    private bool _canSpawn;
    private float _timeUntilNextSpawn;
    private List<GameObject> _mySpawnedEnemies;

    protected override void DoEffect()
    {
        _canSpawn = true;
        foreach (GameObject go in _mySpawnedEnemies)
        {
            go.SetActive(true);
            DOTween.ToAlpha(() => go.GetComponent<SpriteRenderer>().color, x => go.GetComponent<SpriteRenderer>().color = x, 1, 1);
        }
    }

    protected override void UndoEffect()
    {
        if (_killEnemiesOnRegainedGriip)
        {
            foreach (GameObject go in _mySpawnedEnemies)
            {
                Destroy(go);
            }
            _mySpawnedEnemies = new List<GameObject>();
            _timeUntilNextSpawn = 0f;
        }
        else
        {
            foreach (GameObject go in _mySpawnedEnemies)
            {
                DOTween.ToAlpha(() => go.GetComponent<SpriteRenderer>().color, x => go.GetComponent<SpriteRenderer>().color = x, 0, 0.5f);
            }
            Invoke("DisableMyEnemies", 0.5f);
        }

        _canSpawn = false;
    }

    private void DisableMyEnemies()
    {
        foreach (GameObject go in _mySpawnedEnemies)
        {
            go.SetActive(false);
        }
    }

    void Awake()
    {
        _player = GameObject.Find("[Player] Main");
        _nodeGrid = GameObject.Find("[AI] Node Grid").GetComponentsInChildren<Node>();
        _canSpawn = false;
        _mySpawnedEnemies = new List<GameObject>();
        _timeUntilNextSpawn = 0f;
    }

    void FixedUpdate()
    {
        if (_canSpawn && _maxEnemiesICanSpawn > _mySpawnedEnemies.Count)
        {
            _timeUntilNextSpawn -= Time.deltaTime;
            if (_timeUntilNextSpawn <= 0)
            {
                _mySpawnedEnemies.Add(SpawnEnemy());
                _timeUntilNextSpawn = _spawnRate;
            }
        }
    }

    private GameObject SpawnEnemy()
    {
        GameObject go = Instantiate(_enemiesICanSpawn[UnityEngine.Random.Range(0, _enemiesICanSpawn.Length)]);
        Color rgbs = go.GetComponent<SpriteRenderer>().color;
        go.GetComponent<SpriteRenderer>().color = new Color(rgbs.r, rgbs.g, rgbs.b, 0f);
        DOTween.ToAlpha(() => go.GetComponent<SpriteRenderer>().color, x => go.GetComponent<SpriteRenderer>().color = x, 1, 1);
        Enemy enemyScript = go.GetComponent<Enemy>();
        enemyScript.InitializeEnemyData(_nodeGrid, _player, _enemyBehavior);
        return go;
    }
}
