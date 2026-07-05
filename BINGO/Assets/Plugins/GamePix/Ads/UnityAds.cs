#if UNITY_EDITOR || !UNITY_WEBGL
using System;

namespace GamePix.Ads
{
    public class UnityAds: IAds
    {
        public void InterstitialAd(Action onSuccess = null, Action onFail = null)
        {
            Gpx.Log("[Gpx] InterstitialAd");
            onSuccess?.Invoke();
        }

        public void RewardAd(Action onSuccess, Action onFail = null, bool prompt = false)
        {
            Gpx.Log("[Gpx] RewardAd");
            onSuccess();
        }
    }
}
#endif