using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private Projectile _projectile;
    [SerializeField]
    private ObjectPool _objectPool;
    [SerializeField]
    private float _fireInterval;

    private bool _cooldown = false;
    public void Shoot()
    {
        var bullet = _objectPool.GetPooledObject().GetComponent<Projectile>();
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
        bullet.speed = bullet.BaseSpeed +  transform.GetComponentInParent<Rigidbody>().velocity.magnitude;
        StartCoroutine(bullet.DestroyAfter(5));
    }

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
