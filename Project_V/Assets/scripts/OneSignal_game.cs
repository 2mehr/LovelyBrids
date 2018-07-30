using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSignal_game : MonoBehaviour {

	// Use this for initialization
	void Start () {
		OneSignal.StartInit("3aa3511a-f03a-4cb8-8734-ff200ab027d5")
       .HandleNotificationOpened(HandleNotificationOpened)
       .EndInit();

        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
    }

    // Gets called when the player opens the notification.
    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
