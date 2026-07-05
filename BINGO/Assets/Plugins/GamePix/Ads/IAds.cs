using System;

namespace GamePix.Ads
{
    public interface IAds
    {
        void InterstitialAd(Action onSuccess = null, Action onFail = null);
        void RewardAd(Action onSuccess, Action onFail = null, bool prompt = false);
    }
}