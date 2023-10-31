using UnityEngine;

public class PlaneInput : MonoBehaviour
{
    [SerializeField]
    private FlightPhysics _flightPhysics;

    void Start()
    {
        _flightPhysics = gameObject.GetComponent<FlightPhysics>();
    }

    void Update()
    {
        if (Input.GetButton("Accelerate"))
        {
            _flightPhysics.AddThrottle(Input.GetAxis("Accelerate") * .2f * Time.deltaTime);
        }
        if (Input.GetButton("Take Off"))
        {
            _flightPhysics.SetFlapRad(5f * Mathf.Deg2Rad);
        }
        else
        {
            _flightPhysics.SetFlapRad();
        }

        if (Input.GetButton("Vertical"))
        {
            _flightPhysics.ApplyPitchTorque(Input.GetAxis("Vertical"));
        }
        if (Input.GetButton("Horizontal"))
        {
            _flightPhysics.ApplyYawTorque(Input.GetAxis("Horizontal"));
        }
        if (Input.GetButton("Roll"))
        {
            _flightPhysics.ApplyRollTorque(Input.GetAxis("Roll"));
        }
    }
}
