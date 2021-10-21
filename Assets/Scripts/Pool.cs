using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolObject _prefab;
    [Space(10)]
    [SerializeField] private int _minCapacity;
    [SerializeField] private int _maxCapacity;
    [Space(10)]
    [SerializeField] private bool _autoExpand;

    private List<PoolObject> _pool;

    private void OnValidate()
    {
        if (_autoExpand)
        {
            _maxCapacity = Int32.MaxValue;
        }
    }

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        _pool = new List<PoolObject>(_minCapacity);

        for (int i = 0; i < _minCapacity; i++)
        {
            CreateElement();
        }
    }

    private PoolObject CreateElement(bool isActiveByDefalut = false)
    {
        var createObject = Instantiate(_prefab);
        createObject.gameObject.SetActive(isActiveByDefalut);

        _pool.Add(createObject);

        return createObject;
    }

    public bool TryGetElement(out PoolObject element)
    {
        foreach (var item in _pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public bool AreThereAnyActiveElements()
    {
        foreach (var item in _pool)
        {
            if (item.gameObject.activeInHierarchy) return false;
        }
        return true;
    }

    public PoolObject GetFreeElement()
    {
        if (TryGetElement(out var element))
        {
            return element;
        }
        if (_autoExpand)
        {
            return CreateElement(true);
        }

        if (_pool.Count < _maxCapacity)
        {
            return CreateElement(true);
        }

        throw new Exception("Pool is over! ");
    }

    public PoolObject GetFreeElement(Vector3 position)
    {
        var element = GetFreeElement();
        element.transform.position = position;
        return element;
    }

    public PoolObject GetFreeElement(Vector3 position, Quaternion rotation)
    {
        var element = GetFreeElement(position);
        element.transform.rotation = rotation;
        return element;
    }

    public PoolObject GetFreeElement(Vector3 position, Quaternion rotation, Color color)
    {
        var element = GetFreeElement(position, rotation);
        element.GetComponent<SpriteRenderer>().material.color = color;
        return element;
    }

    public PoolObject GetFreeElement(Vector3 position, Quaternion rotation, int generation)
    {
        var element = GetFreeElement(position, rotation);
        element.GetComponent<Asteroid>().ScaleAsteroid(generation);
        return element;
    }
}
