using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField]
    private Target _targetPrefab;

    [SerializeField]
    private CountdownTimer _countdownTimer;

    [SerializeField]
    private Transform _spawnCenter;

    [SerializeField]
    private float _spawnRadius;

    [SerializeField]
    private float _spawnHeight;

    [SerializeField]
    private float _heightVariance;

    [SerializeField]
    private float _spawnInterval;

    private float _timeSinceSpawn = 0f;

    private void Awake()
    {
        Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timeSinceSpawn += Time.fixedDeltaTime;

        if (_timeSinceSpawn >= _spawnInterval)
        {
            _timeSinceSpawn = 0;
           
            Vector3 position = new(_spawnCenter.position.x, _spawnHeight, _spawnCenter.position.z);
            Vector3 offset = new(Random.Range(-_spawnRadius, _spawnRadius),
                                         Random.Range(-_heightVariance, _heightVariance),
                                         Random.Range(-_spawnRadius, _spawnRadius));
            position += offset;

            var target = Instantiate(_targetPrefab, position, Quaternion.identity);
            target.countdownTimer = _countdownTimer;
        }
    }
}
