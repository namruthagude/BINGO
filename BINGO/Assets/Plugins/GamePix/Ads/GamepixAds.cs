#if !UNITY_EDITOR && UNITY_WEBGL
using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace GamePix.Ads
{
    public class GamepixAds : IAds
    {
        [DllImport("__Internal")]
        private static extern void gpxInterstitialAd(
            [MarshalAs(UnmanagedType.FunctionPtr)] Gpx.gpxCallback onSuccess,
            [MarshalAs(UnmanagedType.FunctionPtr)] Gpx.gpxCallback onFail);

        [DllImport("__Internal")]
        private static extern void gpxRewardAd(
            [MarshalAs(UnmanagedType.FunctionPtr)] Gpx.gpxCallback onSuccess,
            [MarshalAs(UnmanagedType.FunctionPtr)] Gpx.gpxCallback onFail,
            bool prompt);

        private static bool isInterstitialAdCalled;
        private static Action interstitialAdOnSuccess;
        private static Action interstitialAdOnFail;

        private static bool isRewardAdCalled;
        private static Action rewardAdOnSuccess;
        private static Action rewardAdOnFail;

        public void InterstitialAd(Action onSuccess = null, Action onFail = null)
        {
            if (isInterstitialAdCalled)
            {
                Debug.Log("[GPX] Interstitial ad already called");
                return;
            }

            isInterstitialAdCalled = true;
            interstitialAdOnSuccess = onSuccess;
            interstitialAdOnFail = onFail;
            gpxInterstitialAd(InterstitialAdShown, InterstitialAdFailed);
        }

        public void RewardAd(Action onSuccess, Action onFail = null, bool prompt = false)
        {
            if (isRewardAdCalled)
            {
                Debug.Log("[GPX] Reward ad already called");
                return;
            }

            isRewardAdCalled = true;
            rewardAdOnSuccess = onSuccess;
            rewardAdOnFail = onFail;
            gpxRewardAd(RewardAdShown, RewardAdFailed, prompt);
        }

        [MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
        private static void InterstitialAdShown()
        {
            interstitialAdOnSuccess?.Invoke();
            FreeInterstitialAdsResources();
        }

        [MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
        private static void InterstitialAdFailed()
        {
            interstitialAdOnFail?.Invoke();
            FreeInterstitialAdsResources();
        }

        private static void FreeInterstitialAdsResources()
        {
            interstitialAdOnSuccess = null;
            interstitialAdOnFail = null;
            isInterstitialAdCalled = false;
        }

        [MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
        private static void RewardAdShown()
        {
            rewardAdOnSuccess?.Invoke();
            FreeRewardAdsResources();
        }

        [MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
        private static void RewardAdFailed()
        {
            rewardAdOnFail?.Invoke();
            FreeRewardAdsResources();
        }

        private static void FreeRewardAdsResources()
        {
            rewardAdOnSuccess = null;
            rewardAdOnFail = null;
            isRewardAdCalled = false;
        }
    }
}
#endif