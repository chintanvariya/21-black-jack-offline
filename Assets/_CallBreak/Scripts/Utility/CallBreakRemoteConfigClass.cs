using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGSBlackJack
{
    public class CallBreakRemoteConfigClass
    {
        [Serializable]
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class AdsIds
        {
            public string callBreakAppOpen;
            public string callBreakBanner;
            public string callBreakInterstitial;
            public string callBreakReward;
            public string callBreakRewardedInterstitial;
        }

        [Serializable]
        public class CallBreakRemoteConfig
        {
            public FlagDetails flagDetails;
            public AdsDetails adsDetails;
            public List<AllLobbyDetail> allLobbyDetails;
        }
        [Serializable]
        public class AllLobbyDetail
        {
            public string countryName;
            public int minAmount;
            public int maxAmount;
            public List<string> countryLevel;
            public List<Sprite> countryLevelSprite;
        }
        [Serializable]
        public class FlagDetails
        {
            public bool isAds;
            public bool isSuccess;
            public bool isForceUpdate;
        }
        [Serializable]
        public class AdsDetails
        {
            public bool isShowInterstitialAdsOnLobby;
            public bool isShowInterstitialAdsOnScoreBoard;
            public int numberOfAds;
            public AdsIds androidAdsIds;
            public AdsIds iosAdsIds;
        }
    }
}