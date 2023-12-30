using UnityEngine;

public class CheckpointGenerator : MonoBehaviour
{
    [Tooltip("Checkpoint Prefab to instantiate")]
    [SerializeField]
    private Checkpoint _checkpoint;

    [Tooltip("The Terrain Controller object")]
    [SerializeField]
    private TerrainController _terrainController;

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

    [Tooltip("Which layers constitute the terrain")]
    [SerializeField]
    private LayerMask _terrainLayers;

    private float _minHeight;
    private float _maxHeight;

    private float _checkpointDiameter;

    private int _checkpointCount = 0;

    [SerializeField]
    private PortalGenerator _portalGenerator;

    private void Awake()
    {
        var Mesh = _checkpoint.GetComponent<MeshFilter>();
        _checkpointDiameter = Mesh.sharedMesh.bounds.size.y;

        _minHeight = _waterLevel.transform.position.y;
        _maxHeight = _minHeight + _heightRadius;

        Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    public void GenerateCheckpoint(Checkpoint lastCheckpoint, Vector3 direction)
    {
        if (++_checkpointCount == 2)
        {
            _checkpointCount = 0;
            _portalGenerator.GeneratePortal(lastCheckpoint, direction);
        }

        Vector3 lastCheckpointPosition = lastCheckpoint.transform.position;

        Checkpoint checkpoint;
        while (true)
        {
            float directionOffset = Random.Range(_minRadius, _maxRadius);
            float lateralOffset = Random.Range(-_lateralRadius, _lateralRadius);
            float height = Random.Range(_minHeight, _maxHeight);

            Vector2 pOffset = Vector2.Perpendicular(new Vector2(direction.x, direction.z)) * lateralOffset;

            Vector3 offset = direction * directionOffset +
                             new Vector3(pOffset.x, 0, pOffset.y);

            Vector3 potentialPosition = lastCheckpointPosition + offset;

            Vector2 tilePosition = _terrainController.TileFromPosition(potentialPosition);

            var terrainMesh = _terrainController.terrainTiles[tilePosition].GetComponent<GenerateMesh>();

            float terrainHeight = terrainMesh.GetTerrainHeightAtPosition(potentialPosition);
            potentialPosition.y = Mathf.Max(height, terrainHeight);

            checkpoint = Instantiate(_checkpoint, potentialPosition, Quaternion.identity);

            if (PlacementIsValid(checkpoint))
            {
                break;
            }

            Destroy(checkpoint);
            checkpoint.gameObject.SetActive(false);
        }

        checkpoint.countdownTimer = lastCheckpoint.countdownTimer;
        checkpoint.generator = this;
        checkpoint.generator._portalGenerator = this._portalGenerator;
    }

    private bool PlacementIsValid(Checkpoint checkpoint)
    {
        return Physics.OverlapSphere(checkpoint.transform.position, 4 * _checkpointDiameter, _terrainLayers).Length == 0;
    }
}
