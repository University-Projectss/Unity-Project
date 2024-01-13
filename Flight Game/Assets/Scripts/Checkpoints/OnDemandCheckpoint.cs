using UnityEngine;

public class OnDemandCheckpoint : MonoBehaviour
{
    public CheckpointGenerator generator;
    public Rigidbody plane;
    private Checkpoint lastCheckpoint;

    private void Update()
    {
        // Check if the C key is released (key up event)
        if (Input.GetKeyUp(KeyCode.C))
        {
            lastCheckpoint = generator.lastCheckpoint;

            Vector3 direction = Vector3.ProjectOnPlane(plane.velocity, Vector3.down).normalized;
            generator.GenerateCheckpoint(direction, plane.transform.position, lastCheckpoint);

            if (lastCheckpoint.gameObject != null)
            {
                Destroy(lastCheckpoint.gameObject);
                lastCheckpoint.gameObject.SetActive(false);
            }
        }
    }
}