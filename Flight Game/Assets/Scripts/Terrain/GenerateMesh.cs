using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(MeshFilter))]
public class GenerateMesh : MonoBehaviour
{
    private MeshFilter _meshFilter;

    [Header("Terrain Settings")]
    [Tooltip("The size of the terrain in the X, Y, and Z dimensions.")]
    public Vector3 TerrainSize;

    [Tooltip("The size of each terrain cell.")]
    public float CellSize;

    [Tooltip("The scale of the noise applied to the terrain.")]
    public float NoiseScale;

    [Tooltip("The gradient used to color the terrain based on height.")]
    public Gradient Gradient;

    [Tooltip("The offset used to generate different noise patterns.")]
    public Vector2 NoiseOffset;

    private static bool _usePerlinNoise = true;

    [Tooltip("Toggle to use Perlin noise. If false, use pre-generated noise from TerrainController.")]
    public static bool UsePerlinNoise { get { return _usePerlinNoise; } set { _usePerlinNoise = value; } }

    [Tooltip("Parameter that decides how much the height scales with the noise.")]
    [SerializeField]
    public static int HeightScale = 90;

    [ContextMenu("Generate Terrain")]
    public void Generate()
    {
        _meshFilter = GetComponent<MeshFilter>();
        
        MeshDraft draft = TerrainDraft(TerrainSize, CellSize, NoiseOffset, NoiseScale, Gradient);
        draft.Move(Vector3.left * TerrainSize.x / 2 + Vector3.back * TerrainSize.z / 2);
        _meshFilter.mesh = draft.ToMesh();

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider)
            meshCollider.sharedMesh = _meshFilter.mesh;
    }

    private static MeshDraft TerrainDraft(Vector3 terrainSize, float cellSize, Vector2 noiseOffset, float noiseScale, Gradient gradient)
    {
        int xSegments = Mathf.FloorToInt(terrainSize.x / cellSize);
        int zSegments = Mathf.FloorToInt(terrainSize.z / cellSize);

        float xStep = cellSize;
        float zStep = cellSize;
        int vertexCount = 6 * xSegments * zSegments;
        MeshDraft draft = new MeshDraft
        {
            name = "Terrain",
            vertices = new List<Vector3>(vertexCount),
            triangles = new List<int>(vertexCount),
            normals = new List<Vector3>(vertexCount),
            colors = new List<Color>(vertexCount)
        };

        // Initialize vertex, triangle, normal, and color lists
        for (int i = 0; i < vertexCount; i++)
        {
            draft.vertices.Add(Vector3.zero);
            draft.triangles.Add(0);
            draft.normals.Add(Vector3.zero);
            draft.colors.Add(Color.black);
        }

        // Loop through each terrain cell
        for (int x = 0; x < xSegments; x++)
        {
            for (int z = 0; z < zSegments; z++)
            {
                // Calculate the indices for the six vertices of the terrain cell
                int index0 = 6 * (x + z * xSegments);
                int index1 = index0 + 1;
                int index2 = index0 + 2;
                int index3 = index0 + 3;
                int index4 = index0 + 4;
                int index5 = index0 + 5;

                // Calculate the heights of the four corners of the terrain cell
                float height00 = GetHeight(x + 0, z + 0, xSegments, zSegments, noiseOffset, noiseScale);
                float height01 = GetHeight(x + 0, z + 1, xSegments, zSegments, noiseOffset, noiseScale);
                float height10 = GetHeight(x + 1, z + 0, xSegments, zSegments, noiseOffset, noiseScale);
                float height11 = GetHeight(x + 1, z + 1, xSegments, zSegments, noiseOffset, noiseScale);

                // Calculate the local-space positions of the four corners
                Vector3 vertex00 = new Vector3((x + 0) * xStep, height00 * terrainSize.y * HeightScale, (z + 0) * zStep);
                Vector3 vertex01 = new Vector3((x + 0) * xStep, height01 * terrainSize.y * HeightScale, (z + 1) * zStep);
                Vector3 vertex10 = new Vector3((x + 1) * xStep, height10 * terrainSize.y * HeightScale, (z + 0) * zStep);
                Vector3 vertex11 = new Vector3((x + 1) * xStep, height11 * terrainSize.y * HeightScale, (z + 1) * zStep);
                
                // Assign vertices and colors to the draft

                // - vertex00 is the bottom-left corner of the terrain cell.
                // - vertex01 is the top-left corner of the terrain cell.
                // - vertex11 is the top-right corner of the terrain cell.
                // - vertex10 is the bottom-right corner of the terrain cell.

                draft.vertices[index0] = vertex00;
                draft.vertices[index1] = vertex01;
                draft.vertices[index2] = vertex11;
                draft.vertices[index3] = vertex00;
                draft.vertices[index4] = vertex11;
                draft.vertices[index5] = vertex10;

                draft.colors[index0] = gradient.Evaluate(height00);
                draft.colors[index1] = gradient.Evaluate(height01);
                draft.colors[index2] = gradient.Evaluate(height11);
                draft.colors[index3] = gradient.Evaluate(height00);
                draft.colors[index4] = gradient.Evaluate(height11);
                draft.colors[index5] = gradient.Evaluate(height10);

                // Calculate normals for the two triangles of the terrain cell

                // - normal000111 represents the upward direction perpendicular to the surface of the first triangle.
                // - normal001011 represents the upward direction perpendicular to the surface of the second triangle.

                Vector3 normal000111 = Vector3.Cross(vertex01 - vertex00, vertex11 - vertex00).normalized;
                Vector3 normal001011 = Vector3.Cross(vertex11 - vertex00, vertex10 - vertex00).normalized;

                // Assign normals to the draft
                draft.normals[index0] = normal000111;
                draft.normals[index1] = normal000111;
                draft.normals[index2] = normal000111;
                draft.normals[index3] = normal001011;
                draft.normals[index4] = normal001011;
                draft.normals[index5] = normal001011;

                // Assign triangles to the draft
                draft.triangles[index0] = index0;
                draft.triangles[index1] = index1;
                draft.triangles[index2] = index2;
                draft.triangles[index3] = index3;
                draft.triangles[index4] = index4;
                draft.triangles[index5] = index5;
            }
        }

        return draft;
    }

    private static float GetHeight(int x, int z, int xSegments, int zSegments, Vector2 noiseOffset, float noiseScale)
    {
        float noiseX = noiseScale * x / xSegments + noiseOffset.x;
        float noiseZ = noiseScale * z / zSegments + noiseOffset.y;

        if (_usePerlinNoise)
            return Mathf.PerlinNoise(noiseX, noiseZ);
        else
            return TerrainController.noisePixels[(int)noiseX % TerrainController.noisePixels.Length][(int)noiseZ % TerrainController.noisePixels[0].Length];
    }

    public float GetTerrainHeightAtPosition(Vector3 worldPosition)
    {

        // Maximum segments per tile
        int xSegments = Mathf.FloorToInt(TerrainSize.x / CellSize);
        int zSegments = Mathf.FloorToInt(TerrainSize.z / CellSize);

        // Convert the world position to local position within the terrain
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);

        // Calculate the indices of the terrain cell containing the given position
        int xIndex = Mathf.FloorToInt(localPosition.x / CellSize) + xSegments / 2;
        int zIndex = Mathf.FloorToInt(localPosition.z / CellSize) + zSegments / 2;

        // Calculate the heights of the four corners of the terrain cell
        float height00 = GetHeight(xIndex + 0, zIndex + 0, Mathf.FloorToInt(TerrainSize.x / CellSize), Mathf.FloorToInt(TerrainSize.z / CellSize), NoiseOffset, NoiseScale);
        float height01 = GetHeight(xIndex + 0, zIndex + 1, Mathf.FloorToInt(TerrainSize.x / CellSize), Mathf.FloorToInt(TerrainSize.z / CellSize), NoiseOffset, NoiseScale);
        float height10 = GetHeight(xIndex + 1, zIndex + 0, Mathf.FloorToInt(TerrainSize.x / CellSize), Mathf.FloorToInt(TerrainSize.z / CellSize), NoiseOffset, NoiseScale);
        float height11 = GetHeight(xIndex + 1, zIndex + 1, Mathf.FloorToInt(TerrainSize.x / CellSize), Mathf.FloorToInt(TerrainSize.z / CellSize), NoiseOffset, NoiseScale);

        // Interpolate the height using bilinear interpolation
        float u = Mathf.InverseLerp(xIndex * CellSize, (xIndex + 1) * CellSize, localPosition.x);
        float v = Mathf.InverseLerp(zIndex * CellSize, (zIndex + 1) * CellSize, localPosition.z);

        float height = Mathf.Lerp(Mathf.Lerp(height00, height10, u), Mathf.Lerp(height01, height11, u), v);

        // Scale the height with TerrainSize.y and HeightScale
        height *= TerrainSize.y * HeightScale;

        return height;
    }
}
