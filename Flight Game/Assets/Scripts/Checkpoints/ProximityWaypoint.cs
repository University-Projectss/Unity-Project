using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ProximityWaypoint : MonoBehaviour
{
    [SerializeField]
    private Transform _plane;

    [SerializeField]
    private float _activationDistance;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _plane = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                          new Vector3(_plane.position.x, 0, _plane.position.z));
        if (distance >= _activationDistance)
        {
            _meshRenderer.enabled = true;
        }
        else
        {
            _meshRenderer.enabled = false;   
        }
    }
}
