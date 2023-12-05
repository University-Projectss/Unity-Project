using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{

    public CountdownTimer countdownTimer;
    public CheckpointGenerator generator;
    public float timerGain;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag)){
            
            countdownTimer.AddTime(timerGain);
            Vector3 direction = Vector3.ProjectOnPlane(other.attachedRigidbody.velocity, Vector3.down).normalized;
            generator.GenerateCheckpoint(this, direction);
            Destroy(gameObject);
            
            //Destroy only happens after the current Update Loop
            //So we disable the object to prevent multiple triggers
            gameObject.SetActive(false);
        }
    }
}
