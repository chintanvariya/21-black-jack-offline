using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FGSBlackJack
{
    public class CallBreakExitController : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI titleText;
        public TMPro.TextMeshProUGUI descriptionText;
        public void OpenScreen(string title, string description)
        {
            titleText.text = title;
            descriptionText.text = description;

            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentClosed += OnAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentFailed += OnAdFullScreenContentFailed;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdNotReady += OnInterstitialAdNotReady;
        }

        private void OnInterstitialAdNotReady()
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            BlackJackGameManager.instance.LeaveGame();
            CloseScreen();
        }

        private void OnAdFullScreenContentFailed(AdError obj)
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            BlackJackGameManager.instance.LeaveGame();
            CloseScreen();
        }

        private void OnAdFullScreenContentClosedHandler()
        {
            CallBreakUIManager.Instance.preLoaderController.ClosePreloader();
            BlackJackGameManager.instance.LeaveGame();
            CloseScreen();
        }

        private void OnDisable()
        {
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentClosed -= OnAdFullScreenContentClosedHandler;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdFullScreenContentFailed -= OnAdFullScreenContentFailed;
            GoogleMobileAds.Sample.InterstitialAdController.OnInterstitialAdNotReady -= OnInterstitialAdNotReady;
        }

        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "Yes":
                    if (BlackJackGameManager.isInGamePlay)
                    {
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

                    }
                    else
                    {
#if !UNITY_EDITOR
                        Application.Quit();
#elif UNITY_EDITOR
                        EditorApplication.isPlaying = false;
#endif
                    }
                    break;
                case "No":
                    CloseScreen();
                    break;
            }
        }

        public void CloseScreen()
        {
            gameObject.SetActive(false);
        }




    }
}
