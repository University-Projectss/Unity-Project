using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public CountdownTimer countdownTimer;
    public CheckpointGenerator generator;
    public float timerGain;
    public int scoreGain;

    [SerializeField]
    private float _minRespawnTime;

    [SerializeField]
    private float _maxRespawnTime;

    [SerializeField]
    [Range(0f, 1f)]
    private float _respawnTimePercentage;

    [SerializeField]
    private Material _material;

    [SerializeField]
    protected GameObject _waypoint;

    [SerializeField]
    protected ScoreCounterSO _scoreCounter;

    private Material _instanceMaterial;

    protected virtual void Awake()
    {
        _instanceMaterial = Instantiate(_material);
        GetComponent<MeshRenderer>().material = _instanceMaterial;
        _waypoint.GetComponent<MeshRenderer>().material = _instanceMaterial;
    }

    protected virtual void Start()
    {
        float respawnTime = Mathf.Clamp(countdownTimer.RemainingTime * respawnTimePercentage, _minRespawnTime, _maxRespawnTime);
        StartCoroutine(RespawnCoroutine(respawnTime));
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag))
        {
            countdownTimer.AddTime(timerGain);
            _scoreCounter.score.checkpoints += 1;
            _scoreCounter.score.total += scoreGain;

            Vector3 direction = Vector3.ProjectOnPlane(other.attachedRigidbody.velocity, Vector3.down).normalized;
            generator.GenerateCheckpoint(direction, other.attachedRigidbody.position, this);
            Destroy(gameObject);

            //Destroy only happens after the current Update Loop
            //So we disable the object to prevent multiple triggers
            gameObject.SetActive(false);
        }
    }

    private IEnumerator RespawnCoroutine(float respawnTime)
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(respawnTime / 100);
            _instanceMaterial.color = new Color(_instanceMaterial.color.r + 0.01f,
                                                _instanceMaterial.color.g - 0.01f,
                                                _instanceMaterial.color.b,
                                                _instanceMaterial.color.a);
        }

    Destroy(gameObject);
    gameObject.SetActive(false);
        generator.GenerateCheckpoint(generator.plane.transform.forward, generator.plane.transform.position, this);
    }
}