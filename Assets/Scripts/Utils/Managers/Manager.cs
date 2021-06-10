using UnityEngine;

namespace Utils.Managers
{
    public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get { return _instance; }
            private set
            {
                if (null == _instance)
                {
                    _instance = value;
                    _instance.gameObject.DontDestroyOnLoad();
                }
                else if (_instance != value)
                {
                    Destroy(value.gameObject);
                }
            }
        }

        public virtual void Awake()
        {
            Instance = this as T;
        }
    }
}