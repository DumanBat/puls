using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<Platform> _ceilingPlatforms;
    [SerializeField]
    private List<Platform> _floorPlatforms;

    [SerializeField]
    private Transform _lastPlatformEnd;

    private void Update()
    {
        if (Vector3.Distance(PlayerController.Instance.GetPosition(), _lastPlatformEnd.position) < 27.0f)
            SpawnPlatform();
    }

    public void SpawnPlatform()
    {
        var ceilingToSpawn = _ceilingPlatforms[Random.Range(0, _ceilingPlatforms.Count)];
        SpawnPlatform(ceilingToSpawn);

        var floorToSpawn = _floorPlatforms[Random.Range(0, _floorPlatforms.Count)];
        var currentPlatform = SpawnPlatform(floorToSpawn);

        _lastPlatformEnd = currentPlatform.GetSpawnPoint();
    }

    public Platform SpawnPlatform(Platform platformToSpawn)
    {
        var platform = Instantiate(platformToSpawn, _lastPlatformEnd.position, Quaternion.identity);
        return platform;
    }
}
