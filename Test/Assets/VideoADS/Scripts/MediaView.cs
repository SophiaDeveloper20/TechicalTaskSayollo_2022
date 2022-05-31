using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml;
using UnityEngine.Video;
using RequestAPI;

[RequireComponent(typeof(VideoPlayer))]
public class MediaView : MonoBehaviour
{
    [SerializeField] private Request request;

    private VideoPlayer _videoPlayer;

    private const string _adsURL = "https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/ad/vast";

    private static string _downloadedXmlFilePath;
    private static string _adsPath;


    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _adsPath = Path.Combine(Application.persistentDataPath, "ads_media.webm");
        _downloadedXmlFilePath = Path.Combine(Application.persistentDataPath, "file.xml");
    }

    public void ShowAds()
    {
        request.Get(_adsURL, OnRequestPassed);
    }

    private void OnRequestPassed(DownloadHandler downloadHandler) 
    {
        request.DownLoadFile(ParseXML(downloadHandler.text), _adsPath, OnMediaFileLoaded);
    }

    private string ParseXML(string xmlText)
    {
        File.WriteAllText(_downloadedXmlFilePath, xmlText);
        var doc = new XmlDocument();
        doc.Load(_downloadedXmlFilePath);
        return doc.GetElementsByTagName("MediaFile")[0].InnerText;
    }

    private void OnMediaFileLoaded()
    {
        ShowVideo(_adsPath);
    }

    private void ShowVideo(string videoFilePath)
    {
        _videoPlayer.url = videoFilePath;
        _videoPlayer.Play();
    }
}