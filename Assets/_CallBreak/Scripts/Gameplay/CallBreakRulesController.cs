using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace FGSOfflineCallBreak
{
    public class CallBreakRulesController : MonoBehaviour
    {
        public RectTransform contentTransfrom;

        public void OpenScreen()
        {
            gameObject.SetActive(true);
            contentTransfrom.anchoredPosition = new Vector2(0, 0);
        }

        public void CloseScreen()
        {
            gameObject.SetActive(false);
        }

        public void NextSlideImage()
        {

        }

        public void BackSlideImage()
        {

        }

    }

}
