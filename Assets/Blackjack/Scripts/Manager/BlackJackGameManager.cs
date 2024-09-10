using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static FGSBlackJack.CallBreakRemoteConfigClass;

namespace FGSBlackJack
{

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
    public class BlackJackGameManager : MonoBehaviour
    {

        public bool isLogOff;

        [SerializeField]
        public UserDetails selfUserDetails;
        public static Sprite profilePicture;

        public List<AllLobbyDetail> allLobbyDetails;

        public static BlackJackGameManager instance;
        [Header("--------------------- Game Default Values --------------------- ")]
        [SerializeField, Range(5, 20)]
        internal float placeBetTimer;
        [SerializeField, Range(10, 20)]
        internal float turnTimer, insuranceTimer;
        [SerializeField]
        internal List<Sprite> userTurnSprites;

        [SerializeField]
        internal List<GameObject> gameScreens;

        [Header("--------------------- Managers --------------------- ")]
        [SerializeField]
        internal BlackJackGameBoardManager boradManager;
        [SerializeField]
        internal BlackJackBetButtons betButtons;
        [SerializeField]
        internal BlackJackPooler pooler;
        [SerializeField]
        internal BlackJackDealer dealer;
        [SerializeField]
        [Header("--------------------- User Info ---------------------")]
        internal Text userNameTxt;
        [SerializeField]
        internal Text craditPointTxt;
        [SerializeField]
        private Image profileImage;
        [SerializeField]
        internal List<Sprite> profileSprites;
        [Space]
        [Header("--------------------- User Info ---------------------")]
        public GameObject leavePopUp;
        [SerializeField] internal Text leavePopupText;

        public static bool isInGamePlay;

        [Space(5)]
        public List<Sprite> allProfileSprite = new List<Sprite>();

        public List<Sprite> allBotSprite = new List<Sprite>();

        public List<ParticleSystem> allProfileParticleSystem;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            Application.targetFrameRate = 70;
            Input.multiTouchEnabled = false;
            Time.timeScale = 1f;

            float sizeOfInt = sizeof(float);

            // Print the size
            Debug.Log("Size of integer: " + sizeOfInt + " bytes");
            Debug.unityLogger.logEnabled = isLogOff;
        }

        // Start is called before the first frame update
        void Start()
        {
            //foreach (var lobby in blackJackLobbies)
            //{
            //    lobby.LobbyJoinButton.onClick.AddListener(() => LobbyJoinClicked(lobby));
            //}
            //ScreenChange("GamePlay");
            //dealer.StartNewRound(false);
        }

        public void StartGamePlay()
        {
            ScreenChange("GamePlay");
            dealer.StartNewRound(false);
            UpdateUserInfo();
            isInGamePlay = true;
        }


        [SerializeField]
        internal string currentScreen = "";
        internal void ScreenChange(string screenName)
        {
            UpdateUserInfo();
            foreach (var screen in gameScreens)
            {
                if (screenName == screen.name)
                    screen.SetActive(true);
                else
                    screen.SetActive(false);
            }
            currentScreen = screenName;
        }

        #region Balance Format

        internal String SetBalanceFormat(float Amount)
        {
            if (Amount < 1000)
            {
                return Amount.ToString();
            }
            else if (Amount < 100000)
            {
                if (Amount % 1000 == 0)
                {
                    return (Amount / 1000).ToString() + "K";
                }
                else
                {
                    return (Amount / 1000).ToString("F1") + "K";
                }
            }
            else if (Amount < 10000000)
            {
                if (Amount % 100000 == 0)
                {
                    return (Amount / 100000).ToString() + "M";
                }
                else
                {
                    return (Amount / 100000).ToString("F1") + "M";
                }
            }
            else if (Amount < 1000000000)
            {
                if (Amount % 10000000 == 0)
                {
                    return (Amount / 10000000).ToString() + "B";
                }
                else
                {
                    return (Amount / 10000000).ToString("F1") + "B";
                }
            }
            else if (Amount < 100000000000)
            {
                if (Amount % 1000000000 == 0)
                {
                    return (Amount / 1000000000).ToString() + "T";
                }
                else
                {
                    return (Amount / 1000000000).ToString("F1") + "T";
                }
            }

            return Amount.ToString();
        }

        internal float SetRoundFormat(float Amount)
        {
            if (Amount < 1000)
            {
                return Amount;
            }
            else if (Amount < 100000)
            {
                return ((int)(Amount / 100)) * 100;
            }
            else if (Amount < 10000000)
            {
                return ((int)(Amount / 10000)) * 10000;
            }
            else if (Amount < 1000000000)
            {
                return ((int)(Amount / 1000000)) * 1000000;
            }
            else if (Amount < 100000000000)
            {
                return ((int)(Amount / 100000000)) * 100000000;
            }
            return Amount;
        }
        #endregion

        #region User Info

        internal void UpdateUserInfo()
        {
            //userNameTxt.text = userDetails.userName;
            //craditPointTxt.text = SetBalanceFormat(userDetails.userChips);
            //profileImage.sprite = profileSprites[userDetails.userAvatarIndex];
            betButtons.player.SetPlayerInfo();
        }

        #endregion

        public void SettingButtonClick(bool isLobby)
        {
            CallBreakUIManager.Instance.menuController.OpenScreen("GamePlaydMenu");
            //BlackJackSettingManager.instance.OpenSettingPanel(isLobby);
        }

        public void RateUs() => Application.OpenURL("https://play.google.com/store/apps/developer?id=Finix+Games+Studio");

        private void OnEnable()
        {
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentClosed += OnAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentFailed += OnAdFullScreenContentFailed;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdNotReady += OnInterstitialAdNotReady;
        }

        private void OnDisable()
        {
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentClosed -= OnAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentFailed -= OnAdFullScreenContentFailed;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdNotReady -= OnInterstitialAdNotReady;
        }

        public void OnInterstitialAdNotReady()
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            dealer.StopAllCoroutines();
            foreach (var screen in gameScreens)
                screen.SetActive(false);
            selfUserDetails.userKeys += totalRoundPlayed;
            Debug.LogError(selfUserDetails.userKeys);
            CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(BlackJackGameManager.instance.selfUserDetails);
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserChips();
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserKeys();
            CallBreakUIManager.Instance.dashboardController.OpenScreen();
        }

        public void OnAdFullScreenContentFailed(AdError error)
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            dealer.StopAllCoroutines();
            foreach (var screen in gameScreens)
                screen.SetActive(false);
            selfUserDetails.userKeys += totalRoundPlayed;
            Debug.LogError(selfUserDetails.userKeys);
            CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(BlackJackGameManager.instance.selfUserDetails);
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserChips();
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserKeys();
            CallBreakUIManager.Instance.dashboardController.OpenScreen();
        }

        private void OnAdFullScreenContentClosedHandler()
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            dealer.StopAllCoroutines();
            foreach (var screen in gameScreens)
                screen.SetActive(false);
            selfUserDetails.userKeys += totalRoundPlayed;
            Debug.LogError(selfUserDetails.userKeys);
            CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(BlackJackGameManager.instance.selfUserDetails);
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserChips();
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserKeys();
            CallBreakUIManager.Instance.dashboardController.OpenScreen();
        }

        public void OnButtonClickedLeavePopUp(string buttonName)
        {
            switch (buttonName)
            {
                case "Yes":
                    leavePopUp.SetActive(false);
                    isInGamePlay = false;
                    //dealer.StopAllCoroutines();
                    if (CallBreakConstants.callBreakRemoteConfig.flagDetails.isAds)
                    {
                        if (CallBreakConstants.callBreakRemoteConfig.adsDetails.isShowInterstitialAdsOnLobby)
                        {
                            CallBreakUIManager.Instance.preLoaderController.OpenPreloader();
                            GoogleMobileAds.Sample.InterstitialAdController.ShowInterstitialAd();
                        }
                        else
                            OnAdFullScreenContentClosedHandler();
                    }
                    else
                        OnAdFullScreenContentClosedHandler();

                    break;
                case "No":
                    leavePopUp.SetActive(false);
                    Time.timeScale = 1;
                    break;
                case "Open":
                    Time.timeScale = 0;
                    leavePopupText.text = "Are you sure ? \n you want to exit ?";
                    leavePopUp.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public BlackJackGameBoardManager gameBoardManager;
        public int totalRoundPlayed;
        public void LeaveGame()
        {
            isInGamePlay = false;
            foreach (var item in gameBoardManager.players)
                item.StopTimer();

            dealer.StopAllCoroutines();
            foreach (var screen in gameScreens)
                screen.SetActive(false);

            selfUserDetails.userKeys += totalRoundPlayed;
            Debug.LogError(selfUserDetails.userKeys);
            CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(BlackJackGameManager.instance.selfUserDetails);
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserChips();
            CallBreakUIManager.Instance.dashboardController.profileUiController.UpdateUserKeys();

            CallBreakUIManager.Instance.dashboardController.OpenScreen();
        }

    }
}

