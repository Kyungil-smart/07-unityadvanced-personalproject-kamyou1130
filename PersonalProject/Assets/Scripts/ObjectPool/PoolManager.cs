using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    
    private Dictionary<GameObject, Queue<GameObject>> _pool = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        
    }

    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!_pool.ContainsKey(prefab))
        {
            _pool.Add(prefab, new Queue<GameObject>());
        }

        GameObject obj;

        if (_pool[prefab].Count > 0)
        {
            obj = _pool[prefab].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
            obj.name = prefab.name;
        }

        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);


        return obj;
    }

    public void Release(GameObject prefab, GameObject obj)
    {
        if (!_pool.ContainsKey(prefab))
        {
            _pool.Add(prefab, new Queue<GameObject>());
        }
        
        obj.SetActive(false);
        _pool[prefab].Enqueue(obj);
    }
}
