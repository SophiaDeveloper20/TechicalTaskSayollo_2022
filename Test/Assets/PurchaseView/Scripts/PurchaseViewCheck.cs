using Newtonsoft.Json;
using RequestAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static JsonData;

public class PurchaseViewCheck : MonoBehaviour
{
    [SerializeField] private Request request;
    [SerializeField] private InputField email;
    [SerializeField] private InputField creditCardNumber;
    [SerializeField] private InputField expiration;

    [SerializeField] private List<InputField> inputFields;

    private const string _purchasePostURL = "https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/v1/gcom/action";

    private string _json;

    public void PostPurchaseData()
    {
        if (!Check(inputFields)) 
        {

            Debug.Log("Please type all credentiald in input fields!");
            return;
        }

        var data = new PurchaseFieldsData
        {
            Email = email.text,
            CreditCardNumber = creditCardNumber.text,
            ExpirationDate = expiration.text
        };

        _json = JsonConvert.SerializeObject(data);
        request.Post(_purchasePostURL, _json, OnPurchase);
    }

    private void OnPurchase(DownloadHandler downloadHandler)
    {
        Debug.Log(downloadHandler.text);
    }

    private bool Check(List<InputField> fields) 
    {
        var firstNullOrEmtpy = fields.FirstOrDefault(firstNullOrEmtpy => firstNullOrEmtpy.gameObject.GetComponent<FieldCheck>().CheckIsNullOrEmpty());
        return firstNullOrEmtpy == null;
    }
}
