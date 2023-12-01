using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] Vector3 vectorPoint;

    void OnCollisionEnter(Collision other)
    {
        if (checkPoints.Contains(other.gameObject))
        {
            vectorPoint = other.transform.position;
            Destroy(other.gameObject);
        }
    }

}
