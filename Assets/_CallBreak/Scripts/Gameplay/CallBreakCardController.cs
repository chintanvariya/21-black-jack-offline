using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FGSOfflineCallBreak
{
    public class CallBreakCardController : MonoBehaviour
    {
        public CardDetail cardDetail;

        public Image cardImage;

        public CallBreakUserController userController;
        public void DoAnimtion(Transform target, float timeDuration)
        {
            Debug.Log("=================");
            transform.DOMove(target.position, timeDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.SetParent(target);
                if (userController != null)
                    userController.OnCompleteAnimation();
                else
                    CallBreakUIManager.Instance.gamePlayController.SetAsLastSibling();
                Debug.Log("<color><b></b>OnCompleteAnimation</color>");

            });
        }
    }
}
