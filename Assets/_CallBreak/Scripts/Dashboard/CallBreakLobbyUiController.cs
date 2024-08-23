using UnityEngine;
using UnityEngine.UI;

namespace FGSOfflineCallBreak
{

    public class CallBreakLobbyUiController : MonoBehaviour
    {
        [Header("BACK GROUND")]
        public Image backGround;

        [Header("TEXT")]
        public TMPro.TextMeshProUGUI lobbyTypeName;
        public TMPro.TextMeshProUGUI minAmountText;
        public TMPro.TextMeshProUGUI maxAmountText;
        public TMPro.TextMeshProUGUI keysAmountText;

        [Header("BUTTON TEXT")]
        public TMPro.TextMeshProUGUI buttonText;

        [Header("LOBBY AMOUNT")]
        public string keysAmount;

        public int minimumTableAmount;
        public int maximumTableAmount;

        [Header("Dashboard Controller")]
        public CallBreakDashboardController dashboardController;

        public void UpdateLobbyText(Sprite bg, string typeName, string minAmount, string maxAmount, string keys)
        {
            backGround.sprite = bg;
            lobbyTypeName.text = typeName;
            minAmountText.text = minAmount;
            maxAmountText.text = maxAmount;
            keysAmountText.text = keys;
            buttonText.text = $"{ minAmount } / {maxAmount}";
        }


        public void OnButtonClicked() => dashboardController.OnButtonPlayNow(this);
    }
}