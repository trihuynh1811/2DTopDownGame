using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    GameObject _objectPoolEmptyHolder;

    static GameObject _gameObjectEmpty;
    static GameObject _UiEmpty;
    static GameObject _particleEmpty;

    public enum PoolType
    {
        GameObject,
        UI,
        ParticleSystem,
        None
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Object");

        _gameObjectEmpty = new GameObject("GameObjects");
        _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _UiEmpty = new GameObject("UI");
        _UiEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _particleEmpty = new GameObject("ParticleSystem");
        _particleEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawanPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookUpString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo { LookUpString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObject.FirstOrDefault();

        if (spawnableObj == null)
        {
            GameObject parentObject = SetParentObject(poolType);

            spawnableObj = Instantiate(objectToSpawn, spawanPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObj.transform.position = spawanPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObject.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookUpString == goName);

        if (pool == null)
        {
            Debug.LogWarning("trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObject.Add(obj);
        }
    }

    static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObject:
                return _gameObjectEmpty;
            case PoolType.UI:
                return _UiEmpty;
            case PoolType.ParticleSystem:
                return _particleEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }

    public class PooledObjectInfo
    {
        public string LookUpString;
        public List<GameObject> InactiveObject = new List<GameObject>();
    }
}
