using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using static FGSOfflineCallBreak.CallBreakRemoteConfigClass;
using static FGSOfflineCallBreak.CallBackLobbyClass;

namespace FGSOfflineCallBreak
{
    [CreateAssetMenu(fileName = "ManagerData", menuName = "ManagerData/BotsDetails", order = 1)]
    [Serializable]
    public class ManageData : ScriptableObject
    {
        public List<BotDetails> allBotDetails;
    }

    [Serializable]
    public class UserDetails
    {
        public string userName;
        public string userId;
        public float userChips;
        public float userKeys;
        public int userAvatarIndex;
        public int level = 1;
        public float levelProgress;
        public int removeAds;
        public UserGameDetails userGameDetails;
    }

    [Serializable]
    public class UserGameDetails
    {
        public int GamePlayed;
        public int GameWon;
        public int GameLoss;
    }

    [Serializable]
    public class BotDetails
    {
        public string userName;
        public string userId;
        public long userChips;
        public long userKeys;
        public int userAvatarIndex;
    }

    [System.Serializable]
    public class CardDetail
    {
        public int cardNumber;
        public int cardValue;
        public string cardName;
        public CardType cardType;
        public Sprite cardSprite;
    }
    public enum CardType { Spade, Diamond, Heart, Club }

    public enum GameModeType { None, MultiHand, Levels, Tournament }

    public sealed class CallBreakGameManager : MonoBehaviour
    {
        public static CallBreakGameManager instance;
        [SerializeField]
        public UserDetails selfUserDetails;
        public static Sprite profilePicture;

        public static bool isInGamePlay;

        public static Action UpdateUserDetails;

        public GenerateTheBots generateTheBots;

        public LobbyDetails lobbyDetails;

        public GameModeType gameModeType;

        private void Awake()
        {
            if (instance == null) instance = this;

            StopAllCoroutines();

            Application.targetFrameRate = 70;
            Input.multiTouchEnabled = false;
            Time.timeScale = 1f;

            float sizeOfInt = sizeof(float);

            // Print the size
            Debug.Log("Size of integer: " + sizeOfInt + " bytes");

        }

        internal List<CallBreakCardController> allTableCards = new List<CallBreakCardController>();

        [Header("CURRENT PLAYER INDEX")]
        public int currentPlayerIndex;

        private const float WinCardScale = 1.5f;
        private float delayTime = 0.1f;

        [Space(5)]

        public List<Sprite> allProfileSprite = new List<Sprite>();
        public List<GameObject> particles = new List<GameObject>();

        public int clubCardCounter;

        public CallBreakGamePlayController gamePlayController;

        internal void NextUserTurn()
        {

        }

        internal void WinnerTurn()
        {

        }

        public void RestartGame()
        {

        }

        public void TurnDataReset()
        {
            allTableCards.ForEach(c => c.gameObject.SetActive(false));
            allTableCards.ForEach(c => Destroy(c.gameObject));
            allTableCards.Clear();
        }
    }
}
