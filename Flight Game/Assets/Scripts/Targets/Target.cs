using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public CountdownTimer countdownTimer;
    
    [SerializeField]
    private Material _material;
    [SerializeField]
    private float _timerGain;
    [SerializeField]
    private float _hits;

    private Material _instanceMaterial;
    private float _hitsLeft;

    private void Awake()
    {
        _instanceMaterial = Instantiate(_material);
        GetComponent<MeshRenderer>().material = _instanceMaterial;
        _hitsLeft = _hits;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.ProjectileTag))
        {
            --_hitsLeft;
            _instanceMaterial.color = new Color(_instanceMaterial.color.r, 
                                                _instanceMaterial.color.g, 
                                                _instanceMaterial.color.b,
                                                _instanceMaterial.color.a - 1/_hits);

            if(_hitsLeft <= 0)
            {
            countdownTimer.AddTime(_timerGain);
            Destroy(gameObject);

            //Destroy only happens after the current Update Loop
            gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag(Constants.TerrainTag))
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
