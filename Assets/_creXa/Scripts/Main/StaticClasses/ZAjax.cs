using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace creXa.GameBase
{
	public static class ZAjax
	{

        public static IEnumerator GetTexture(string url, Action<Sprite> Success, Action<string, string, WWWForm> Error = null, float timeOut = 30)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            www.timeout = Mathf.RoundToInt(timeOut);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Error(www.error, url, null);
                www.Dispose();
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                Success(sprite);
            }            
        }

        [Obsolete("Use GetSprite instead.")]
        public static Sprite WWWToSprite(UnityWebRequest www)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }

		public static IEnumerator SendRestoreForm(string url, WWWForm form = null, Action<UnityWebRequest, WWWForm> Success = null, Action<string, string, WWWForm> Error = null, float timeOut = 30, bool forWebGL = false)
		{
			if (!ZBase.It || !ZBase.It.isNetworkGame)
			{
				Error("Network Blocked.", url, form);
				yield break;
			}

            if (forWebGL)
            {
                form.headers.Add("Access-Control-Allow-Credentials", "true");
                form.headers.Add("Access-Control-Allow-Headers", "Accept");
                form.headers.Add("Access-Control-Allow-Methods", "POST");
                form.headers.Add("Access-Control-Allow-Origin", "*");
            }

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                www.timeout = Mathf.RoundToInt(timeOut);
                www.chunkedTransfer = false;

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Error(www.error, url, form);
                    www.Dispose();
                }
                else
                {
                    Success(www, form);
                }
            }
            
		}

		public static IEnumerator SendRestore(string url, WWWForm form = null, Action<UnityWebRequest> Success = null, Action<string, string, WWWForm> Error = null, float timeOut = 30, bool forWebGL = false)
        {
			if (!ZBase.It || !ZBase.It.isNetworkGame)
			{
				Error("Network Blocked.", url, form);
				yield break;
			}

            if (forWebGL)
            {
                form.headers.Add("Access-Control-Allow-Credentials", "true");
                form.headers.Add("Access-Control-Allow-Headers", "Accept");
                form.headers.Add("Access-Control-Allow-Methods", "POST");
                form.headers.Add("Access-Control-Allow-Origin", "*");
            }

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                www.timeout = Mathf.RoundToInt(timeOut);
                www.chunkedTransfer = false;

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Error(www.error, url, form);
                    www.Dispose();
                }
                else
                {
                    Success(www);
                }
            }
            
        }

        public static IEnumerator Send(string url, WWWForm form = null, Action<String> Success = null, Action<string, string, WWWForm> Error = null, float timeOut = 30, bool forWebGL = false)
        {
            if (!ZBase.It || !ZBase.It.isNetworkGame)
            {
                Error("Network Blocked.", url, form);
                yield break;
            }

            if (forWebGL)
            {
                form.headers.Add("Access-Control-Allow-Credentials", "true");
                form.headers.Add("Access-Control-Allow-Headers", "Accept");
                form.headers.Add("Access-Control-Allow-Methods", "POST");
                form.headers.Add("Access-Control-Allow-Origin", "*");
            }

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                www.timeout = Mathf.RoundToInt(timeOut);
                www.chunkedTransfer = false;

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Error(www.error, url, form);
                    www.Dispose();
                }
                else
                {
                    Success(www.downloadHandler.text);
                }
            }

        }

        public static IEnumerator Send(string url, byte[] rawData = null, Dictionary<string, string> headers = null, Action<UnityWebRequest> Success = null, Action<string, string, byte[], Dictionary<string, string>> Error = null, float timeOut = 30, bool forWebGL = false)
        {
            if (!ZBase.It || !ZBase.It.isNetworkGame)
            {
                Error("Network Blocked.", url, rawData, headers);
                yield break;
            }

            if (forWebGL)
            {
                headers.Add("Access-Control-Allow-Credentials", "true");
                headers.Add("Access-Control-Allow-Headers", "Accept");
                headers.Add("Access-Control-Allow-Methods", "POST");
                headers.Add("Access-Control-Allow-Origin", "*");
            }

            using (UnityWebRequest www = UnityWebRequest.Put(url, rawData))
            {
                www.timeout = Mathf.RoundToInt(timeOut);
                www.chunkedTransfer = false;
                foreach(string key in headers.Keys)
                {
                    www.SetRequestHeader(key, headers[key]);
                }
                

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Error(www.error, url, rawData, headers);
                    www.Dispose();
                }
                else
                {
                    Success(www);
                }
            }

        }

	}
}