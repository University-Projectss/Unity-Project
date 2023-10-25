using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class FlightPhysics : MonoBehaviour
{
    [Tooltip("The maximum thrust of the engine")]
    public float MaxThrust;
    [Range(0,1)]
    [Tooltip("The fraction of the Maximum Thrust currently applied")]
    public float Throttle;

    public float LiftPower;

    public float InducedDrag;

    public AnimationCurve LiftCoefficientCurve;

    //Both velocity and angular velocity are in Local/Model Space
    //So if our object is normally _, but rotated |, UP is always (0,1,0) in Local Space, but (-1,0,0) in World Space
    private Vector3 _velocity;
    private Vector3 _angularVelocity;

    private float _angleOfAttack;
    private float _flap;

    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _velocity = Vector3.zero;
        _angularVelocity = Vector3.zero;
    }

    // FixedUpdate is called at a fixed rate
    void FixedUpdate()
    {
        DetermineState();
        ApplyForces(Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _flap = 5f;
        }
        else
        {
            _flap = 0f;
        }
    }

    private void DetermineState()
    {
        _velocity = transform.InverseTransformDirection(_rigidBody.velocity);
        _angularVelocity = transform.InverseTransformDirection(_rigidBody.angularVelocity);

        DetermineAngleOfAttack();
    }

    private void DetermineAngleOfAttack()
    {
        _angleOfAttack = Mathf.Atan2(-_velocity.y, _velocity.z);
    }

    private void ApplyForces(float deltaTime)
    {
        ApplyThrust();
        ApplyLift();
        ApplyDrag();
    }

    private void ApplyThrust()
    {
        _rigidBody.AddRelativeForce(Throttle * MaxThrust * Vector3.forward);
    }

    private void ApplyLift()
    {
        float liftCoefficient = LiftCoefficientCurve.Evaluate(_angleOfAttack * Mathf.Rad2Deg + _flap);

        Vector3 liftVelocity = Vector3.ProjectOnPlane(_velocity, Vector3.right);
        float liftMagnitude = liftCoefficient * liftVelocity.sqrMagnitude * LiftPower;

        float dragMagnitude = liftCoefficient * liftCoefficient * InducedDrag;

        Vector3 lift = liftMagnitude * Vector3.up;
        Vector3 inducedDrag = dragMagnitude * (-liftVelocity.normalized);

        _rigidBody.AddRelativeForce(lift + inducedDrag);
    }

    private void ApplyDrag()
    {
        var drag = 1f * _velocity.sqrMagnitude * -_velocity.normalized;
        _rigidBody.AddRelativeForce(drag);
    }
}
