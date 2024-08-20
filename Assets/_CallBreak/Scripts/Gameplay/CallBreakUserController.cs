using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
namespace FGSOfflineCallBreak
{
    public class CallBreakUserController : MonoBehaviour
    {
        [Header("BOT DETAILS")]
        public BotDetails botDetails;

        [Header("STATIC SEAT INDEX")]
        public int staticSeatIndex;

        [Header("USER DETAILS REF")]
        public Image profilePicture;
        public TextMeshProUGUI userNameText;
        public TextMeshProUGUI userChipsText;

        [Space(10)]
        [Header("TOOL TIPS")]
        //public GameObject crownObj;
        public TextMeshProUGUI toolTipsText;
        public GameObject bidObjectToolTip;
        public GameObject dealerIcon;

        [Header("CARD PARENTS")]
        public Transform cardParent01;
        public Transform cardParent02;

        [Header("CARD PARENTS")]
        public HorizontalLayoutGroup horizontalLayoutGroup01;
        public HorizontalLayoutGroup horizontalLayoutGroup02;

        [Header("CARD PARENTS")]
        public Transform cardFoldPosition01;
        public Transform cardFoldPosition02;

        [Header("MY CARD 01 && 02")]
        public List<CallBreakCardController> myCard01;
        public List<CallBreakCardController> myCard02;

        [Header("IS SELF PLAYER")]
        public bool isSelfPlayer;

        [Header("IS DEALER")]
        public bool isDealer;

        [Header("USER TOTAL BET AMOUNT AND TEXT")]
        public GameObject userTotalBetAmount;
        public int totalBetAmount;
        public TMPro.TextMeshProUGUI userTotalBetAmountText;

        [Header("TIMER")]
        public CallBreakUserTurnTimer turnTimer;

        [Header("SCORE HUD 01")]
        public GameObject scoreHud01;
        public TMPro.TextMeshProUGUI scoreHud01Text;
        public void UpdateTheScoreValue01(string value) => scoreHud01Text.text = value;
        public void ShowAndHideHud01(bool isActive)
        {
            scoreHud01.SetActive(isActive);
            scoreHud01.transform.SetAsLastSibling();
        }

        [Header("SCORE HUD 02")]
        public GameObject scoreHud02;
        public TMPro.TextMeshProUGUI scoreHud02Text;
        public void UpdateTheScoreValue02(string value) => scoreHud02Text.text = value;
        public void ShowAndHideHud02(bool isActive)
        {
            scoreHud02.SetActive(isActive);
            scoreHud02.transform.SetAsLastSibling();
        }

        [Header("SPLIT FLAG")]
        public bool isSplit;
        public int currentSplitCardIndex;

        public void ResetUserTimer() => turnTimer.TimerObjectDeActivate();

        public void UserTurnStarted() => turnTimer.SelfUserTimer();

        internal void UpdateUserToolTip(string status) => toolTipsText.text = status;

        public int currentMyCardListIndex;

        internal void ProfileAndNameDataSet()
        {
            if (isSelfPlayer)
            {
                Debug.Log($"CallBreakGameManager.instance.selfUserDetails.userName {CallBreakGameManager.instance.selfUserDetails.userName}");
                userNameText.text = CallBreakGameManager.instance.selfUserDetails.userName;
                profilePicture.sprite = CallBreakGameManager.profilePicture;
                userChipsText.text = CallBreakUtilities.AbbreviateNumber(CallBreakGameManager.instance.selfUserDetails.userChips);
            }
            else
            {
                profilePicture.sprite = CallBreakGameManager.instance.generateTheBots.allBotSprite[botDetails.userAvatarIndex];
                userNameText.text = botDetails.userName;
            }
            UpdateUserToolTip("");
        }

        public void UpdateMyCards(CallBreakCardController cardController, Transform startPosition, float jumpTime)
        {
            cardController.transform.position = startPosition.position;
            cardController.userController = this;

            if (currentMyCardListIndex == 0)
            {
                cardController.DoAnimtion(cardParent01, jumpTime);
                myCard01.Add(cardController);
            }
            else if (currentMyCardListIndex == 1)
            {
                myCard02.Add(cardController);
                cardController.DoAnimtion(cardParent02, jumpTime);
            }

        }

        public void OnCompleteAnimation()
        {
            if (myCard01.Count >= 2)
            {
                ShowAndHideHud01(true);
                UpdateTheScoreValue01(ReturnScore(myCard01));
            }
            if (myCard02.Count >= 2)
            {
                ShowAndHideHud02(true);
                UpdateTheScoreValue02(ReturnScore(myCard02));
            }
            CallBreakUIManager.Instance.gamePlayController.splitButton.interactable = CheckForSplit();
        }

        public void FoldYourCards()
        {
            horizontalLayoutGroup01.enabled = false;
            horizontalLayoutGroup02.enabled = false;
            foreach (var item in myCard01)
            {
                item.cardImage.sprite = CallBreakCardAnimation.instance.cardBackSprite;
                item.transform.SetParent(transform);
            }
            foreach (var item in myCard02)
            {
                item.cardImage.sprite = CallBreakCardAnimation.instance.cardBackSprite;
                item.transform.SetParent(transform);
            }
            Invoke(nameof(CollectCards), 1f);
        }

        public void CollectCards()
        {
            foreach (var item in myCard01)
                item.transform.DOMove(myCard01[0].transform.position, 0.5f).SetEase(Ease.Linear);
            foreach (var item in myCard02)
                item.transform.DOMove(myCard02[0].transform.position, 0.5f).SetEase(Ease.Linear);

            Invoke(nameof(MoveCardsToWasteCard), 2f);
        }

        public void MoveCardsToWasteCard()
        {
            foreach (var item in myCard01)
                item.transform.DOMove(CallBreakUIManager.Instance.gamePlayController.dealersFoldPosition.transform.position, 0.5f).SetEase(Ease.Linear);
            foreach (var item in myCard02)
                item.transform.DOMove(CallBreakUIManager.Instance.gamePlayController.dealersFoldPosition.transform.position, 0.5f).SetEase(Ease.Linear);
        }

        public void UserTurnForHitAndStand()
        {
            HighlightsCards(new Vector3(.75f, .75f, .75f), myCard01);
            HighlightsCards(new Vector3(.75f, .75f, .75f), myCard02);

            if (!isSplit)
                HighlightsCards(Vector3.one, myCard01);
            else
                HighlightsCards(Vector3.one, myCard02);
        }

        public void HighlightsCards(Vector3 scaleOfCards, List<CallBreakCardController> cards)
        {
            foreach (var item in cards)
                item.transform.localScale = scaleOfCards;
        }

        public string ReturnScore(List<CallBreakCardController> cardControllers)
        {
            int score = 0;
            string scoreString = string.Empty;
            for (int i = 0; i < cardControllers.Count; i++)
            {
                score += cardControllers[i].cardDetail.cardValue;
                if (score > 21)
                {
                    OpenTheUserToolTips("BUSTED");
                    CallBreakUIManager.Instance.gamePlayController.OnButtonClicked("Stand");
                }
                else if (score > 21 && cardControllers[i].cardDetail.cardNumber == 14)
                {
                    scoreString = $"{score}";
                }
                else if (score < 21 && cardControllers[i].cardDetail.cardNumber == 14)
                {
                    scoreString = $"{score}/{score - 9}";
                }
                scoreString = $"{score}";
            }
            IEnumerable<CallBreakCardController> elementsWith14 = cardControllers.Where(x => x.cardDetail.cardNumber == 14);
            if (elementsWith14.Count() == 2)
            {

            }
            return scoreString;
        }

        public void UserStand()
        {

        }

        public void ShowAllCardOfDealer()
        {
            Debug.Log($"<color><b> ShowAllCardOfDealer </b></color>");
            for (int i = 0; i < myCard01.Count; i++)
                myCard01[i].cardImage.sprite = myCard01[i].cardDetail.cardSprite;

            ShowAndHideHud01(true);
            if (ReturnDealerCardScore(myCard01) >= 17)
            {
                Debug.Log($"<color><b> STAND </b></color>");
                CallBreakUIManager.Instance.gamePlayController.OnButtonClicked("Stand");
            }
            else if (ReturnDealerCardScore(myCard01) < 17)
            {
                Debug.Log($"<color><b> HIT </b></color>");
                CallBreakUIManager.Instance.gamePlayController.OnButtonClicked("Hit");
            }
        }

        public void ClickedOnSplit()
        {
            if (CheckForSplit())
            {
                isSplit = true;
                myCard01[1].transform.SetParent(cardParent02);
                myCard02.Add(myCard01[1]);
                myCard01.RemoveAt(1);

                cardParent02.gameObject.SetActive(true);
                totalBetAmount += totalBetAmount;
                userTotalBetAmountText.text = CallBreakUtilities.AbbreviateNumber(totalBetAmount);

                ShowAndHideHud01(true);
                UpdateTheScoreValue01(ReturnScore(myCard01));

                ShowAndHideHud02(true);
                UpdateTheScoreValue02(ReturnScore(myCard02));
            }

        }

        internal void OpenTheUserToolTips(string status)
        {
            UpdateUserToolTip(status);
            bidObjectToolTip.SetActive(true);
            bidObjectToolTip.transform.localScale = Vector3.zero;
            bidObjectToolTip.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutExpo);
        }


        internal void ResetPlayerData()
        {
            isSplit = false;
            totalBetAmount = 0;
            currentMyCardListIndex = 0;

            foreach (var item in myCard01)
                Destroy(item.gameObject);

            foreach (var item in myCard02)
                Destroy(item.gameObject);

            myCard01.Clear();
            myCard02.Clear();

            userTotalBetAmount.SetActive(false);

            ShowAndHideHud01(false);
            ShowAndHideHud02(false);

            cardParent02.gameObject.SetActive(false);
            UpdateUserToolTip("");
            bidObjectToolTip.SetActive(false);
            ResetUserTimer();
        }

        public bool CheckForSplit()
        {
            if (myCard01.Count >= 2)
            {
                if (myCard01[0].cardDetail.cardValue == myCard01[1].cardDetail.cardValue && myCard01[0].cardDetail.cardValue > 8)
                    return true;
                else
                    return false;
            }
            else return false;
        }

        public int ReturnDealerCardScore(List<CallBreakCardController> cardControllers)
        {
            int score = 0;
            for (int i = 0; i < cardControllers.Count; i++)
                score += cardControllers[i].cardDetail.cardValue;
            return score;
        }
    }
}





