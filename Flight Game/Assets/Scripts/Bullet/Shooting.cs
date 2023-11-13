using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private Transform _firingPoint; 

    [SerializeField]
    private Projectile projectile;
    
    public void Shoot()
    {
        Instantiate(projectile, _firingPoint, false);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

}
