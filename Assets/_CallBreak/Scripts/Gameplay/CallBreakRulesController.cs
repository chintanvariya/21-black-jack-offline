using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace FGSBlackJack
{
    public class CallBreakRulesController : MonoBehaviour
    {
        public RectTransform contentTransfrom;

        public void OpenScreen()
        {
            gameObject.SetActive(true);
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
