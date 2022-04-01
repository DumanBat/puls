using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private string _platformName;
    private int _platformType; // 1 - floor; -1 - ceiling
    private List<int> _spawnedObstaclePositions;
    private List<Obstacle> _spawnedObstacles;

    [SerializeField]
    private Transform _obstaclesRoot;
    [SerializeField]
    private Transform _nextPlatformSpawnPoint;

    public Transform GetSpawnPoint() => _nextPlatformSpawnPoint;
    public string GetPlatformName() => _platformName;
    public void SetPlatformName(string val) => _platformName = val; 

    public void SpawnObstacles(List<Obstacle> obstacles, int platformType)
    {
        _spawnedObstaclePositions = new List<int>();
        _spawnedObstacles = new List<Obstacle>();
        _platformType = platformType;

        foreach (var obstacleToSpawn in obstacles)
        {
            var xPos = GetObstacleSpawnPosition();
            var obstaclePos = new Vector3(xPos - 0.5f, transform.position.y - _platformType, transform.position.z);
            var obstacle = GetObstacle(obstacleToSpawn, obstaclePos);
            obstacle.transform.SetParent(_obstaclesRoot);

            _spawnedObstaclePositions.Add(xPos);
            _spawnedObstacles.Add(obstacle);

            StartCoroutine(MoveObstacle(obstacle));
        }
    }

    private Obstacle GetObstacle(Obstacle obstacleToSpawn, Vector3 position)
    {
        var obstacle = PoolController.Instance.GetPooledObstacle(obstacleToSpawn.name);
        obstacle.transform.position = position;
        return obstacle;
    }

    public int GetObstacleSpawnPosition()
    {
        var xPos = (int)transform.position.x + UnityEngine.Random.Range(-9, 10);

        return _spawnedObstaclePositions.Contains(xPos)
            ? GetObstacleSpawnPosition()
            : xPos;
    }

    public IEnumerator MoveObstacle(Obstacle obstacle)
    {
        while (!obstacle.IsGrounded(Vector2.down * _platformType))
        {
            var pos = obstacle.transform.position;
            obstacle.transform.position = new Vector3(pos.x, pos.y - (0.1f * _platformType), pos.z);
            yield return new WaitForEndOfFrame();

            if (Math.Abs(pos.y) > 5)
            {
                _spawnedObstacles.Remove(obstacle);
                PoolController.Instance.ReleasePooledObstacle(obstacle);
                break;
            }
        }
    }

    public void Unload()
    {
        if (_spawnedObstacles != null)
        {
            foreach (var obstacle in _spawnedObstacles)
                PoolController.Instance.ReleasePooledObstacle(obstacle);

            _spawnedObstacles.Clear();
        }

        if (_spawnedObstaclePositions != null)
            _spawnedObstaclePositions.Clear();
    }
}
