using System;
using UnityEngine.Networking;

namespace RequestAPI
{
    public interface IRequest
    {
        void Get(string uri, Action<DownloadHandler> callback);
        void Get(string uri, DownloadHandler downloadHandler, Action<DownloadHandler> callback);
        void Post(string uri, string json, Action<DownloadHandler> callback);
        void DownLoadFile(string uri, string path, Action callback);
    }
}