using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileSource : MonoBehaviour
{
    [SerializeField]
    private Projectile _projectile;
    [SerializeField]
    private ObjectPool _objectPool;
    [SerializeField]
    private float _fireInterval;
    [SerializeField]
    private float _spreadRadius;

    private bool _shooting = false;
   

    public void StartShooting() 
    {
        if (!_shooting)
        {
            _shooting = true;
            StartCoroutine(ShootingCoroutine());
        }
    }

    public void StopShooting()
    {
        _shooting = false;
    }

    private IEnumerator ShootingCoroutine()
    {
        while (_shooting)
        {
            Shoot();
            yield return new WaitForSeconds(_fireInterval);
        }

    }
    private void Shoot()
    {
        var bullet = _objectPool.GetPooledObject().GetComponent<Projectile>();
        Vector3 spread = Random.Range(-_spreadRadius, _spreadRadius) * transform.right +
                         Random.Range(-_spreadRadius, _spreadRadius) * transform.up;
        bullet.transform.SetPositionAndRotation(transform.position + spread, transform.rotation);

        bullet.speed = bullet.BaseSpeed + transform.GetComponentInParent<Rigidbody>().velocity.magnitude;
        StartCoroutine(bullet.DestroyAfter(5));
    }

}
