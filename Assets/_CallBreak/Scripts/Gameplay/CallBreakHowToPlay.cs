using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace FGSOfflineCallBreak
{
    public class CallBreakHowToPlay : MonoBehaviour
    {
        public void OpenScreen() { gameObject.SetActive(true); }
        public void CloseScreen() { gameObject.SetActive(false); }
    }
}
