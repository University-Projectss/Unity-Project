using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _plane;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private RectTransform _directionCrosshair;

    [SerializeField]
    private RectTransform _velocityCrosshair;

    void Update()
    {
        _directionCrosshair.localPosition = _camera.WorldToScreenPoint(_camera.transform.position + _plane.transform.forward)
                                            - new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2);

        _velocityCrosshair.localPosition = _camera.WorldToScreenPoint(_camera.transform.position + _plane.velocity.normalized)
                                            - new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2);
    }
}
