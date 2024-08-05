using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

namespace FGSOfflineCallBreak
{
    public class CallBreakGamePlayController : MonoBehaviour
    {
        [Header("SELF USER")]
        public CallBreakUserController selfUser;
        [Header("GAME LOBBY AMOUNT")]
        public TMPro.TextMeshProUGUI gamePlayLobbyAmount;
        public TMPro.TextMeshProUGUI gamePlayRoundText;

        [Header("Buttons Transform")]
        public Button splitButton;
        public RectTransform dealButtonsParent;
        public RectTransform chipsButtonsParent;
        public RectTransform gamePlayButtonsParent;

        [Header("All Coins Value")]
        public List<int> allCoinsValue;
        [Header("All Coins Value Text")]
        public List<TMPro.TextMeshProUGUI> allCoinValueText;

        [Header("Current Player Turn Index")]
        public int currentPlayerTurn;

        [Header("Current Deal Amount")]
        public int currentDealAmount;
        public int reDealCurrentDealAmount;

        [Header("Coin Animation Object")]
        public GameObject coinAnimationObject;
        public TMPro.TextMeshProUGUI coinAnimationObjectText;

        [Header("Animation Variable")]
        public Transform startPosition, target;
        [Range(-500, 500)]
        public float jumpMultiplayer;

        [Range(0, 10)]
        public int jumpCount;

        [Range(0, 10)]
        public float jumpTime;

        public Transform deckPosition;
        public Transform dealersHandPosition;

        [Header("DEALER")]
        public bool isDealerTurn;

        public Transform dealerCardParent;
        public List<CallBreakCardController> dealersCards;

        [Header("DEALER SCORE HUD 01")]
        public GameObject scoreHud01;
        public TMPro.TextMeshProUGUI scoreHud01Text;

        public void OpenScreen()
        {
            CallBreakCardAnimation.instance.StopAllCoroutinesOnGamePlay();
            CallBreakCardAnimation.instance.SetAndStartGamePlay();
            CallBreakGameManager.isInGamePlay = true;
            //Card Deal Animation

            int minValue = CallBreakUIManager.Instance.dashboardController.currentLobbyPlay.minimumTableAmount;
            int maxValue = CallBreakUIManager.Instance.dashboardController.currentLobbyPlay.maximumTableAmount;

            int stepSize = (maxValue - minValue) / 4;

            allCoinsValue.Clear();
            // Calculate the intermediate values
            allCoinsValue.Add(minValue);
            allCoinsValue.Add(RoundToNearestHundred(minValue + stepSize));
            allCoinsValue.Add(RoundToNearestHundred(minValue + 2 * stepSize));
            allCoinsValue.Add(RoundToNearestHundred(minValue + 3 * stepSize));
            allCoinsValue.Add(maxValue);

            for (int i = 0; i < allCoinValueText.Count; i++)
            {
                allCoinValueText[i].text = CallBreakUtilities.AbbreviateNumber(allCoinsValue[i]);
                Debug.Log($"<color><b>indexOfValue => <color=green><b>{RoundToNearestHundred(allCoinsValue[i])} </b></color> ||</b></color>");
            }

            selfUser.ProfileAndNameDataSet();
            NewRound();
            gameObject.SetActive(true);
        }

        public void NewRound()
        {
            currentPlayerTurn = 0;
            reDealCurrentDealAmount = currentDealAmount;
            currentDealAmount = 0;
            selfUser.turnTimer.SelfUserTimer();
            dealButtonsParent.gameObject.SetActive(true);
            chipsButtonsParent.gameObject.SetActive(true);
            gamePlayButtonsParent.gameObject.SetActive(false);
            selfUser.ResetPlayerData();
            ResetDealerData();
        }


        int RoundToNearestHundred(int value)
        {
            int remainder = value % 100;
            if (remainder < 50)
                value -= remainder;
            else
                value += (100 - remainder);
            return value;
        }

        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "GamePlaydMenu":
                    CallBreakUIManager.Instance.menuController.OpenScreen("GamePlaydMenu");
                    break;
                case "Min":
                    OnButtonClickedBet(0);
                    break;
                case "Max":
                    OnButtonClickedBet(allCoinsValue.Count - 1);
                    break;
                case "Deal":
                    //START THE CARD DEALING
                    if (currentDealAmount == 0)
                    {
                        CallBreakUIManager.Instance.toolTipsController.OpenToolTips("Confirm", "put some chips", "");
                    }
                    else
                    {
                        selfUser.turnTimer.TimerObjectDeActivate();
                        dealButtonsParent.gameObject.SetActive(false);
                        chipsButtonsParent.gameObject.SetActive(false);
                        gamePlayButtonsParent.gameObject.SetActive(true);

                        StartCoroutine(CardDealAnimation());
                    }
                    break;
                case "ReDeal":
                    if (reDealCurrentDealAmount > 0 && CallBreakGameManager.instance.selfUserDetails.userChips > reDealCurrentDealAmount)
                    {
                        currentDealAmount += reDealCurrentDealAmount;
                        CallBreakGameManager.instance.selfUserDetails.userChips -= currentDealAmount;
                        CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(CallBreakGameManager.instance.selfUserDetails);
                        selfUser.ProfileAndNameDataSet();

                        selfUser.totalBetAmount = currentDealAmount;
                        selfUser.userTotalBetAmount.SetActive(true);
                        selfUser.userTotalBetAmountText.text = CallBreakUtilities.AbbreviateNumber(currentDealAmount);
                    }
                    else
                    {

                    }
                    break;
                case "Clear":
                    CallBreakGameManager.instance.selfUserDetails.userChips += currentDealAmount;
                    CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(CallBreakGameManager.instance.selfUserDetails);
                    selfUser.ProfileAndNameDataSet();
                    currentDealAmount = 0;
                    selfUser.userTotalBetAmount.SetActive(false);
                    break;
                case "DoubleBet":
                    if (currentDealAmount > 0)
                    {
                        currentDealAmount = currentDealAmount * 2;
                        coinAnimationObjectText.text = CallBreakUtilities.AbbreviateNumber(currentDealAmount);
                    }
                    break;
                case "Double":

                    break;
                case "Split":
                    selfUser.ClickedOnSplit();
                    break;
                case "Stand":
                    //Check for winner
                    if (selfUser.isSplit)
                    {
                        selfUser.currentMyCardListIndex++;
                        selfUser.UserTurnForHitAndStand();
                        if (selfUser.currentMyCardListIndex == 2)
                        {
                            StartCoroutine(nameof(ShowTheDealerCards));
                        }
                    }
                    else
                    {
                        StartCoroutine(nameof(ShowTheDealerCards));
                    }
                    break;
                case "Hit":
                    selfUser.UpdateMyCards(CallBreakCardAnimation.instance.RetrunCardObject(), deckPosition, 0.5f);
                    break;
                default:
                    break;
            }
        }

        string IsDealerDetermineWinner(int total1, int total2)
        {
            if (total1 == 21 || (total1 < 21 && total2 > 21))
            {
                Debug.Log($"<color><b>WIN DEALER => <color=green><b> {selfUser.ReturnDealerCardScore(selfUser.myCard01)}</b></color></b></color>");
                Debug.Log("Player wins!");
                return "Win";
            }
            else if (total2 == 21 || (total2 < 21 && total1 > 21))
            {
                Debug.Log($"<color><b>WIN DEALER => <color=green><b> {selfUser.ReturnDealerCardScore(selfUser.myCard01)}</b></color></b></color>");
                Debug.Log("DEALER wins!");
                return "Lose";
            }
            else if (Mathf.Abs(total1 - 21) < Mathf.Abs(total2 - 21))
            {
                Debug.Log("Player wins!");
                return "Win";
            }
            else if (Mathf.Abs(total2 - 21) < Mathf.Abs(total1 - 21))
            {
                Debug.Log("DEALER wins!");
                return "Lose";
            }
            else
            {
                //PUSH
                Debug.Log("It's a tie!");
                return "Push";
            }
        }

        public void UpdateTheUserWallet()
        {
            CallBreakUIManager.Instance.rewardCoinAnimation.CollectCoinAnimation("WinnerLoser");
        }

        public void AutoBetOnTimerOut()
        {
            selfUser.turnTimer.TimerObjectDeActivate();
            OnButtonClicked("Min");
            OnButtonClicked("Deal");
        }

        public void CloseScreen()
        {
            gameObject.SetActive(false);
            CallBreakCardAnimation.instance.StopAllCoroutinesOnGamePlay();
        }

        public void OnButtonClickedBet(int indexOfValue)
        {
            Debug.Log($"<color><b>indexOfValue => <color=green><b>{indexOfValue} </b></color> || BET VALUE => {allCoinsValue[indexOfValue]}</b></color>");

            if (CallBreakGameManager.instance.selfUserDetails.userChips > allCoinsValue[indexOfValue])
            {
                startPosition = allCoinValueText[indexOfValue].transform.parent;
                currentDealAmount += allCoinsValue[indexOfValue];

                CallBreakGameManager.instance.selfUserDetails.userChips -= currentDealAmount;
                CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(CallBreakGameManager.instance.selfUserDetails);
                selfUser.ProfileAndNameDataSet();

                coinAnimationObjectText.text = CallBreakUtilities.AbbreviateNumber(allCoinsValue[indexOfValue]);

                coinAnimationObject.transform.position = startPosition.position;
                coinAnimationObject.SetActive(true);

                coinAnimationObject.transform.DOJump(target.position, jumpMultiplayer, jumpCount, jumpTime).SetEase(Ease.Linear).OnComplete(() =>
                {
                    coinAnimationObject.SetActive(false);
                    selfUser.totalBetAmount = currentDealAmount;
                    selfUser.userTotalBetAmount.SetActive(true);
                    selfUser.userTotalBetAmountText.text = CallBreakUtilities.AbbreviateNumber(currentDealAmount);
                });
            }
            else
            {
                CallBreakUIManager.Instance.toolTipsController.OpenToolTips("Chips", "not enough chips ! buy some chips from store.", "");
            }
        }


        public IEnumerator CardDealAnimation()
        {
            for (int j = 0; j < 2; j++)
            {
                bool isSingleCardDeal = false;
                for (int i = 0; i < 2; i++)
                {
                    yield return new WaitForSeconds(0.5f);

                    if (!isSingleCardDeal)
                    {
                        isSingleCardDeal = true;
                        selfUser.UpdateMyCards(CallBreakCardAnimation.instance.RetrunCardObject(), deckPosition, 0.5f);
                    }
                    else
                        UpdateMyCards(CallBreakCardAnimation.instance.RetrunCardObject(), deckPosition, 0.5f);
                }
            }
            selfUser.UserTurnForHitAndStand();
        }



        public void ResetDealerData()
        {
            foreach (var item in dealersCards)
                Destroy(item.gameObject);

            dealersCards.Clear();
            UpdateTheScoreValue01("");
            ShowAndHideHud01(false);
        }

        public void UpdateTheScoreValue01(string value) => scoreHud01Text.text = value;
        public void ShowAndHideHud01(bool isActive) => scoreHud01.SetActive(isActive);
        public void SetAsLastSibling() => scoreHud01.transform.SetAsLastSibling();
        public IEnumerator ShowTheDealerCards()
        {
            if (selfUser.toolTipsText.text == "BUSTED")
            {
                MoveChips(target, dealerCardParent.transform, true);
            }
            else
            {
                foreach (var item in dealersCards)
                    item.cardImage.sprite = item.cardDetail.cardSprite;

                UpdateTheScoreValue01($"{selfUser.ReturnDealerCardScore(dealersCards)}");
                yield return new WaitForSeconds(1f);
                if (selfUser.ReturnDealerCardScore(dealersCards) >= 17)
                {
                    CheckWinnerFor();
                }
                else if (selfUser.ReturnDealerCardScore(dealersCards) < 17)
                {
                    UpdateMyCards(CallBreakCardAnimation.instance.RetrunCardObject(), deckPosition, 0.5f);
                    StartCoroutine(nameof(ShowTheDealerCards));
                }
            }
            SetAsLastSibling();
        }

        public void CheckWinnerFor()
        {
            switch (IsDealerDetermineWinner(selfUser.ReturnDealerCardScore(selfUser.myCard01), selfUser.ReturnDealerCardScore(dealersCards)))
            {
                case "Win":
                    MoveChips(target, selfUser.userChipsText.transform, false);
                    break;
                case "Lose":
                    MoveChips(target, dealerCardParent.transform, true);
                    break;
                case "Push":
                    MoveChips(target, selfUser.userChipsText.transform, false);
                    selfUser.UpdateUserToolTip("PUSH");
                    break;
                default:
                    break;
            }
        }


        public void UpdateMyCards(CallBreakCardController cardController, Transform startPosition, float jumpTime)
        {
            cardController.transform.position = startPosition.position;
            if (dealersCards.Count == 1)
            {
                cardController.cardImage.sprite = CallBreakCardAnimation.instance.cardBackSprite;
                UpdateTheScoreValue01($"{dealersCards[0].cardDetail.cardValue}");
                if (dealersCards[0].cardDetail.cardNumber == 14)
                {
                    Debug.Log("<color><b> Go for insurance </b></color>");
                }
            }
            ShowAndHideHud01(true);
            dealersCards.Add(cardController);
            Debug.LogError("=======> DEALER CARDS");
            scoreHud01.transform.SetAsLastSibling();
            cardController.DoAnimtion(dealerCardParent, jumpTime);
            SetAsLastSibling();
        }

        public void MoveChips(Transform start, Transform targetToWinner, bool isDealerWinner)
        {
            coinAnimationObjectText.text = CallBreakUtilities.AbbreviateNumber(currentDealAmount);
            coinAnimationObject.transform.position = start.position;

            selfUser.userTotalBetAmount.SetActive(false);
            coinAnimationObject.SetActive(true);

            coinAnimationObject.transform.DOMove(targetToWinner.position, jumpTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                coinAnimationObject.SetActive(false);
                if (isDealerWinner)
                {
                    //CallBreakGameManager.instance.selfUserDetails.userChips -= currentDealAmount;
                }
                else
                {
                    CallBreakGameManager.instance.selfUserDetails.userChips += currentDealAmount * 2;
                    CallBreakGameManager.instance.selfUserDetails.userKeys += CallBreakUIManager.Instance.dashboardController.currentLobbyPlay.minimumTableAmount / 8;
                }
                CallBreakConstants.UserDetialsJsonString = CallBreakUtilities.ReturnJsonString(CallBreakGameManager.instance.selfUserDetails);
                selfUser.ProfileAndNameDataSet();
                NewRound();
            });
        }


    }
}
