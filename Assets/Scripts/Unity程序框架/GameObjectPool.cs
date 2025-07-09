using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool{
	private Vector2 O = new Vector2(0, 0);

	private static GameObjectPool instance;

	private Dictionary<string, Queue<GameObject>> ObjectPool = new Dictionary<string, Queue<GameObject>>();

	private Dictionary<string, GameObject> LastAddObject = new Dictionary<string, GameObject>();

	private GameObject pool;

	public static GameObjectPool Instance
	{
		get 
		{ 
			if(instance == null)
			{
				instance= new GameObjectPool();
			}
            return instance;
        }
	}
	
	public GameObject CreateGameObject(GameObject prefab,Vector2 position)
	{
		if(prefab == null)
		{
			return null;
		}
		GameObject _object;
		if(!ObjectPool.ContainsKey(prefab.name) || ObjectPool[prefab.name].Count == 0)
		{
			_object = GameObject.Instantiate(prefab);
			AddObject(_object,false);
			if(pool == null)
			{
				pool = new GameObject("GameObjectPools");
			}
			GameObject childpool = GameObject.Find(prefab.name+"_Pool");
			if (!childpool)
			{
				childpool = new GameObject(prefab.name + "_Pool");
				childpool.transform.SetParent(pool.transform);
			}
			_object.transform.SetParent(childpool.transform);
		}
        _object = ObjectPool[prefab.name].Dequeue();
        if (_object == null)
		{
			_object = CreateGameObject(prefab,position);
		}
        _object.transform.position = position;
        _object.SetActive(true);
        return _object;
	}

    public GameObject CreateGameObject(GameObject prefab,bool isExist = false)
    {
        GameObject _object;
        string _name;
        _name = prefab.name;
        if (isExist)
        {
            _name = prefab.name.Replace("(Clone)", string.Empty);
        }
        if (!ObjectPool.ContainsKey(_name) || ObjectPool[_name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            AddObject(_object, false);
            if (pool == null)
            {
                pool = new GameObject("GameObjectPools");
            }
            GameObject childpool = GameObject.Find(prefab.name + "_Pool");
            if (!childpool)
            {
                childpool = new GameObject(prefab.name + "_Pool");
                childpool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childpool.transform);
        }
        _object = ObjectPool[_name].Dequeue();
		//退群
        _object.SetActive(true);
        return _object;
    }

    public void AddObject(GameObject obj,bool Des = true)
	{
		string _name = obj.name.Replace("(Clone)",string.Empty);
		if(!ObjectPool.ContainsKey(_name))
		{
			ObjectPool.Add(_name, new Queue<GameObject>());
		}
		ObjectPool[_name].Enqueue(obj);
		if (Des)
		{
            if (!LastAddObject.ContainsKey(obj.tag))
            {
                LastAddObject.Add(obj.tag, obj);
            }
            LastAddObject[obj.tag] = obj;
        }
        obj.SetActive(false);
	}

	public Queue<GameObject> GetQueue(string key)
	{
		if (ObjectPool.ContainsKey(key))
		{
			return ObjectPool[key];
		}
		else
		{
			return null;
		}
	}

    public GameObject GetLastAddGameObject(string key)
    {
        if (LastAddObject.ContainsKey(key))
        {
            return LastAddObject[key];
        }
        else
        {
            return null;
        }
    }

	public void ClearQueue()
	{
		ObjectPool.Clear();
		LastAddObject.Clear();
	}

	public void Dequeque(GameObject obj)
	{
        ObjectPool[obj.name].Dequeue();
    }
}
