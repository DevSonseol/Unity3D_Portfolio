using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public partial class Main : MonoBehaviour
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }

        //정리용 Root
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create() 
        {
            GameObject go = Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }
        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);   //업데이트 안받고 수면상태
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            if(_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.SetParent(parent);
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    // 키값 게임 오브젝트 이름
    [Header("Pooling")]
    public GameObject[] poolPrefabs;
    private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    private Transform _root;

    void InitPooling() 
    {
        if (_root == null)
        {
            _root = new GameObject { name = "Pool_Root" }.transform;
            _root.SetParent(transform);
        }
    }

    void CreatePool(GameObject original, int count = 5) 
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    void Push(Poolable poolable) 
    {
        string name = poolable.gameObject.name;

        //Pop을 한번도안하고 Push를하는경우 (ex.에디터상에서 드래그드롭으로 만들경우)
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        _pool[name].Push(poolable);
    }

    Poolable Pop(GameObject original, Transform parent = null) 
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }

    GameObject GetOriginal(string name) 
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Original;
    }

    void ClearPool()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}