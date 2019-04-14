using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase
{
    public abstract class ZResLoader<U> : ZSingleton<U> where U : MonoBehaviour
    {
        public class SrcPool<T> where T : Object
        {
            public string relativePath;
            public string int2strFormat;
            Dictionary<string, T> src;

            public SrcPool(string _relativePath = "", string _int2strFormat = "")
            {
                relativePath = _relativePath;
                int2strFormat = _int2strFormat;
                Init();
            }

            public void Init()
            {
                src = new Dictionary<string, T>();
            }

            public T LoadFromResources(string key)
            {
                if (src == null) Init();
                T tmp = Resources.Load<T>(relativePath + key);
                if (tmp == null)
                {
                    Debug.LogWarning("Resources: " + (relativePath + key) + " not exists.");
                    return null;
                }
                src.Add(key, tmp);
                return tmp;
            }

            public T Get(int key)
            {
                return Get(key.ToString(int2strFormat));
            }

            public T Get(string key)
            {
                if (src == null) { Init(); return null; }
                if (src.ContainsKey(key)) return src[key];
                if (LoadFromResources(key)) return src[key];
                else return null;
            }

            public void Remove(string key)
            {
                if (src == null) { return; }
                if (src.ContainsKey(key)) src.Remove(key);
            }

            public void Clear()
            {
                src.Clear();
            }

            public void LoadAllFromResources()
            {
                if (src == null) { Init(); }
                T[] tmp = Resources.LoadAll<T>(relativePath);

                for (int i = 0; i < tmp.Length; i++)
                    src.Add(tmp[i].name, tmp[i]);                    
            }

            public int Count()
            {
                return src.Count;
            }
        }

        public class SrcList<T> where T : Object
        {
            public string relativePath;
            T[] src;

            public SrcList()
            {
                relativePath = "";
            }

            public SrcList(string _relativePath)
            {
                relativePath = _relativePath;
            }

            public T Get(int index)
            {
                if (index < 0 || index >= src.Length) return null;
                return src[index];
            }

            public void LoadAllFromResources()
            {
                src = Resources.LoadAll<T>(relativePath);
            }

            public int Length()
            {
                return src.Length;
            }

        }

    }
}
