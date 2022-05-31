using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RequestAPI
{
    public class Request : MonoBehaviour, IRequest
    {
        public void Get(string uri, DownloadHandler downloadHandler, Action<DownloadHandler> callback)
        {
            StartCoroutine(GetRequest(uri, downloadHandler, callback));
        }

        public void Get(string uri, Action<DownloadHandler> callback)
        {
            StartCoroutine(GetRequest(uri, callback));
        }

        public void Post(string uri, string json, Action<DownloadHandler> callback)
        {
            StartCoroutine(PostRequest(uri, json, callback));
        }

        public void DownLoadFile(string uri, string path, Action callback)
        {
            StartCoroutine(DownloadFile(uri, path, callback));
        }

        private void ProcessResult(UnityWebRequest webRequest, Action<DownloadHandler> callback)
        {
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Error due to connection");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    callback?.Invoke(webRequest.downloadHandler);
                    break;
            }
        }

        private IEnumerator GetRequest(string uri, Action<DownloadHandler> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                ProcessResult(webRequest, callback);
            }
        }

        private IEnumerator GetRequest(string uri, DownloadHandler downloadHandler, Action<DownloadHandler> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.downloadHandler = downloadHandler;
                yield return webRequest.SendWebRequest();

                ProcessResult(webRequest, callback);
            }
        }

        private IEnumerator PostRequest(string uri, string json, Action<DownloadHandler> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, "POST"))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                byte[] rawData = Encoding.UTF8.GetBytes(json);
                webRequest.uploadHandler = new UploadHandlerRaw(rawData);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    callback?.Invoke(webRequest.downloadHandler);
                }
            }
        }

        private IEnumerator DownloadFile(string url, string path, Action callback)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            uwr.downloadHandler = new DownloadHandlerFile(path);
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
                Debug.LogError(uwr.error);
            else
                callback?.Invoke();

        }
    }
}