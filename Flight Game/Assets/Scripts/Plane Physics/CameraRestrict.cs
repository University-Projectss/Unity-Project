using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRestrict : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rotation = transform.rotation.eulerAngles;
        transform.InverseTransformPoint(transform.position);

        transform.Rotate(0, 0, -rotation.z);
    }
}
