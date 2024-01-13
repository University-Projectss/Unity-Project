using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField]
    private Vector2 _min;
    [SerializeField]
    private Vector2 _max;
    [SerializeField]
    private float _height;

    [SerializeField]
    private CheckpointGenerator _generator;

    [SerializeField] 
    private TerrainController _terrainController;

    [SerializeField]
    private Checkpoint _seedCheckpoint;

    void Start()
    {
        var position = new Vector3(Random.Range(_min.x, _max.x), _height, Random.Range(_min.y, _max.y));
        transform.position = position;
        GetComponent<Rigidbody>().position = position;

        _terrainController.Generate();
        _generator.GenerateCheckpoint(transform.forward, transform.position, _seedCheckpoint);
        Destroy(_seedCheckpoint.gameObject);
    }
}