using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _baseSpeed;
    public float BaseSpeed { get { return _baseSpeed; } }
    public float Speed { get; set; }

    private void Update()
    {
        transform.position += Speed * Time.deltaTime * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Destroys the projectile after a certain amount of time
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
