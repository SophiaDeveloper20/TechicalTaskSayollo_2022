using Newtonsoft.Json;
using RequestAPI;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static JsonData;

[RequireComponent(typeof(ItemViewData))]
public class PurchaseItemView : MonoBehaviour
{
    [SerializeField] private Request request;

    private PostRequestData _testPostRequestData;
    private ItemJsonData _itemJsonData;
    private ItemViewData _purchaseViewData;

    private DownloadHandlerTexture _downloadHandlerTexture;

    private string _downloadedImagePath;

    private const string _postUrl = "https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/v1/gcom/ad";

    private bool _canClickBuyBtn;


    private void Awake()
    {
        _canClickBuyBtn = false;
        _purchaseViewData = GetComponent<ItemViewData>();
        _downloadedImagePath = Application.persistentDataPath + "Image.jpeg";
    }

    private void Start()
    {
        PostTestJsonBody();
    }

    public void PostTestJsonBody()
    {
        _testPostRequestData = new PostRequestData
        {
            data = "TestData"
        };
        var testJson = JsonConvert.SerializeObject(_testPostRequestData);
        request.Post(_postUrl, testJson, OnRequestPassed);
    }

    public void OnClickBuyBtn()
    {
        if (!_canClickBuyBtn) return;
        _purchaseViewData.ShowScreen(true);
    }

    private void OnRequestPassed(DownloadHandler downloadHandler)
    {
        _itemJsonData = JsonConvert.DeserializeObject<ItemJsonData>(downloadHandler.text);
        LoadImage(_itemJsonData.item_image);
    }

    private void LoadImage(string imageUrl)
    {
        request.Get(imageUrl, new DownloadHandlerTexture(), OnLoadedImage);
    }

    private void OnLoadedImage(DownloadHandler downloadHandler)
    {
        _canClickBuyBtn = true;
        var imageBytes = downloadHandler.data;
        File.WriteAllBytes(_downloadedImagePath, imageBytes);
        var fileData = File.ReadAllBytes(_downloadedImagePath);
        var texture2D = new Texture2D(2, 2);
        texture2D.LoadImage(fileData);
        var s = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
        Vector2.zero, 1f);
        SetData(_itemJsonData, s);
    }

    private void SetData(ItemJsonData itemJsonData, Sprite s)
    {
        _purchaseViewData.SetData(itemJsonData, s);
    }
}