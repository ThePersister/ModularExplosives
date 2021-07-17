using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : Singleton<MineSpawner>
{
    [Header("Spawner")]
    [SerializeField]
    private GameObject _minePrefab;

    [SerializeField]
    private float _spawnOffsetXZ = 20f;

    [SerializeField]
    private float _spawnHeight = 10f;

    [SerializeField]
    private float _maxSpawnDelay = 2f;

    [SerializeField]
    private AnimationCurve _spawnCurve;

    [Header("Throw")]
    [SerializeField]
    private float _minThrowSpeed = 4f;

    [SerializeField]
    private float _maxThrowSpeed = 10f;

    [SerializeField]
    private float _floorThrowArea01 = 0.6f;

    private void Start()
    {
        StartCoroutine(KeepSpawningMines());
    }

    private IEnumerator KeepSpawningMines()
    {
        yield return new WaitForSeconds(_maxSpawnDelay * _spawnCurve.Evaluate(1.0f - GameManager.Instance.TimeLeft01));
        SpawnMine();

        yield return new WaitUntil(GameNoLongerPaused);
        yield return KeepSpawningMines();
    }

    private bool GameNoLongerPaused()
    {
        return !GameManager.Instance.IsPaused;
    }


    private void SpawnMine()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-1.0f, 1.0f) * _spawnOffsetXZ, Random.value * _spawnHeight, Random.Range(-1.0f, 1.0f) * _spawnOffsetXZ);
        GameObject newMine = GameObject.Instantiate(_minePrefab, spawnPosition, Quaternion.identity, transform);

        Vector3 target = new Vector3(Random.Range(-0.5f, 0.5f) * _floorThrowArea01 * GameManager.Instance.FloorSize.x, Random.value * 3.0f, Random.Range(-0.5f, 0.5f) * _floorThrowArea01 * GameManager.Instance.FloorSize.z);
        Vector3 throwDirection = (target - spawnPosition).normalized;
        float randomThrowSpeed = Random.Range(_minThrowSpeed, _maxThrowSpeed);
        newMine.GetComponent<Rigidbody>().AddForce(throwDirection * randomThrowSpeed, ForceMode.Impulse);
        newMine.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * randomThrowSpeed, ForceMode.Impulse);
    }
}
