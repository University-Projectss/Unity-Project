using System.Collections;
using UnityEngine;

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

    private bool _cooldown = false;
    public void Shoot()
    {
        var bullet = _objectPool.GetPooledObject().GetComponent<Projectile>();
        Vector3 spread = Random.Range(-_spreadRadius, _spreadRadius) * transform.right +
                         Random.Range(-_spreadRadius, _spreadRadius) * transform.up;
        bullet.transform.SetPositionAndRotation(transform.position + spread, transform.rotation);
        
        bullet.speed = bullet.BaseSpeed +  transform.GetComponentInParent<Rigidbody>().velocity.magnitude;
        StartCoroutine(bullet.DestroyAfter(5));
    }

    //TODO: Rework Input
    public void Update()
    {
        if (Input.GetMouseButton(0) && !_cooldown)
        {
            Shoot();
            StartCoroutine(Cooldown(_fireInterval));
            
        }
    }



    private IEnumerator Cooldown(float seconds)
    {
        _cooldown = true;
        yield return new WaitForSeconds(seconds);
        _cooldown = false;
    }

}
