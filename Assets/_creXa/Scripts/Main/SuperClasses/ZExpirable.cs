using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace creXa.GameBase
{
    public class ZExpirable : MonoBehaviour
    {

        public DateTime? Updated = null;
        public int ExpiredMinute = 30;

        public bool Expired
        {
            get
            {
                if (!Updated.HasValue) return true;
                return (Updated.Value - DateTime.Now).TotalMinutes > ExpiredMinute;
            }
        }

        public void Renew()
        {
            Updated = DateTime.Now;
        }
    }
}