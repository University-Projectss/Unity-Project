using UnityEngine;

public class CameraRestrict : MonoBehaviour
{
    //the previous value of the parent's Z euler Angle
    private float _previousZ;
    
    // Start is called before the first frame update
    void Start()
    {
        _previousZ = transform.parent.localEulerAngles.z;
    }

    void LateUpdate()
    {
        //We compute the rotation on the Z axis since the last frame
        var deltaZ = transform.parent.localEulerAngles.z - _previousZ;
        _previousZ = transform.parent.localEulerAngles.z;
        
        //Parent's position in world space
        var targetPosition = transform.parent.position;

        //We rotate the camera around the parent's forward axis with -deltaZ
        //This effectively counters the rotation normally applied by the object hierarchy
        //Since RotateAround works in world space, we convert the parent's forward to world space
        transform.RotateAround(targetPosition, transform.parent.TransformDirection(Vector3.forward), -deltaZ);
    }
}
