using UnityEngine;
using System.Collections.Generic;

namespace creXa.GameBase
{
    /// <summary>
    /// Setting Up List to Gather Objects Under the Same Parent
    /// </summary>
    /// <typeparam name="T">Monobehaviour extends ZSpwan under this root</typeparam>
    public abstract class ZRoot<T> : MonoBehaviour where T : MonoBehaviour
    {
        public GameObject spwanObject;
        protected List<T> items = new List<T>();

        public delegate void OnListRemovedDel();
        public static OnListRemovedDel OnListRemoved;

        public delegate void OnListAddedDel();
        public static OnListAddedDel OnListAdded;

        public int Count
        {
            get { return (items == null)? 0 : items.Count; }
        }

        public void Add(T item, Vector3? defaultPos = null)
        {
            if (items.Contains(item)) return;

            items.Add(item);
            item.transform.SetParent(transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = defaultPos.HasValue ? (Vector3)defaultPos : Vector3.zero;

            if (OnListAdded != null) OnListAdded();
        }

        public T Add(Vector3? defaultPos = null)
        {
            if (spwanObject == null) return null;
            GameObject tmp = Instantiate(spwanObject);
            tmp.transform.SetParent(transform);
            tmp.transform.localScale = Vector3.one;
            T item = tmp.GetComponent<T>();
            
            if (ZLangSys.It)
            {
                ZLangObj[] lang = tmp.GetComponentsInChildren<ZLangObj>(true);
                if (lang != null && ZLangSys.It)
                    for (int j = 0; j < lang.Length; j++)
                        lang[j].Language = ZLangSys.It.AppLanguage;
            }
            if (ZThemeSys.It)
            {
                ZThemeObj[] theme = tmp.GetComponentsInChildren<ZThemeObj>(true);
                if (theme != null && ZThemeSys.It)
                    for (int j = 0; j < theme.Length; j++)
                        theme[j].Theme = ZThemeSys.It.Theme;
            }

            items.Add(item);

            if (OnListAdded != null) OnListAdded();

            return item;
        }

        public void DestroyAll()
        {
            if (items.Count == 0)
            {
                T[] itemsa = GetComponentsInChildren<T>();
                for (int i = 0; i < itemsa.Length; i++)
                    DestroyImmediate(itemsa[i].gameObject);
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                    DestroyImmediate(items[i].gameObject);
            }

            items = new List<T>();
        }
        
        public void Remove(T item, bool DESTROY = false)
        {
            if(!items.Contains(item)) return;
            items.Remove(item);

            if (DESTROY && item) DestroyImmediate(item.gameObject);
            if (OnListRemoved != null) OnListRemoved();
        }

        public T[] GetItems()
        {
            return items.ToArray();
        }

        public T GetItem(int index)
        {
            return items[index];
        }
    }
}
