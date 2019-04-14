using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    public class ZStateCtrl : ExtendedMonoBehaviour
    {
        [SerializeField]
        private int _state;
        public int State
        {
            get { return _state; }
            set { NextState(value); }
        }

        protected virtual void NextState(int next)
        {
            _state = next;
        }

    }
}
