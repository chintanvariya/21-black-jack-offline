using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FGSBlackJack
{
    public class CallBreakItemPurchase : MonoBehaviour
    {
        public List<CallBreakItemPurchaseUi> allCoinPack;
        public List<string> allCoinPackString;
        public List<string> allCoinPackValue;

        public void OpenScreen()
        {
            CallBreakUIManager.Instance.toolTipsController.OpenToolTips("AdsIsNotReady", "Coming Soon", "");
            return;
            for (int i = 0; i < allCoinPack.Count; i++)
            {
                Debug.Log(allCoinPack[i]);
                allCoinPack[i].UpdateTheValue(CallBreakIAPManager.Instance.ReturnTheProduct(allCoinPackString[i]));
            }
            gameObject.SetActive(true);
        }

        public void CloseScreen() => gameObject.SetActive(false);
    }
}