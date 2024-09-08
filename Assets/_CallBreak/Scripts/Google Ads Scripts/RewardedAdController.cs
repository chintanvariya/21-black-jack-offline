using System;
using UnityEngine;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Sample
{
    /// <summary>
    /// Demonstrates how to use Google Mobile Ads rewarded ads.
    /// </summary>
    [AddComponentMenu("GoogleMobileAds/Samples/RewardedAdController")]
    public class RewardedAdController : MonoBehaviour
    {
        /// <summary>
        /// UI element activated when an ad is ready to show.
        /// </summary>
        //public GameObject AdLoadedStatus;

        // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
        private const string _adUnitId = "ca-app-pub-5918737477932362/5933816206";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        private const string _adUnitId = "unused";
#endif

        private static RewardedAd _rewardedAd;

        /// <summary>
        /// Loads the ad.
        /// </summary>
        public static void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                DestroyAd();
            }

            Debug.Log("Loading rewarded ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            string adUnitId = string.Empty;

            if (CallBreakConstants.callBreakRemoteConfig.flagDetails.isSuccess)
#if UNITY_ANDROID
                adUnitId = CallBreakConstants.callBreakRemoteConfig.adsDetails.androidAdsIds.callBreakReward;
#elif UNITY_IPHONE
            adUnitId = CallBreakConstants.callBreakRemoteConfig.adsDetails.iosAdsIds.callBreakReward;
#else

#endif
            // Send the request to load the ad.
            else
                adUnitId = _adUnitId;

            // Send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // If the operation failed with a reason.
                if (error != null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }
                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _rewardedAd = ad;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);

                // Inform the UI that the ad is ready.
                //AdLoadedStatus?.SetActive(true);
            });
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public static void ShowRewardedAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                Debug.Log("Showing rewarded ad.");
                _rewardedAd.Show((Reward reward) =>
                {
                    Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}", reward.Amount, reward.Type));
                    OnRewardedAdGranted?.Invoke();
                });
            }
            else
            {
                OnRewardedAdNotReady?.Invoke();
                Debug.LogError("Rewarded ad is not ready yet.");
            }

            // Inform the UI that the ad is not ready.
            //AdLoadedStatus?.SetActive(false);
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public static void DestroyAd()
        {
            if (_rewardedAd != null)
            {
                Debug.Log("Destroying rewarded ad.");
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            // Inform the UI that the ad is not ready.
            //AdLoadedStatus?.SetActive(false);
        }

        /// <summary>
        /// Logs the ResponseInfo.
        /// </summary>
        public void LogResponseInfo()
        {
            if (_rewardedAd != null)
            {
                var responseInfo = _rewardedAd.GetResponseInfo();
                UnityEngine.Debug.Log(responseInfo);
            }
        }

        // Raised when the ad is estimated to have earned money.
        public static event Action<AdValue> OnRewardedAdPaid;
        // Raised when an impression is recorded for an ad.
        public static event Action OnRewardedAdImpressionRecorded;
        // Raised when a click is recorded for an ad.
        public static event Action OnRewardedAdClicked;
        // Raised when an ad opened full screen content.
        public static event Action OnRewardedAdFullScreenContentOpened;
        // Raised when the ad closed full screen content.
        public static event Action OnRewardedAdFullScreenContentClosed;
        // Raised when the ad failed to open full screen content.
        public static event Action<AdError> OnRewardedAdFullScreenContentFailed;
        // Raise when the ad in not ready
        public static event Action OnRewardedAdNotReady;
        //Rewarded ad granted
        public static event Action OnRewardedAdGranted;

        private static void RegisterEventHandlers(RewardedAd ad)
        {
            ad.OnAdPaid += (AdValue adValue) => OnRewardedAdPaid?.Invoke(adValue);
            ad.OnAdImpressionRecorded += () => OnRewardedAdImpressionRecorded?.Invoke();
            ad.OnAdClicked += () => OnRewardedAdClicked?.Invoke();
            ad.OnAdFullScreenContentOpened += () => OnRewardedAdFullScreenContentOpened?.Invoke();
            ad.OnAdFullScreenContentClosed += () =>
            {
                OnRewardedAdFullScreenContentClosed?.Invoke();
                LoadRewardedAd(); // Reload ad after it's closed
            };
            ad.OnAdFullScreenContentFailed += (AdError error) => OnRewardedAdFullScreenContentFailed?.Invoke(error);
        }
    }
}
