using AOT;
using GamePix;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    [SerializeField] private float speed, tilt;
    private Vector3 target;

    private void Start()
    {
        target = transform.position;
    }

    private void Update()
    {
        if (Gpx.IsGamePaused)
        {
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if (transform.position == target)
        {
            if (target.y != 0.1f)
            {
                target.y = 0.1f;
                Gpx.Ads.RewardAd(Success, Fail);
            }
            else if (target.y == 0.1f)
            {
                target.y = 2f;
                Gpx.Ads.InterstitialAd(InterstitialAd);
            }
        }
        transform.Rotate(Vector3.up * tilt + Vector3.forward * tilt);
    }

    [MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    private static void InterstitialAd()
    {
       // Any actions after interstitial Ad
    }

    [MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    private static void Success()
    {
       // Any actions after success rewardAd
    }

    [MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
    private static void Fail()
    {
       // Any actions after fail rewardAd
    }
}