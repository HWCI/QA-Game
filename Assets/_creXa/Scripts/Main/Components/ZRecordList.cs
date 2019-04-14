using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace creXa.GameBase
{ 
    public class ZRecordList : MonoBehaviour
    {
        public ZSelectable[] Header;
        public ZRecordRoot Root;

        public void MarkColumn(int x)
        {
            for (int i = 0; i < Header.Length; i++)
                Header[i].Selected = (i == x);
        }

        public void SetHeaderClick(UnityAction<int> _action = null)
        {
            for (int i = 0; i < Header.Length; i++)
            {
                int x = i;
                Header[i].btn.onClick.AddListener(() => { if(_action != null) _action(x); MarkColumn(x); });
            }   
        }

        public ZRecord GetRecord(int idx)
        {
            return Root.GetItem(idx);
        }

        public ZRecord[] GetRecords()
        {
            return Root.GetItems();
        }

        public void RootInit()
        {
            Root.DestroyAll();
        }

        public ZRecord AddRecord(string key, string[] _field, UnityAction<ZRecord> _action = null)
        {
            ZRecord record = Root.Add();
            record.Init(key, _field);
            if(_action != null)
                record.SetOnClick(_action);
            return record;
        }

        public ZRecord AddRecord(string key, string[] _field, UnityAction<ZRecord, int> _action)
        {
            ZRecord record = Root.Add();
            record.Init(key, _field);
            if (_action != null)
                record.SetOnClick(_action);
            return record;
        }

    }
}
