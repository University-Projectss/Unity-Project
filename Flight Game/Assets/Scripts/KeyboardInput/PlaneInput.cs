using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneInput : MonoBehaviour
{
    [SerializeField]
    private FlightPhysics _flightPhysics;

    private Vector3 _flightVec;

    private float _lift;

    private float _thrust;

    public void ProcessFlight(InputAction.CallbackContext callbackContext) =>
        _flightVec = callbackContext.ReadValue<Vector3>();
    public void ProcessTakeOff(InputAction.CallbackContext callbackContext) =>
        _lift = callbackContext.ReadValue<float>() * 5f * Mathf.Deg2Rad;
    public void ProcessThrust(InputAction.CallbackContext callbackContext) =>
        _thrust = callbackContext.ReadValue<float>() * .2f * Time.deltaTime;

    public void ProcessFire(InputAction.CallbackContext callbackContext)
    {
        var click = callbackContext.ReadValue<float>();
        //improvement here, look for an alternative to BroadcastMessage
        //a potential is a custom implmenetation using ExecuteEvents.Execute recursively on all children.
        if (click > 0)
        {
            gameObject.BroadcastMessage("StartShooting");
        }
        else
        {
            gameObject.BroadcastMessage("StopShooting");
        }
    }

    void Start() =>
        _flightPhysics = gameObject.GetComponent<FlightPhysics>();

    void Update()
    {
        _flightPhysics.AddThrottle(_thrust);

        _flightPhysics.SetFlapRad(_lift);

        _flightPhysics.ApplyPitchTorque(_flightVec.y);
        _flightPhysics.ApplyYawTorque(_flightVec.x);
        _flightPhysics.ApplyRollTorque(_flightVec.z);
    }
}
