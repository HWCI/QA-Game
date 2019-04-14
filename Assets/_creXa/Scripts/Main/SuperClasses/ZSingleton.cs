using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    /// <summary>
    /// Setting Up Current Class as Singleton Pattern
    /// </summary>
    /// <typeparam name="T">Should be the current class</typeparam>
    public abstract class ZSingleton<T> : ZStateCtrl where T : MonoBehaviour
    {
        public bool DontDestroy = true;
        private static bool applicationIsQuitting = false;

        private static T _instance;
        public static T It
        {
            get
            {
                if (applicationIsQuitting) return null;

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        sealed protected override void Awake()
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();
            else
            {
                if (this != _instance)
                {
                    DestroyImmediate(gameObject);
                    return;
                }
            }
            if (!transform.parent && DontDestroy)
                DontDestroyOnLoad(_instance.gameObject);
            
            AwakeRun();
        }

        public static bool HasInstance
        {
            get { return _instance != null; }
        }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        /// <summary>
        /// AwakeRun() will be called as Awake()
        /// Put everything that wanted to call in Awake() here instead
        /// </summary>
        virtual protected void AwakeRun() { }

        virtual public void SetActive(bool f)
        {
            if (gameObject.activeSelf != f) gameObject.SetActive(f);
        }

    }

}
