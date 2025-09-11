using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对象池
/// </summary>
public class GameObjectPool
{
	private Vector2 O = new Vector2(0, 0);

	private static GameObjectPool instance;
	/// <summary>
	/// 对象存储队列
	/// </summary>
	private Dictionary<string, Queue<GameObject>> ObjectPool = new Dictionary<string, Queue<GameObject>>();
	/// <summary>
	/// 
	/// </summary>
	private Dictionary<string, GameObject> LastAddObject = new Dictionary<string, GameObject>();
	/// <summary>
	/// 对象池父对象
	/// </summary>
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
	/// <summary>
	/// 从对象池取出一个对象
	/// </summary>
	/// <param name="prefab">对象</param>
	/// <param name="position">对象放置的位置</param>
	/// <returns>取出的对象</returns>
	public GameObject CreateGameObject(GameObject prefab,Vector2 position)
	{
		if(prefab == null)
		{
			return null;
		}
		GameObject _object;
		if(!ObjectPool.ContainsKey(prefab.name) || ObjectPool[prefab.name].Count == 0)//判断队列中有没有多余的对象
		{
			_object = GameObject.Instantiate(prefab);
			AddObject(_object,false);
			if(pool == null)
			{
				pool = new GameObject("GameObjectPools");
			}
			GameObject childpool = GameObject.Find(prefab.name+"_Pool");
			if (childpool==null)
			{
				childpool = new GameObject(prefab.name + "_Pool");
				childpool.transform.SetParent(pool.transform);
			}
			_object.transform.SetParent(childpool.transform);
		}
        _object = ObjectPool[prefab.name].Dequeue();//从对象池队列中取出对象
        if (_object == null)
		{
			_object = CreateGameObject(prefab,position);
		}
        _object.transform.position = position;
        _object.SetActive(true);
        return _object;
	}
    /// <summary>
    /// 从对象池取出一个对象
    /// </summary>
    /// <param name="prefab">对象</param>
    /// <param name="isExist"></param>
    /// <returns>取出的对象</returns>
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
	/// <summary>
	/// 把对象加入对象池
	/// </summary>
	/// <param name="obj">对象</param>
	/// <param name="Des"></param>
    public void AddObject(GameObject obj,bool Des = true)
	{
		string _name = obj.name.Replace("(Clone)",string.Empty);
		if(!ObjectPool.ContainsKey(_name))//判断字典有没有对应对象的队列
		{
			ObjectPool.Add(_name, new Queue<GameObject>());
		}
		ObjectPool[_name].Enqueue(obj);//把对象压入到队列中
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
	/// <summary>
	/// 获取指定的对象队列
	/// </summary>
	/// <param name="key">对象名字</param>
	/// <returns>对象队列</returns>
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
	/// <summary>
	/// 清空对象池
	/// </summary>
	public void ClearQueue()
	{
		ObjectPool.Clear();
		LastAddObject.Clear();
	}
	/// <summary>
	/// 取出对象
	/// </summary>
	/// <param name="obj">对象</param>
	public void Dequeque(GameObject obj)
	{
        ObjectPool[obj.name].Dequeue();
    }
}
