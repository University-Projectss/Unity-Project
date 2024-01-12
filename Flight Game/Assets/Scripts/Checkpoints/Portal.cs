using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : Checkpoint
{
    private bool _entered;

    protected override void Awake() { }

    protected override void Start() { }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (!_entered && other.gameObject.CompareTag(Constants.PlayerTag))
        {
            StartCoroutine(TerrainCoroutine(other));
            generator.switcher.SwitchDimension();
            _scoreCounter.score.portals += 1;

            _waypoint.SetActive(false);
            _entered = true;
        }
    }

    private IEnumerator TerrainCoroutine(Collider other)
    {
        yield return new WaitForSeconds(0.5f);
        generator.terrainController.ChangeNoiseTexture(() => StartCoroutine(GenerationCoroutine(other)));
    }

    private IEnumerator GenerationCoroutine(Collider other)
    {
        yield return new WaitForSeconds(0.5f);
        base.OnTriggerEnter(other);
    }
}
