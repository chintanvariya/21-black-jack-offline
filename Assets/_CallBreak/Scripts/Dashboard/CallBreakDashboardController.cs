using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental;
using GoogleMobileAds.Api;
using System;
using UnityEditor;
using BlackJackOffline;

namespace FGSOfflineCallBreak
{
    public enum LobbyType { Rookie, Newbie, Experienced, Gifted }
    public class CallBreakDashboardController : MonoBehaviour
    {
        [Header("HourlyRewardController")]
        public CallBreakHourlyRewardController hourlyRewardController;
        public CallBreakHourlyRewardController spinnerHourlyRewardController;

        [Header("Dashborad ICONS")]
        public List<GameObject> dashBoardIcons;
        public List<GameObject> allIcons;
        public List<GameObject> quickRoundObj;

        public Image standardModeBtn, quickModeBtn;
        public Sprite modeSelectBG, normalBG;
        public Color modeSelectColor, normalColor;
        public GameObject roundInfoToolTip;

        public TextMeshProUGUI userIDText;

        private const int DefaultRound = 5;

        public CallBreakProfileUiController profileUiController;

        [Header("LEVEL BUTTONS")]
        public List<Button> allLevelTypeButtons;
        public List<Image> allLevelType;
        public List<TextMeshProUGUI> allLevelTypeText;

        public CallBreakLobbyUiController lobbyPrefab;
        public Transform parentOfLobby;

        [Header("ALL LOBBIES")]
        public List<CallBreakLobbyUiController> allLobbies;
        public List<int> allLobbyAmount = new List<int>();
        public List<Sprite> practiesAndCoins;

        public Sprite freeLobbyBg;
        public Sprite coinLoobyBG;

        [Header("ALL LOBBIES TYPE")]
        public CallBreakLobbyTypeUi prefabLobbyTypeUi;
        public Transform parentOfLobbyType;
        public List<CallBreakLobbyTypeUi> allLevelTypes;

        public void OpenScreen()
        {
            try
            {
                //CallBreakConstants.callBreakRemoteConfig = new CallBreakRemoteConfigClass.CallBreakRemoteConfig();
                this.enabled = true;
                CallBreakGameManager.instance.selfUserDetails.userId = SystemInfo.deviceUniqueIdentifier.Substring(0, 8);
                userIDText.text = "User ID : " + CallBreakGameManager.instance.selfUserDetails.userId;

                profileUiController.OpenScreen();

                foreach (var item in allIcons)
                    item.transform.localScale = Vector3.one;

                allIcons[2].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                allLobbyAmount = new List<int>();
                //if (CallBreakConstants.callBreakRemoteConfig.flagDetails.isSuccess)
                //{
                //    allLobbyAmount = CallBreakConstants.callBreakRemoteConfig.levelDetails.allLobbyAmount;
                //    CallBreakConstants.coinsToClearLevel = CallBreakConstants.callBreakRemoteConfig.levelDetails.coinsToClearLevel;
                //}
                //else
                {
                    List<int> lobbyValue1 = new List<int> { 0, 10, 20, 30, 40, 50, 100, 200, 300, 400, 500 };
                    allLobbyAmount = new List<int>(lobbyValue1);
                    int lobby = 1000;
                    for (int i = 0; i < 39; i++)
                    {
                        allLobbyAmount.Add(lobby);
                        lobby += 500;
                    }
                }

                Debug.Log("===============>" + allLobbies.Count);
                if (allLobbies.Count == 0)
                {
                    for (int i = 0; i < CallBreakGameManager.instance.lobbyDetails.allLobbyDetails.Count; i++)
                    {
                        CallBreakLobbyTypeUi lobbyTypeUi = Instantiate(prefabLobbyTypeUi, parentOfLobbyType);

                        lobbyTypeUi.UpdateLobbyTypeText(CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].countryName);
                        lobbyTypeUi.staticIndex = i;
                        lobbyTypeUi.dashboardController = this;

                        allLevelTypes.Add(lobbyTypeUi);
                        for (int j = 0; j < CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].countryLevel.Count; j++)
                        {
                            CallBreakLobbyUiController cloneOfLobby = Instantiate(lobbyPrefab, parentOfLobby);
                            cloneOfLobby.dashboardController = this;

                            Sprite BG = (i % 2 == 0) ? freeLobbyBg : coinLoobyBG;

                            string min = CallBreakUtilities.AbbreviateNumber(CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].minAmount);
                            string max = CallBreakUtilities.AbbreviateNumber(CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].maxAmount);
                            string levelName = CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].countryLevel[j];
                            string keys = CallBreakUtilities.AbbreviateNumber(CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].minAmount / 8);
                            //Debug.Log($"<color><b>LEVEL NAME => <color=green><b>{levelName} </b></color> || MIN => {min} || MAX => {max} </b></color>");

                            cloneOfLobby.minimumTableAmount = CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].minAmount;
                            cloneOfLobby.maximumTableAmount = CallBreakGameManager.instance.lobbyDetails.allLobbyDetails[i].maxAmount;

                            cloneOfLobby.UpdateLobbyText(BG, levelName, min, max, keys);
                            allLobbies.Add(cloneOfLobby);
                        }
                        //Debug.Log($"<color=red><b>===============================</b></color>");
                    }
                }

                SelectedLobbyType(allLevelTypes[0]);

                for (int i = 0; i < allLevelTypes.Count; i++)
                    allLevelTypes[i].lobbyButton.interactable = false;

                for (int i = 0; i < CallBreakGameManager.instance.selfUserDetails.level; i++)
                    allLevelTypes[i].lobbyButton.interactable = true;

                gameObject.SetActive(true);
            }
            catch (Exception ex)
            {
                Debug.Log("=========>" + ex.ToString());
                throw;
            }
        }

        public void RoundSectionActivate(bool isActive)
        {
            foreach (var item in quickRoundObj)
            {
                item.SetActive(isActive);
            }
        }
        public void RoundBtnClick(int roundNumber)
        {
            RoundInfoObj(false);
            CallBreakSoundManager.PlaySoundEvent(SoundEffects.Click);
            string roundText = string.Empty;
            if (roundNumber == DefaultRound)
            {
                standardModeBtn.sprite = modeSelectBG;
                quickModeBtn.sprite = normalBG;
                RoundSectionActivate(true);
                roundText = "Standard";
            }
            else
            {
                quickModeBtn.sprite = modeSelectBG;
                standardModeBtn.sprite = normalBG;
                RoundSectionActivate(false);
                roundText = "Quick";
            }
            CallBreakGameManager.instance.totalRound = roundNumber;

            //foreach (var item in allLobbies)
            //    item.UpdateRoundText(roundText);
        }



        public void OnButtonClicked(int buttonIndex)
        {
            foreach (var item in allIcons)
                item.transform.localScale = Vector3.one;

            allIcons[buttonIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            switch (buttonIndex)
            {
                case 0:
                    CallBreakUIManager.Instance.dailyRewardManager.OpenScreen();
                    FirebaseController.instance.FirelogEvent("DailyBonus", "DashboardController", "CallBreak");
                    break;
                case 1:
                    CallBreakUIManager.Instance.itemPurchase.OpenScreen();
                    FirebaseController.instance.FirelogEvent("StoreStore", "DashboardController", "CallBreak");
                    break;
                case 2:
                    CallBreakUIManager.Instance.preLoaderController.OpenPreloader();
                    GoogleMobileAds.Sample.RewardedAdController.ShowRewardedAd();
                    FirebaseController.instance.FirelogEvent("100FreeCoins", "DashboardController", "CallBreak");
                    break;
                case 3:
                    CloseScreen();
                    if (spinnerHourlyRewardController.Ready())
                        CallBreakUIManager.Instance.spinnerController.OpenScreen(spinnerHourlyRewardController);
                    FirebaseController.instance.FirelogEvent("LuckySpin", "DashboardController", "CallBreak");
                    break;
                case 4:
                    CallBreakUIManager.Instance.collectRewardController.OpenCollectReward("Free Coins", 200, hourlyRewardController);
                    FirebaseController.instance.FirelogEvent("FreeCoins", "DashboardController", "CallBreak");
                    break;
                default:
                    break;
            }
        }

        public void InfoBtn(string roundState)
        {
            RoundInfoObj(true);

            if (roundState == "Standard")
            {
                roundInfoToolTip.GetComponent<RectTransform>().anchoredPosition = new Vector2(-175, -125);
                roundInfoToolTip.GetComponentInChildren<TextMeshProUGUI>().text = CallBreakConstants.StandardRoundInfoDes;
            }
            else
            {
                roundInfoToolTip.GetComponent<RectTransform>().anchoredPosition = new Vector2(115, -125);
                roundInfoToolTip.GetComponentInChildren<TextMeshProUGUI>().text = CallBreakConstants.QuickRoundInfoDes;
            }
        }

        public void RoundInfoObj(bool isActive)
        {
            roundInfoToolTip.SetActive(isActive);
        }


        public void UpdateTheLobbiesDetails()
        {
            for (int i = 0; i < allLobbies.Count; i++)
            {
                int lobbyAmount = allLobbyAmount[0];
                string keys = string.Empty;
                string round = "Standard";
                string playButton = string.Empty;
                string winAmount = string.Empty;

                Sprite practicesAndCoin;
                if (i == 0)
                {
                    keys = "Practice";
                    playButton = "Play";
                    winAmount = $"Free";
                    practicesAndCoin = practiesAndCoins[0];
                    //allLobbies[i].bg.sprite = freeLobbyBg;
                }
                else
                {
                    lobbyAmount = allLobbyAmount[i];
                    keys = $"+{CallBreakUtilities.AbbreviateNumber(lobbyAmount / 2)}";
                    practicesAndCoin = practiesAndCoins[1];
                    playButton = $"Play {CallBreakUtilities.AbbreviateNumber(lobbyAmount)}";
                    winAmount = $"{CallBreakUtilities.AbbreviateNumber(lobbyAmount * 4)}";
                    //allLobbies[i].bg.sprite = coinLoobyBG;
                }
                //allLobbies[i].UpdateLobbyText(practicesAndCoin, lobbyAmount, keys, round, playButton, winAmount);
            }

            gameObject.SetActive(true);
        }



        public void SelectedLobbyType(CallBreakLobbyTypeUi lobbyType)
        {
            foreach (var item in allLevelTypes)
                item.lobbyButtonImage.sprite = normalBG;
            foreach (var item in allLevelTypes)
                item.lobbyTypeText.color = normalColor;

            allLevelTypes[lobbyType.staticIndex].lobbyButtonImage.sprite = modeSelectBG;
            allLevelTypes[lobbyType.staticIndex].lobbyTypeText.color = modeSelectColor;

            switch (lobbyType.staticIndex)
            {
                case 0:
                    ResetAllLobby(0, 5);
                    break;
                case 1:
                    ResetAllLobby(5, 10);
                    break;
                case 2:
                    ResetAllLobby(10, 15);
                    break;
                case 3:
                    ResetAllLobby(15, 20);
                    break;
                case 4:
                    ResetAllLobby(20, 25);
                    break;
                case 5:
                    ResetAllLobby(25, 30);
                    break;
                case 6:
                    ResetAllLobby(30, 35);
                    break;
                case 7:
                    ResetAllLobby(35, 40);
                    break;
                case 8:
                    ResetAllLobby(40, 45);
                    break;
                case 9:
                    ResetAllLobby(45, 50);
                    break;
                case 10:
                    ResetAllLobby(50, 55);
                    break;
                case 11:
                    ResetAllLobby(55, 60);
                    break;
                case 12:
                    ResetAllLobby(60, 65);
                    break;

                default:
                    break;
            }
        }

        public void ResetAllLobby(int start, int end)
        {
            foreach (var item in allLobbies)
                item.gameObject.SetActive(false);

            for (int i = start; i < end; i++)
                allLobbies[i].gameObject.SetActive(true);
        }

        public CallBreakLobbyUiController currentLobbyPlay;

        public void OnButtonPlayNow(CallBreakLobbyUiController lobbyUiController)
        {
            currentLobbyPlay = lobbyUiController;
            if (CallBreakConstants.callBreakRemoteConfig.adsDetails.isShowInterstitialAdsOnLobby)
            {
                CallBreakUIManager.Instance.preLoaderController.OpenPreloader();
                GoogleMobileAds.Sample.InterstitialAdController.ShowInterstitialAd();
            }
            else
                OnAdFullScreenContentClosedHandler();
        }

        private void OnEnable()
        {
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentClosed += OnAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentFailed += OnAdFullScreenContentFailed;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdNotReady += OnInterstitialAdNotReady;

            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdFullScreenContentFailed += OnRewardedAdFullScreenContentFailed;
            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdFullScreenContentClosed += OnRewardedAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdNotReady += OnRewardedAdNotReady;
            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdGranted += OnRewardedAdGranted;

        }
        private void OnDisable()
        {
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentClosed -= OnAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentFailed -= OnAdFullScreenContentFailed;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdNotReady -= OnInterstitialAdNotReady;

            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdFullScreenContentFailed -= OnRewardedAdFullScreenContentFailed;
            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdFullScreenContentClosed -= OnRewardedAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdNotReady -= OnRewardedAdNotReady;
            GoogleMobileAds.Sample.RewardedAdController.OnRewardedAdGranted -= OnRewardedAdGranted;
        }

        public GameObject gamePlay;
        public BlackJackDealer dealer;
        public void OnInterstitialAdNotReady()
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            //CallBreakUIManager.Instance.toolTipsController.OpenToolTips("AdsIsNotReady", "Ad is not ready yet !!", "");

            profileUiController.CloseScreen();
            CloseScreen();
                           

            gamePlay.SetActive(true);
            //dealer.StartNewRound(false);

            //AdsIsNotReady
        }


        public void OnAdFullScreenContentFailed(AdError error)
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
        }

        private void OnAdFullScreenContentClosedHandler()
        {
            if (CallBreakGameManager.instance.selfUserDetails.userChips < currentLobbyPlay.minimumTableAmount)
            {
                CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
                CloseScreen();
                CallBreakUIManager.Instance.notEnoughCoinsController.OpenScreen("Not Enough Coins", "Insufficient coins! Watch a video for 500 free coins!", 500);
            }
            else
            {
                CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
                gamePlay.SetActive(true);
                dealer.StartNewRound(false);

                profileUiController.CloseScreen();
                CloseScreen();
            }
        }

        public void OnRewardedAdFullScreenContentFailed(AdError error)
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
        }

        public void OnRewardedAdGranted()
        {
            Debug.Log("CallBreakDashboardController || OnRewardedAdGranted ");
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            CallBreakUIManager.Instance.rewardCoinAnimation.CollectCoinAnimation("100Coins");
        }

        public void OnRewardedAdFullScreenContentClosedHandler()
        {

        }

        public void OnRewardedAdNotReady()
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            CallBreakUIManager.Instance.toolTipsController.OpenToolTips("AdsIsNotReady", "Ad is not ready yet !!", "");
        }


        public void CloseScreen() => this.enabled = false;

    }
}

