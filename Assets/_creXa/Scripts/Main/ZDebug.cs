using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Base/Debug")]
    [RequireComponent(typeof(ZBase))]
    public class ZDebug : ZSingleton<ZDebug>
    {
        public bool SendLogToServer = false;

        public bool SendDebugLog = false;
        public bool SendWarning = false;
        public bool SendError = false;
        public bool SendException = true;
        public bool SendAssert = true;

        public bool SendLogOnApplicationQuit = false;
        public bool ExceptionQuit = true;

        class LogBlock
        {
            public DateTime _datetime;
            public string _condition;
            public string _stackTrace;
            public LogType _type;

            public LogBlock(string condition, string stackTrace, LogType type)
            {
                _datetime = DateTime.Now;
                _condition = condition;
                _stackTrace = stackTrace;
                _type = type;
            }

            override public string ToString()
            {
                return _datetime + "//" + _type + "\n" + _condition + "\n" + _stackTrace + "\n\n";
            }
        };

        List<LogBlock> logs;

        #region Definition

        public enum ErrorType
        {
            UnexpectedNullPointer = 100,
            NetworkConnection = 500,
            NotClassified = 999
        }

        protected override void AwakeRun()
        {
            if(SendLogToServer)
                logs = new List<LogBlock>();
        }

        #endregion

        void OnEnable()
        {
            if (SendLogToServer)
                Application.logMessageReceived += HandleException;
        }

        void OnDisable()
        {
            if (SendLogToServer)
                Application.logMessageReceived -= HandleException;
        }

        void HandleException(string condition, string stackTrace, LogType type)
        {
            if (!SendLogToServer && !ZBase.It.isNetworkGame) return;
            LogBlock tmp = new LogBlock(condition, stackTrace, type);

            switch (type)
            {
                case LogType.Log:
                    if (SendDebugLog) logs.Add(tmp);
                    break;
                case LogType.Warning:
                    if (SendWarning) logs.Add(tmp);
                    break;
                case LogType.Error:
                    if (SendError) logs.Add(tmp);
                    break;
                case LogType.Exception:
                    if (ExceptionQuit) { StartCoroutine(ExQuit(tmp)); }
                    else{ if (SendException) logs.Add(tmp); }
                    break;
                case LogType.Assert:
                    if (SendAssert) logs.Add(tmp);
                    break;
            }
        }

        string MakeLogString()
        {
            string rtn = "";
            foreach (LogBlock log in logs)
                rtn += log.ToString();
            return rtn;
        }

        IEnumerator ExQuit(LogBlock log)
        {
            if (!SendLogToServer)
            {
                WWWForm form = new WWWForm();
                form.AddField("LogData", MakeLogString());
                form.AddField("LogPath", ZBase.It.LogPath);
                yield return StartCoroutine(ZAjax.Send(ZBase.It.LogPHP, form, null, null, 30, ZBase.It.isWebGL));
            }
            if (!Application.isEditor)
                Application.Quit();
        }

        override protected void OnApplicationQuit()
        {
			base.OnApplicationQuit();
            if (SendLogToServer && SendLogOnApplicationQuit)
            {
                WWWForm form = new WWWForm();
                form.AddField("LogData", MakeLogString());
                form.AddField("LogPath", ZBase.It.LogPath);
                StartCoroutine(ZAjax.Send(ZBase.It.LogPHP, form, null, null, 30, ZBase.It.isWebGL));
            }
        }

    }
}
