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
            generator.terrainController.ChangeNoiseTexture();
            generator.switcher.SwitchDimension();

            _waypoint.SetActive(false);
            _entered = true;
            StartCoroutine(BaseCoroutine(other));
        }
    }

    //If we try and spawn the new checkpoint right after we switch dimensions
    //it will break because it can't find the previous tiles
    //Giving it a slight delay is a simple, albeit hack-ish fix.
    private IEnumerator BaseCoroutine(Collider other)
    {
        yield return new WaitForSeconds(0.75f);
        base.OnTriggerEnter(other);
    } 
}
