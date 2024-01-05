using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public CountdownTimer countdownTimer;
    public CheckpointGenerator generator;
    public GameObject plane;
    public float timerGain;
    public int scoreGain;

    [SerializeField]
    protected ScoreCounterSO _scoreCounter;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.PlayerTag)){

            countdownTimer.AddTime(timerGain);
            _scoreCounter.score.checkpoints += 1;
            _scoreCounter.score.total += scoreGain;

            Vector3 direction = Vector3.ProjectOnPlane(other.attachedRigidbody.velocity, Vector3.down).normalized;
            generator.GenerateCheckpoint(direction, plane.transform.position, this);
            Destroy(gameObject);

            //Destroy only happens after the current Update Loop
            //So we disable the object to prevent multiple triggers
            gameObject.SetActive(false);
        }
    }
}
