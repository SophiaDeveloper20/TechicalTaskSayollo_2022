using UnityEngine;

namespace VideoADS.Scripts
{
    public class AdsUseBehaviour : MonoBehaviour
    {
        [SerializeField] private MediaView adsLoader;

        private void Start()
        {
            adsLoader.ShowAds();
        }
    }
}