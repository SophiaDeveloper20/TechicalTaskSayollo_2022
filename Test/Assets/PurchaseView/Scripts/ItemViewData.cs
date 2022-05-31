using UnityEngine;
using UnityEngine.UI;
using static JsonData;

public class ItemViewData : MonoBehaviour
{
    [SerializeField] private GameObject itemViewPrefab;
    [SerializeField] private GameObject paymentViewPrefab;
    [SerializeField] private Image item_image;
    [SerializeField] private Text title;
    [SerializeField] private Text price;

    private void Start()
    {
        item_image.type = Image.Type.Simple;
        item_image.preserveAspect = true;
    }

    public void SetData(ItemJsonData data, Sprite s)
    {
        title.text = data.title;
        price.text = $"{data.price} {data.currency}";
        item_image.sprite = s;
    }

    public void ShowScreen(bool show) 
    {
        paymentViewPrefab.SetActive(show);
        itemViewPrefab.SetActive(!show);
    }
}
