using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FGSBlackJack
{
    public class CallBreakPreLoaderController : MonoBehaviour
    {
        public void OpenPreloader() => gameObject.SetActive(true);
        public void ClosePreloader() => gameObject.SetActive(false);

    }
}
