using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : Checkpoint
{
    [SerializeField]
    private GameObject _waypoint;

    private bool _entered;

    protected override void OnTriggerEnter(Collider other)
    {
        if (!_entered && other.gameObject.CompareTag(Constants.PlayerTag))
        {
            generator.terrainController.ChangeNoiseTexture(() => base.OnTriggerEnter(other));
            generator.switcher.SwitchDimension();
            _scoreCounter.score.portals += 1;

            _waypoint.SetActive(false);
            _entered = true;
        }
    }
}
