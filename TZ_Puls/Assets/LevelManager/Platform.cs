using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Transform _nextPlatformSpawnPoint;

    public Transform GetSpawnPoint() => _nextPlatformSpawnPoint;
}
