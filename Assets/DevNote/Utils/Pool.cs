using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class Pool<T> where T : Component
    {
        private T _prefab;
        private List<T> _poolObjects;


        public Pool(T poolObject)
        {
            _prefab = poolObject;
            _poolObjects = new();

            if (poolObject.gameObject.IsPrefab() == false)
            {
                poolObject.gameObject.SetActive(false);
                _poolObjects.Add(poolObject);
            }
        }

        public T Get(Transform parent = null)
        {
            var poolObject = _poolObjects.Find(poolObject => !poolObject.gameObject.activeSelf);

            if (poolObject != null)
            {
                poolObject.gameObject.SetActive(true);
                poolObject.transform.SetParent(parent);
            }
            else
            {
                poolObject = Object.Instantiate(_prefab, parent);
                _poolObjects.Add(poolObject);
            }

            return poolObject;
        }

        public void Clear()
        {
            for (int i = 0; i < _poolObjects.Count; i++)
                ReturnToPool(_poolObjects[i]);
        }

        public void ReturnToPool(T poolObject) => poolObject.gameObject.SetActive(false);



    }
}



