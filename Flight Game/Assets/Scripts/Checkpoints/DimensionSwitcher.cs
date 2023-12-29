using System;
using System.Linq;
using UnityEngine;

public class DimensionSwitcher : MonoBehaviour
{
    [SerializeField]
    private Portal _portal;

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

    private float _portalDiameter;

    private void Awake()
    {
        terrainMaterial = Resources.Load("Materials/Snow", typeof(Material)) as Material;

        var Mesh = _portal.GetComponent<MeshFilter>();
        _portalDiameter = Mesh.sharedMesh.bounds.size.y;

        _minHeight = _waterLevel.transform.position.y;
        _maxHeight = _minHeight + _heightRadius;

        UnityEngine.Random.InitState((int)System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    public void GeneratePortal(Checkpoint lastCheckpoint, Vector3 direction)
    {
        Vector3 lastCheckpointPosition = lastCheckpoint.transform.position;

        Portal portal;
        while (true)
        {
            float directionOffset = UnityEngine.Random.Range(_minRadius, _maxRadius);
            float lateralOffset = UnityEngine.Random.Range(-_lateralRadius, _lateralRadius);
            float height = UnityEngine.Random.Range(_minHeight, _maxHeight);

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

        portal.switcher = this;
    }

    private bool PlacementIsValid(Portal portal)
    {
        return Physics.OverlapSphere(portal.transform.position, 4 * _portalDiameter, _terrainLayers).Length == 0;
    }

    public enum DimensionOption
    {
        IceMountains,
        Toxic,
        StarryNight
    }

    public DimensionOption currentDimension = DimensionOption.IceMountains;

    Material terrainMaterial;


    public void SwitchDimension()
    {
        // Initialize dimension properties with default values
        Color fogColor = Color.black, terrainColor = Color.black;
        Material skyboxMaterial = null;
        float fogDensity = 0, ambientIntensity = 0;

        currentDimension = ChooseRandomDimension();

        switch (currentDimension)
        {
            case DimensionOption.IceMountains:
                ColorUtility.TryParseHtmlString("#5AB1FF", out fogColor);
                terrainColor = Color.white;
                skyboxMaterial = Resources.Load("Materials/Skybox Cubemap Extended Day", typeof(Material)) as Material;
                fogDensity = 0.018f;
                ambientIntensity = 1;
                break;

            case DimensionOption.Toxic:
                ColorUtility.TryParseHtmlString("#FAFAC5", out fogColor);
                ColorUtility.TryParseHtmlString("#670516", out terrainColor);
                skyboxMaterial = Resources.Load("Materials/meow", typeof(Material)) as Material;
                fogDensity = 0.016f;
                ambientIntensity = 1.85f;
                break;

            case DimensionOption.StarryNight:
                ColorUtility.TryParseHtmlString("#58486A", out fogColor);
                terrainColor = Color.white;
                skyboxMaterial = Resources.Load("Materials/Skybox Cubemap Extended Night", typeof(Material)) as Material;
                fogDensity = 0.02f;
                ambientIntensity = 1;
                break;
        }

        ChangeSceneDetails(
            fogColor: fogColor,
            terrainColor: terrainColor,
            skyboxMaterial: skyboxMaterial,
            fogDensity: fogDensity,
            ambientIntensity: ambientIntensity
        );
    }

    private DimensionOption ChooseRandomDimension()
    {
        // Get all values from the DimensionOption enum and remove the current dimension
        var allDimensions = ((DimensionOption[])Enum
            .GetValues(typeof(DimensionOption)))
            .Where(dimension => !dimension.Equals(currentDimension))
            .ToList();

        // Choose a random dimension option
        return allDimensions[UnityEngine.Random.Range(0, allDimensions.Count)];
    }

    private void ChangeSceneDetails(
        Color fogColor,
        Color terrainColor,
        Material skyboxMaterial,
        float fogDensity,
        float ambientIntensity)
    {
        terrainMaterial.color = terrainColor;
        RenderSettings.fogColor = fogColor;
        RenderSettings.skybox = skyboxMaterial;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.ambientIntensity = ambientIntensity;
    }
}
