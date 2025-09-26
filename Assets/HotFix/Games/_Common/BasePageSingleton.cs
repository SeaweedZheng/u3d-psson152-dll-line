using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BasePageSingleton<T> : PageBase where T : MonoBehaviour
    {
        /*
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        */

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var founds = FindObjectsOfType(typeof(T));
                    if (founds.Length > 1)
                    {
                        DebugUtils.LogError("[Weak Singleton] Singlton '" + typeof(T) +
                            "' should never be more than 1!");
                        return null;
                    }
                    else if (founds.Length > 0)
                    {
                        _instance = (T)founds[0];
                    }
                }
                return _instance;
            }
        }
        protected virtual void OnDisable()
        {
            _instance = null;
        }
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
