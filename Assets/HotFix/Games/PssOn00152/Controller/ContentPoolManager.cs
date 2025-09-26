using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMaker;

namespace PssOn00152
{
    public class ContentPoolManager : MonoWeakSingleton<ContentPoolManager>
    {

        Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();
        protected void OnEnable()
        {
            this.pools.Clear();
            ObjectPool[] pools = transform.GetComponentsInChildren<ObjectPool>();
            foreach (ObjectPool pool in pools)
            {
                this.pools.Add(pool.name, pool);
            }
        }

        public GameObject GetObject(string name)
        {
            if (pools.ContainsKey(name))
            {
               return pools[name].GetObject().gameObject;
            }
            return null;
        } 
    }
}