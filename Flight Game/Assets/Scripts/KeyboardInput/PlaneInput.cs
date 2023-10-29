using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneInput : MonoBehaviour
{
    private FlightPhysics _flightPhysics;

    void Start()
    {
        _flightPhysics = gameObject.GetComponent<FlightPhysics>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _flightPhysics.AddThrottle(.2f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _flightPhysics.AddThrottle(-.2f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.F))
        {
            _flightPhysics.SetFlapRad(5f * Mathf.Deg2Rad);
        }
        else
        {
            _flightPhysics.SetFlapRad();
        }

        if (Input.GetKey(KeyCode.S))
        {
            _flightPhysics.ApplyPitchTorque(-1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _flightPhysics.ApplyPitchTorque(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _flightPhysics.ApplyYawTorque(-1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _flightPhysics.ApplyYawTorque(1);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _flightPhysics.ApplyRollTorque(1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            _flightPhysics.ApplyRollTorque(-1);
        }
    }
}
