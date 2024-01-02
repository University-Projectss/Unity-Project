using System;
using TMPro;
using UnityEngine;

public class SpeedIndicator : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _plane;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private TextMeshProUGUI _speedText;

    [SerializeField]
    private Color _colorFast;

    [SerializeField]
    private Color _colorMedium;

    [SerializeField]
    private Color _colorSlow;

    [SerializeField]
    private float _Xoffset;

    void Update()
    {
        _speedText.transform.localPosition = _camera.WorldToScreenPoint(_camera.transform.position + _plane.transform.forward) - new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2) + Vector3.left * _Xoffset;

        float speed = _plane.velocity.magnitude;
        if (speed > 37)
        {
            _speedText.color = _colorFast;
        }
        else if (speed < 24)
        {
            _speedText.color = _colorSlow;
        }
        else
        {
            _speedText.color = _colorMedium;
        }
        _speedText.text = Math.Floor(_plane.velocity.magnitude).ToString();
    }
}
