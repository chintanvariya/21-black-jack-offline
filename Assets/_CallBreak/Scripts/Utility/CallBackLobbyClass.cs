using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FGSBlackJack.CallBreakRemoteConfigClass;

namespace FGSBlackJack
{
    [System.Serializable]
    public class CallBackLobbyClass
    {
        [System.Serializable]
        public class LobbyDetails
        {
            public List<AllLobbyDetail> allLobbyDetails;
        }
    }
}