using UnityEngine;

namespace RocketUtils
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)FindObjectOfType(typeof(T));
                if (Instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                                   " is needed in the scene, but there is none.");
                }
                else
                {
                    OnAwake();
                }
            }
            else if (Instance != this)
            {
                Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
            }
        }        

        protected abstract void OnAwake();

        public static T Instance { get; private set; }
    }
}