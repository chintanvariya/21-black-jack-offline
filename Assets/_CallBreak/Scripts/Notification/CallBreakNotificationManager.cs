//using System.Collections;
//using System.Collections.Generic;
//using Unity.Notifications.Android;
//using UnityEngine;

//namespace FGSBlackJack
//{
//    public class CallBreakNotificationManager : MonoBehaviour
//    {
//        // Start is called before the first frame update
//        void Start()
//        {
//            var channel = new AndroidNotificationChannel()
//            {
//                Id = "Channel_Id",
//                Name = "Default Channel",
//                Importance = Importance.Default,
//                Description = "Generic notification",
//            };

//            AndroidNotificationCenter.RegisterNotificationChannel(channel);

//            var notification = new AndroidNotification();
//            notification.Title = "Call Break: Your Move!";
//            notification.Text = "Get back to the table and claim your victory. Time to play!";
//            notification.SmallIcon = "icon_0";
//            notification.LargeIcon = "icon_1";

//            notification.FireTime = System.DateTime.Now.AddMinutes(120);

//            AndroidNotificationCenter.SendNotification(notification, "Channel_Id");
//        }
//    }
//}
////Title: "Your Call Break Game Awaits!"
////Message: "Ready for another round? Jump back in and outsmart your opponents!"

////Title: "It's Time for Call Break!"
////Message: "Sharpen your skills and show your mastery. Play Call Break now!"

////Title: "Call Break: Your Move!"
////Message: "Get back to the table and claim your victory. Time to play!"