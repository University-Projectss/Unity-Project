using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    public DimensionSwitcher switcher;
    public PortalGenerator generator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag))
        {
            generator._terrainController.ChangeNoiseTexture();
            switcher.SwitchDimension();
            
            gameObject.SetActive(false);
        }
    }
}
