using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace creXa.GameBase
{
    public static class ZDateTime
    {

        public static DateTime? ParseMySQLDateTime(string _mysqlDate)
        {
            DateTime rtn;
            if (DateTime.TryParseExact(_mysqlDate, "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out rtn))
            {
                return rtn;
            }
            return null;
        }

        public static string ToMySQLDateTime(DateTime _date)
        {
            return _date.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}