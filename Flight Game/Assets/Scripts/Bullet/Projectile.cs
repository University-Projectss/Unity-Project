using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private void Update()
    {
        transform.position += _speed * Time.deltaTime * transform.up;
    }

}
