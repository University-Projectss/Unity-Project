using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointGenerator : MonoBehaviour
{
    [Tooltip("Checkpoint Prefab to instantiate")]
    [SerializeField]
    private Checkpoint _checkpoint;

    [Tooltip("Portal Prefab to instantiate")]
    [SerializeField]
    private Portal _portal;

    [Tooltip("Dimension Switcher object")]
    public DimensionSwitcher switcher;

    [Tooltip("How often portals should spawn instead of normal checkpoints")]
    [SerializeField]
    private int _portalFrequency;

    [Tooltip("The Terrain Controller object")]
    public TerrainController terrainController;

    [Tooltip("Minimum placement distance in the movement direction")]
    [SerializeField]
    private float _minRadius;

    [Tooltip("Maximum placement distance in the movement direction")]
    [SerializeField]
    private float _maxRadius;

    [Tooltip("Placement radius perpendicular to the movement direction")]
    [SerializeField]
    private float _lateralRadius;

    [Tooltip("Water Object used for the minimum height")]
    [SerializeField]
    private Transform _waterLevel;

    [Tooltip("Maximum placement height, counting from water level")]
    [SerializeField]
    private float _heightRadius;

    [Tooltip("Portal placement height, counting from water level")]
    [SerializeField]
    private float _portalHeight;

    [Tooltip("Which layers constitute the terrain")]
    [SerializeField]
    private LayerMask _terrainLayers;

    [SerializeField]
    private GameObject _targetGenerator;

    [SerializeField]
    private Rigidbody _plane;

    [SerializeField]
    private int _cubeEventDurationSeconds;

    [Tooltip("Number of portals taken before a Cube Event")]
    [SerializeField]
    private int _cubeEventFrequency;

    private float _minHeight;
    private float _maxHeight;

    private int _checkpointCount = 1;

    private bool _cubeEvent = false;
    private bool _triggerPortal = false;

    private Checkpoint _lastCheckpoint;

    private void Awake()
    {
        _minHeight = _waterLevel.transform.position.y;
        _maxHeight = _minHeight + _heightRadius;

        Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    public void GenerateCheckpoint(Vector3 direction, Vector3 planePos, Checkpoint lastCheckpoint = null)
    {
        if (_cubeEvent)
        {
            _cubeEvent = false;
            _targetGenerator.GetComponent<TargetGenerator>().enabled = true;
            StartCoroutine(DisableScriptWait());
            return;
        }

        if (lastCheckpoint == null)
        {
            lastCheckpoint = _lastCheckpoint;
        }

        Vector3 lastCheckpointPosition = planePos;
        Checkpoint checkpoint;
        bool portal;
        if (!_triggerPortal)
        {

            ++_checkpointCount;
            portal = _checkpointCount % (_portalFrequency + 1) == 0;
            _cubeEvent = _checkpointCount % (_cubeEventFrequency * (_portalFrequency + 1)) == 0;

            if (_cubeEvent)
            {
                _checkpointCount = 0;
                _triggerPortal = true;
            }
        }
        else
        {
            portal = true;
            _triggerPortal = false;
        }

        while (true)
        {
            float directionOffset = Random.Range(_minRadius, _maxRadius);
            float lateralOffset = Random.Range(-_lateralRadius, _lateralRadius);
            float height = portal ? _minHeight + _portalHeight : Random.Range(_minHeight, _maxHeight);
            Vector2 pOffset = Vector2.Perpendicular(new Vector2(direction.x, direction.z)) * lateralOffset;

            Vector3 offset = direction * directionOffset +
                             new Vector3(pOffset.x, 0, pOffset.y);

            Vector3 potentialPosition = lastCheckpointPosition + offset;

            Vector2 tilePosition = terrainController.TileFromPosition(potentialPosition);

            var terrainMesh = terrainController.terrainTiles[tilePosition].GetComponent<GenerateMesh>();

            float terrainHeight = terrainMesh.GetTerrainHeightAtPosition(potentialPosition);
            potentialPosition.y = Mathf.Max(height, terrainHeight);

            if (portal)
            {
                checkpoint = Instantiate(_portal, potentialPosition, Quaternion.identity);
            }
            else
            {
                checkpoint = Instantiate(_checkpoint, potentialPosition, Quaternion.identity);
            }
            if (PlacementIsValid(checkpoint))
            {
                break;
            }

            Destroy(checkpoint);
            checkpoint.gameObject.SetActive(false);
        }

        checkpoint.countdownTimer = lastCheckpoint.countdownTimer;
        checkpoint.generator = this;
        _lastCheckpoint = checkpoint;
    }

    private bool PlacementIsValid(Checkpoint checkpoint)
    {
        var Mesh = _checkpoint.GetComponent<MeshFilter>();
        var checkpointDiameter = Mesh.sharedMesh.bounds.size.y;
        return Physics.OverlapSphere(checkpoint.transform.position, 4 * checkpointDiameter, _terrainLayers).Length == 0;
    }

    IEnumerator DisableScriptWait()
    {
        yield return new WaitForSeconds(_cubeEventDurationSeconds);
        _targetGenerator.GetComponent<TargetGenerator>().enabled = false;
        Vector3 direction = Vector3.ProjectOnPlane(_plane.velocity, Vector3.down).normalized;
        GenerateCheckpoint(direction, _plane.transform.position, null);
    }
}
