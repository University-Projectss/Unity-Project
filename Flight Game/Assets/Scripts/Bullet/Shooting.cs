using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private Transform _firingPoint; 

    [SerializeField]
    private Projectile projectile;
    
    public void Shoot()
    {
        var bullet = Instantiate(projectile, _firingPoint, false);
        bullet.transform.parent = null;
        bullet.speed += transform.GetComponent<Rigidbody>().velocity.magnitude;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

}
