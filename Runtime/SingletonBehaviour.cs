using UnityEngine;

namespace RocketUtils
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T) FindObjectOfType(typeof(T));
                    if (_instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(T) +
                                                   " is needed in the scene, but there is none.");
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if(_instance && _instance != this)
                Destroy(gameObject);
        }

        private static T _instance;
    }
}