using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolController : Singleton<PoolController>
{
    private Dictionary<string, ObjectPool<Platform>> _platformPoolDictionary = new Dictionary<string, ObjectPool<Platform>>();
    private Dictionary<string, ObjectPool<Obstacle>> _obstaclePoolDictionary = new Dictionary<string, ObjectPool<Obstacle>>();

    public void CreatePlatformPool(Platform platformPrefab)
    {
        if (_platformPoolDictionary.ContainsKey(platformPrefab.name))
            return;

        var pool = new ObjectPool<Platform>(() =>
        {
            var platform = Instantiate(platformPrefab);
            platform.SetPlatformName(platformPrefab.name);
            return platform;
        }, 
        platform => platform.gameObject.SetActive(true),
        platform =>
        {
            platform.Unload();
            platform.gameObject.SetActive(false);
        },
        platform => Destroy(platform.gameObject),
        false, 2, 3);

        _platformPoolDictionary.Add(platformPrefab.name, pool);
    }

    public Platform GetPooledPlatform(string name) => _platformPoolDictionary[name].Get();
    public void ReleasePooledPlatform(Platform platform) => _platformPoolDictionary[platform.GetPlatformName()].Release(platform);

    public void CreateObstaclePool(Obstacle obstaclePrefab)
    {
        if (_obstaclePoolDictionary.ContainsKey(obstaclePrefab.name))
            return;

        var pool = new ObjectPool<Obstacle>(() =>
        {
            var obstacle = Instantiate(obstaclePrefab);
            obstacle.SetObstaclename(obstaclePrefab.name);
            return obstacle;
        },
        obstacle => obstacle.gameObject.SetActive(true),
        obstacle =>
        {
            obstacle.gameObject.SetActive(false);
            obstacle.transform.position = Vector3.zero;
        },
        obstacle => Destroy(obstacle.gameObject),
        false, 5, 10);

        _obstaclePoolDictionary.Add(obstaclePrefab.name, pool);
    }
    public Obstacle GetPooledObstacle(string name) => _obstaclePoolDictionary[name].Get();
    public void ReleasePooledObstacle(Obstacle obstacle) => _obstaclePoolDictionary[obstacle.GetObstacleName()].Release(obstacle);
}
