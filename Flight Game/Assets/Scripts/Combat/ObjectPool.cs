using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _pool;

    [SerializeField]
    private GameObject _object;

    public int poolSize;

    void Start()
    {
        _pool = new List<GameObject>();
        InitializeObjects();
    }

    public GameObject GetPooledObject()
    {
        foreach(GameObject obj in _pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        int index = ResizePool();
        _pool[index].SetActive(true);
        return _pool[index];
    }

    int ResizePool()
    {
        int oldSize = poolSize;
        poolSize = Mathf.RoundToInt(poolSize * 1.5f);
        InitializeObjects();
        return oldSize;
    }

    private void InitializeObjects()
    {
        for (int i = _pool.Count; i < poolSize; i++)
        {
            GameObject obj = Instantiate(_object);
            obj.SetActive(false);
            _pool.Add(obj);
        }
    }
}
