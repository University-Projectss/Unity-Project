using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : Checkpoint
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag))
        {
            generator.terrainController.ChangeNoiseTexture();
            generator.switcher.SwitchDimension();

            StartCoroutine(BaseCoroutine(other));
        }
    }

    private IEnumerator BaseCoroutine(Collider other)
    {
        yield return new WaitForSeconds(0.5f);
        base.OnTriggerEnter(other);
    } 
}
