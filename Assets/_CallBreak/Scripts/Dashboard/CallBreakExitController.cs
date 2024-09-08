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

        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "Yes":
                    //if (CallBreakGameManager.isInGamePlay)
                    //{
                    //    CallBreakUIManager.Instance.dashboardController.OpenScreen();
                    //    CloseScreen();  
                    //}
                    //else
                    //{
#if !UNITY_EDITOR
                        Application.Quit();
#elif UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#endif
                    //}
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
