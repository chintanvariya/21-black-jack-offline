using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGSOfflineCallBreak
{
    [System.Serializable]
    public class CallBackLobbyClass
    {
        [System.Serializable]
        public class LobbyDetails
        {
            public List<LobbyData> allLobbyDetails;
        }

        [System.Serializable]
        public class LobbyData
        {
            public string countryName;
            public int minAmount;
            public int maxAmount;
            public List<string> countryLevel;
            public List<Sprite> countryLevelSprite;
        }

    }
}