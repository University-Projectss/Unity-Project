using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    public DimensionSwitcher switcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag))
        {
            Vector3 direction = Vector3.ProjectOnPlane(other.attachedRigidbody.velocity, Vector3.down).normalized;
            switcher.SwitchDimension();
            //Destroy(gameObject);
            
            gameObject.SetActive(false);
        }
    }
}
