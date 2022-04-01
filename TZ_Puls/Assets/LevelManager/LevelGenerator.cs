using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<Platform> _ceilingPlatforms;
    [SerializeField]
    private List<Obstacle> _ceilingObstacles;

    [SerializeField]
    private List<Platform> _floorPlatforms;
    [SerializeField]
    private List<Obstacle> _floorObstacles;

    private Transform _nextPlatformSpawnPoint;
    private List<(Platform, Platform)> _spawnedPlatforms;   

    private bool _isActive;

    private void Update()
    {
        if (!_isActive)
            return;

        if (Vector3.Distance(PlayerController.Instance.GetPosition(), _nextPlatformSpawnPoint.position) < 25.0f)
            SpawnPlatforms();
    }

    public void Init()
    {
        var platforms = _ceilingPlatforms.Concat(_floorPlatforms).ToList();
        foreach (var platform in platforms)
            PoolController.Instance.CreatePlatformPool(platform);

        var obstacles = _ceilingObstacles.Concat(_floorObstacles).ToList();
        foreach (var obstacle in obstacles)
            PoolController.Instance.CreateObstaclePool(obstacle);

        var ceiling = GetPlatform(_ceilingPlatforms[0], Vector3.zero);
        var floor = GetPlatform(_floorPlatforms[0], Vector3.zero);

        _nextPlatformSpawnPoint = floor.GetSpawnPoint();
        _spawnedPlatforms = new List<(Platform, Platform)>();
        _spawnedPlatforms.Add((ceiling, floor));
        _isActive = true;
    }

    private void SpawnPlatforms()
    {
        if (_spawnedPlatforms.Count != 1)
            UnloadPreviousPlatforms();

        var ceilingToSpawn = _ceilingPlatforms[Random.Range(0, _ceilingPlatforms.Count)];
        var ceiling = GetPlatform(ceilingToSpawn, _nextPlatformSpawnPoint.position);
        var obstacleToSpawn = GetObstaclesToSpawn(_ceilingObstacles, Random.Range(0, 6));
        ceiling.SpawnObstacles(obstacleToSpawn, -1);

        var floorToSpawn = _floorPlatforms[Random.Range(0, _floorPlatforms.Count)];
        var floor = GetPlatform(floorToSpawn, _nextPlatformSpawnPoint.position);
        obstacleToSpawn = GetObstaclesToSpawn(_floorObstacles, Random.Range(0, 6));
        floor.SpawnObstacles(obstacleToSpawn, 1);

        _nextPlatformSpawnPoint = floor.GetSpawnPoint();
        _spawnedPlatforms.Add((ceiling, floor));
    }

    private void UnloadPreviousPlatforms()
    {
        var platformPair = _spawnedPlatforms[0];
        _spawnedPlatforms.Remove(platformPair);
        PoolController.Instance.ReleasePooledPlatform(platformPair.Item1);
        PoolController.Instance.ReleasePooledPlatform(platformPair.Item2);
    }

    private Platform GetPlatform(Platform platformToSpawn, Vector3 position)
    {
        var platform = PoolController.Instance.GetPooledPlatform(platformToSpawn.name);
        platform.transform.position = position;
        return platform;
    }

    private List<Obstacle> GetObstaclesToSpawn(List<Obstacle> obstaclePool, int amount)
    {
        var obstaclesToSpawn = new List<Obstacle>();

        for (int i = 0; i < amount; i++)
            obstaclesToSpawn.Add(obstaclePool[Random.Range(0, obstaclePool.Count)]);

        return obstaclesToSpawn;
    }

    public void Unload()
    {
        _isActive = false;

        foreach (var spawnedPlatform in _spawnedPlatforms)
        {
            PoolController.Instance.ReleasePooledPlatform(spawnedPlatform.Item1);
            PoolController.Instance.ReleasePooledPlatform(spawnedPlatform.Item2);
        }
        _spawnedPlatforms = null;
    }
}
