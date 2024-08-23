using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using System;

namespace FGSOfflineCallBreak
{
    public sealed class CallBreakCardAnimation : MonoBehaviour
    {
        public CallBreakGamePlayController gamePlayController;

        public static CallBreakCardAnimation instance;

        public static bool isSplashShow = true;

        public List<Sprite> allStaticCard = new List<Sprite>();
        public List<Sprite> allShuffleCard = new List<Sprite>();

        [Space(10)]
        public CallBreakCardController prefabCard;
        public Transform cardParent;
        public Transform cardSpawnPoint;

        private const float cardSize = 1.4f;

        public Transform table;

        public List<CardDetail> allCardDetails = new List<CardDetail>();
        public List<CardDetail> discardedCardDetails = new List<CardDetail>();
        public List<CallBreakCardController> allCardsObject = new List<CallBreakCardController>();

        public Sprite cardBackSprite;

        private void Awake()
        {
            if (instance == null) instance = this;
            StopAllCoroutines();
            DOTween.KillAll();
        }

        public void StopAllCoroutinesOnGamePlay()
        {
            CallBreakGameManager.instance.TurnDataReset();
            CallBreakUIManager.Instance.userTurnTimer.ResetUserTimer();


            CallBreakUIManager.Instance.gamePlayController.selfUser.ResetPlayerData();

            CallBreakUIManager.Instance.bidSelectionController.CloseScreen();

            StopAllCoroutines();
            DOTween.KillAll();
        }

        internal void SetAndStartGamePlay()
        {
            allCardDetails.Clear();
            discardedCardDetails.Clear();

            // Shuffle Card
            allShuffleCard = allStaticCard.OrderBy(a => Guid.NewGuid()).ToList();
            for (int i = 0; i < allStaticCard.Count; i++)
            {
                CardDetail cardDetail = new CardDetail();
                string[] allStaticCardSplit = allShuffleCard[i].name.Split("-");
                cardDetail.cardType = CallBreakUtilities.ReturnMyCardType(allStaticCardSplit[0]);
                cardDetail.cardNumber = CallBreakUtilities.ReturnMyCardValue(allStaticCardSplit[1]);
                cardDetail.cardValue = CallBreakUtilities.ReturnValue(allStaticCardSplit[1]);
                cardDetail.cardName = allShuffleCard[i].name;
                cardDetail.cardSprite = allShuffleCard[i];
                allCardDetails.Add(cardDetail);
            }

            List<CardDetail> tempCards = new List<CardDetail>(allCardDetails);
            for (int i = 0; i < 7; i++)
            {
                allCardDetails.AddRange(tempCards);
            }
        }

        public Sprite ReturnCardSprite(string cardName)
        {
            Sprite sprite = null;
            for (int i = 0; i < allShuffleCard.Count; i++)
            {
                if (allShuffleCard[i].name == cardName)
                    sprite = allShuffleCard[i];
            }
            return sprite;
        }

        public CallBreakCardController RetrunCardObject()
        {
            CallBreakCardController cloneCard = Instantiate(prefabCard, cardParent);
            if (allCardDetails.Count > 0)
            {
                cloneCard.cardDetail = allCardDetails[0];
                //cloneCard.cardImage.sprite = cardBackSprite;
                cloneCard.cardImage.sprite = cloneCard.cardDetail.cardSprite;
                discardedCardDetails.Add(allCardDetails[0]);
                allCardDetails.RemoveAt(0);
            }
            else
            {
                Debug.Log("THERE IS NO CARDS");
            }
            return cloneCard;
        }
    }
}
