using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.up;
    }

}
