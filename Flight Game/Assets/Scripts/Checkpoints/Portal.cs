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
            switcher.SwitchDimension();
            
            gameObject.SetActive(false);
        }
    }
}
