using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _pool;

    [SerializeField]
    private GameObject _object;

    public int poolSize;

    private int _validIndex;

    void Start()
    {
        _pool = new List<GameObject>();
        InitializeObjects();
        _validIndex = 0;
    }

    public GameObject GetPooledObject()
    {

        var obj = _pool[_validIndex];

        for(int i = (_validIndex + 1) % poolSize; i != _validIndex; i = ++i % poolSize)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _validIndex = i;
                Debug.Log(_validIndex);
                obj.SetActive(true);
                return obj;
            }
        }

        _validIndex = ResizePool();
        obj.SetActive(true);
        Debug.Log(_validIndex);
        return obj;
        
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
            GameObject obj = Instantiate(_object, transform);
            obj.SetActive(false);
            _pool.Add(obj);
        }
    }
}
