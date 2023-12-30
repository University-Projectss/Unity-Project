using UnityEngine;

public class PortalGenerator : MonoBehaviour
{
    [SerializeField]
    private Portal _portal;

    [Tooltip("The Terrain Controller object")]
    [SerializeField]
    public TerrainController _terrainController;

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

    private float _portalDiameter;

    [SerializeField]
    public DimensionSwitcher switcher;

    private void Awake()
    {
        var Mesh = _portal.GetComponent<MeshFilter>();
        _portalDiameter = Mesh.sharedMesh.bounds.size.y;

        _minHeight = _waterLevel.transform.position.y;
        _maxHeight = _minHeight + _heightRadius;

        Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    public void GeneratePortal(Checkpoint lastCheckpoint, Vector3 direction)
    {
        Vector3 lastCheckpointPosition = lastCheckpoint.transform.position;

        Portal portal;
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

            portal = Instantiate(_portal, potentialPosition, Quaternion.identity);

            if (PlacementIsValid(portal))
            {
                break;
            }

            Destroy(portal);
            portal.gameObject.SetActive(false);
        }

        portal.gameObject.SetActive(true);
        portal.switcher = this.switcher;
    }

    private bool PlacementIsValid(Portal portal)
    {
        return Physics.OverlapSphere(portal.transform.position, 4 * _portalDiameter, _terrainLayers).Length == 0;
    }
}
