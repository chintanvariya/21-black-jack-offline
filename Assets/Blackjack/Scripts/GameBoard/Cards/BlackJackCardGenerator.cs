using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackJackOffline
{
    public class BlackJackCardGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> cardSprites;

        //public List<Sprite> randomBoradCard = new List<Sprite>();
        public List<Sprite> totalBoradCard = new List<Sprite>();
        //List<Sprite> cards = new List<Sprite>();
        private void Awake()
        {
            //CreateTotalCard();
        }

        public void CreateTotalCard()
        {
            Debug.Log("BlackJackCardGenerator || CreateTotalCard");
            for (int i = 0; i < 6; i++)
                totalBoradCard.AddRange(cardSprites);

            totalBoradCard = totalBoradCard.OrderBy(a => Guid.NewGuid()).ToList();
        }

        internal void SetRendomCard()
        {
            //randomBoradCard = new List<Sprite>(cardSprites);
            //if (rendomBoradCard.Count > 0)
            //{
            //    rendomBoradCard.Clear();
            //}
            //foreach (var item in cardSprites)
            //{
            //    cards.Add(item);
            //}
            //for (int i = 0; i < 52; i++)
            //{
            //    int rendomIndex = Random.Range(0, cards.Count);
            //    rendomBoradCard.Add(cards[rendomIndex]);
            //    cards.RemoveAt(rendomIndex);
            //}
            //randomBoradCard = randomBoradCard.OrderBy(a => Guid.NewGuid()).ToList();
        }
    }
}
