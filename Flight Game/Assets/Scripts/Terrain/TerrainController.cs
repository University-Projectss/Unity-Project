﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainController : MonoBehaviour {

    [Tooltip("Prefab for the terrain tile.")]
    [SerializeField]
    private GameObject _terrainTilePrefab = null;
    [SerializeField]
    private Vector3 _terrainSize = new Vector3(20, 1, 20);
    public Vector3 TerrainSize { get { return _terrainSize; } }
    [SerializeField]
    private Gradient _gradient;
    [SerializeField]
    private float _noiseScale = 2, _cellSize = 1;

    [Tooltip("Radius around the player within which terrain tiles are rendered.")]
    [SerializeField]
    private int _radiusToRender = 5;

    [Tooltip("Transforms of the game objects to be tracked for terrain loading.")]
    [SerializeField]
    private Transform[] _gameTransforms;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private Transform _water;
    public Transform Water { get { return _water; } }
    [SerializeField]
    private int _seed;
    [SerializeField]
    private float _destroyDistance = 1000;
    [SerializeField]
    private bool _usePerlinNoise = true;
    [SerializeField]
    private Texture2D _noise;
    public static float[][] noisePixels;

    private Vector2 _startOffset;

    private Dictionary<Vector2, GameObject> _terrainTiles = new Dictionary<Vector2, GameObject>();

    private Vector2[] _previousCenterTiles;
    private List<GameObject> _previousTileObjects = new List<GameObject>();
    public Transform Level { get; set; }
    private Vector2 _noiseRange;

    private void Awake() {
        if (_noise)
            noisePixels = GetGrayScalePixels(_noise);
        GenerateMesh.UsePerlinNoise = _usePerlinNoise;
        _noiseRange = _usePerlinNoise ? Vector2.one * 256 : new Vector2(noisePixels.Length, noisePixels[0].Length);
    }

    private void Start() {
        InitialLoad();
    }

    public void InitialLoad() {
        DestroyTerrain();

        Level = new GameObject("Level").transform;
        _water.parent = Level;
        _playerTransform.parent = Level;
        foreach (Transform t in _gameTransforms)
            t.parent = Level;

        float waterSideLength = _radiusToRender * 2 + 1;
        _water.localScale = new Vector3(_terrainSize.x / 10 * waterSideLength, 1, _terrainSize.z / 10 * waterSideLength);

        Random.InitState(_seed);
        //choose a random place on perlin noise
        _startOffset = new Vector2(Random.Range(0f, _noiseRange.x), Random.Range(0f, _noiseRange.y));
        RandomizeInitState();
    }

    private void Update() {
        //save the tile the player is on
        Vector2 playerTile = TileFromPosition(_playerTransform.localPosition);
        //save the tiles of all tracked objects in gameTransforms (including the player)
        List<Vector2> centerTiles = new List<Vector2>();
        centerTiles.Add(playerTile);
        foreach (Transform t in _gameTransforms)
            centerTiles.Add(TileFromPosition(t.localPosition));

        //if no tiles exist yet or tiles should change
        if (_previousCenterTiles == null || HaveTilesChanged(centerTiles)) {
            List<GameObject> tileObjects = new List<GameObject>();
            //activate new tiles
            foreach (Vector2 tile in centerTiles) {
                bool isPlayerTile = tile == playerTile;
                int radius = isPlayerTile ? _radiusToRender : 1;
                for (int i = -radius; i <= radius; i++)
                    for (int j = -radius; j <= radius; j++)
                        ActivateOrCreateTile((int)tile.x + i, (int)tile.y + j, tileObjects);
                if (isPlayerTile)
                    _water.localPosition = new Vector3(tile.x * _terrainSize.x, _water.localPosition.y, tile.y * _terrainSize.z);
            }
            //deactivate old tiles
            foreach (GameObject g in _previousTileObjects)
                if (!tileObjects.Contains(g))
                    g.SetActive(false);

            //destroy inactive tiles if they're too far away
            List<Vector2> keysToRemove = new List<Vector2>();//can't remove item when inside a foreach loop
            foreach (KeyValuePair<Vector2, GameObject> kv in _terrainTiles) {
                if (Vector3.Distance(_playerTransform.position, kv.Value.transform.position) > _destroyDistance && !kv.Value.activeSelf) {
                    keysToRemove.Add(kv.Key);
                    Destroy(kv.Value);
                }
            }
            foreach (Vector2 key in keysToRemove)
                _terrainTiles.Remove(key);

            _previousTileObjects = new List<GameObject>(tileObjects);
        }

        _previousCenterTiles = centerTiles.ToArray();
    }

    //Helper methods below

    private void ActivateOrCreateTile(int xIndex, int yIndex, List<GameObject> tileObjects) {
        if (!_terrainTiles.ContainsKey(new Vector2(xIndex, yIndex))) {
            tileObjects.Add(CreateTile(xIndex, yIndex));
        } else {
            GameObject t = _terrainTiles[new Vector2(xIndex, yIndex)];
            tileObjects.Add(t);
            if (!t.activeSelf)
                t.SetActive(true);
        }
    }

    private GameObject CreateTile(int xIndex, int yIndex) {
        GameObject terrain = Instantiate(
            _terrainTilePrefab,
            Vector3.zero,
            Quaternion.identity,
            Level
        );
        //had to move outside of instantiate because it's a local position
        terrain.transform.localPosition = new Vector3(_terrainSize.x * xIndex, _terrainSize.y, _terrainSize.z * yIndex);
        terrain.name = TrimEnd(terrain.name, "(Clone)") + " [" + xIndex + " , " + yIndex + "]";

        _terrainTiles.Add(new Vector2(xIndex, yIndex), terrain);

        GenerateMesh gm = terrain.GetComponent<GenerateMesh>();
        gm.TerrainSize = _terrainSize;
        gm.Gradient = _gradient;
        gm.NoiseScale = _noiseScale;
        gm.CellSize = _cellSize;
        gm.NoiseOffset = NoiseOffset(xIndex, yIndex);
        gm.Generate();

        Random.InitState((int)(_seed + (long)xIndex * 100 + yIndex));//so it doesn't form a (noticable) pattern of similar tiles
        RandomizeInitState();

        return terrain;
    }

    private Vector2 NoiseOffset(int xIndex, int yIndex) {
        Vector2 noiseOffset = new Vector2(
            (xIndex * _noiseScale + _startOffset.x) % _noiseRange.x,
            (yIndex * _noiseScale + _startOffset.y) % _noiseRange.y
        );
        //account for negatives (ex. -1 % 256 = -1, needs to loop around to 255)
        if (noiseOffset.x < 0)
            noiseOffset = new Vector2(noiseOffset.x + _noiseRange.x, noiseOffset.y);
        if (noiseOffset.y < 0)
            noiseOffset = new Vector2(noiseOffset.x, noiseOffset.y + _noiseRange.y);
        return noiseOffset;
    }

    private Vector2 TileFromPosition(Vector3 position) {
        return new Vector2(Mathf.FloorToInt(position.x / _terrainSize.x + .5f), Mathf.FloorToInt(position.z / _terrainSize.z + .5f));
    }

    private void RandomizeInitState() {
        Random.InitState((int)System.DateTime.UtcNow.Ticks);//casting a long to an int "loops" it (like modulo)
    }

    private bool HaveTilesChanged(List<Vector2> centerTiles) {
        if (_previousCenterTiles.Length != centerTiles.Count)
            return true;
        for (int i = 0; i < _previousCenterTiles.Length; i++)
            if (_previousCenterTiles[i] != centerTiles[i])
                return true;
        return false;
    }

    public void DestroyTerrain() {
        _water.parent = null;
        _playerTransform.parent = null;
        foreach (Transform t in _gameTransforms)
            t.parent = Level;
        Destroy(Level);
        _terrainTiles.Clear();
    }

    private static string TrimEnd(string str, string end) {
        if (str.EndsWith(end))
            return str.Substring(0, str.LastIndexOf(end));
        return str;
    }

    public static float[][] GetGrayScalePixels(Texture2D texture2D) {
        List<float> grayscale = texture2D.GetPixels().Select(c => c.grayscale).ToList();

        List<List<float>> grayscale2d = new List<List<float>>();
        for (int i = 0; i < grayscale.Count; i += texture2D.width)
            grayscale2d.Add(grayscale.GetRange(i, texture2D.width));

        // The height of the image is determined by the number of rows (lists) in grayscale2d
        // The width of the image is determined by the number of elements in each row (floats in each list)
        // Image dimensions: Height = grayscale2d.Count, Width = grayscale2d[0].Count

        return grayscale2d.Select(a => a.ToArray()).ToArray();
    }

}