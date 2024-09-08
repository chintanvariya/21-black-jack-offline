using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGSBlackJack
{
    public class CallBreakLobbyTypeUi : MonoBehaviour
    {
        public int staticIndex;

        [Header("TEXT")]
        public TMPro.TextMeshProUGUI lobbyTypeText;
        public UnityEngine.UI.Image lobbyButtonImage;
        public UnityEngine.UI.Button lobbyButton;
        [Header("DASHBOARD")]
        public CallBreakDashboardController dashboardController;

        public void UpdateLobbyTypeText(string type) => lobbyTypeText.text = type;
        public void UpdateLobbyTypeImage(Sprite notmalOrSelected) => lobbyButtonImage.sprite = notmalOrSelected;

        public void OnButtonClicked() => dashboardController.SelectedLobbyType(this);
    }
}
