using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameAnalyticsSDK;
using System;
public class MenuManger : MonoBehaviour {
    public Texture2D instaShire;
    bool isMute;
    public Image Mute;
    private bool isProcessing = false;
    public Image Panel_Ch;
    public Image Butten_Page_Ch;
    bool ISNext ;
  //  public Tabligh tab;
    public Image[] Lock = new Image[8];
    public Text Ghalb_Text;
   // public Text text_Error;
    // Use this for initialization
    void Awake () {
        Camera.main.GetComponent<AudioSource>().mute = Convert.ToBoolean(PlayerPrefs.GetInt("Sound"));
        isMute = Convert.ToBoolean(PlayerPrefs.GetInt("Sound"));
        Mute.enabled = isMute;
        PlayerPrefs.SetInt("plus", 0);
        GameAnalytics.Initialize();
       // PlayerPrefs.SetInt("Ghalb", 50000);
       // StartCoroutine(Tab());
        Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
        Tab_Start__Banaer.gameObject.SetActive(true);
    }
	
	// Update is called once per frame

    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }

    string subject = "LovelyBirds";
    string body = "https://cafebazaar.ir/app/com.rapidgame.lovelybirds/";
    public void shire()
    {
        //execute the below lines if being run on a Android device
#if UNITY_ANDROID
        //Refernece of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //Refernece of AndroidJavaObject class for intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        //call setAction method of the Intent object created
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //set the type of sharing that is happening
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        //add data to be passed to the other activity i.e., the data to be sent
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        //get the current activity
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //start the activity by sending the intent data
        currentActivity.Call("startActivity", intentObject);
#endif


    }
    public void Instageram(string pageName)
    {
        string pageAddress = "http://instagram.com/_u/" + pageName;

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
        intentObject.Call<AndroidJavaObject>("setData", uriClass.CallStatic<AndroidJavaObject>("parse", pageAddress));
        intentObject.Call<AndroidJavaObject>("setPackage", "com.instagram.android");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);

    }
    public void Telegram(string message)
    {

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

        intentObject.Call<AndroidJavaObject>("setPackage", "org.telegram.messenger");

        intentObject.Call<AndroidJavaObject>("setPackage", "org.thunderdog.challegram");
       


        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);
    }
    // Is play Sound For Game
   
    public void Sound()
    {
        isMute = Convert.ToBoolean(PlayerPrefs.GetInt("Sound"));
        isMute = !isMute;

        PlayerPrefs.SetInt("Sound", Convert.ToInt32(isMute));
        if (isMute == true)
        {
            Mute.enabled = true;
        }
        else
        {
            Mute.enabled = false;
        }

    }
    /// <summary>
    /// Page Character 
    /// </summary>
    public Tab_Item_Char Tab_Item_Char;
    public Tab_Start__Banaer Tab_Start__Banaer;
    public void ShowPanel_Ch()
    {
        Tab_Start__Banaer.gameObject.SetActive(false);
        Tab_Item_Char.gameObject.SetActive(true);
        Tab_Item_Char.LoadAd();
        Panel_Ch.gameObject.SetActive (true);
        Sd();

    }
    public void HidePanel_Ch()
    {
        Panel_Ch.gameObject.SetActive(false);
        Tab_Item_Char.gameObject.SetActive(false);

    }
    public GameObject Flash;
    public void Next_Page_char()
    {
        ISNext = !ISNext;
        if (ISNext == true)
        {
            Panel_Ch.GetComponentInChildren<Animator>().enabled = true;
            // Butten_Page_Ch.transform.Rotate(new Vector3(0, 180, 0));
            Panel_Ch.GetComponentInChildren<Animator>().Play("Page_Ch Animation");
            Flash.transform.Rotate(new Vector3(0, 0, 180));

        }
        else
        {
            Panel_Ch.GetComponentInChildren<Animator>().Play("Page_Ch Animation 0");
            //  Panel_Ch.GetComponentInChildren<Animator>().enabled = false;
            Flash.transform.Rotate(new Vector3(0, 0, -180));
        }


        print(ISNext);
    }

    public Text Text_ghalb;
  public void Select_char1(int d)
    {
        if (PlayerPrefs.GetInt("Ghalb")> d&&Lock[1].enabled==true)
        {
            PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") - d);
            //PlayerPrefs.SetInt("Select", d);
            PlayerPrefs.SetString("Char1", "True");
            Lock[1].enabled = false;
            Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
            PlayerPrefs.SetInt("Player", 1);
        }
        else if (Lock[1].enabled == false)
        {
            PlayerPrefs.SetString("Char1", "True");
            PlayerPrefs.SetInt("Player", 1);
        }
        else
        {
            Text_ghalb.text = "Low Ghalb";
            StartCoroutine(ErseText());
        }
    }
    public void Select_char2(int d)
    {
        if (PlayerPrefs.GetInt("Ghalb") > d&& Lock[2].enabled==true)
        {
            PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") - d);
           // PlayerPrefs.SetInt("Select", d);
            PlayerPrefs.SetString("Char2", "True");
            Lock[2].enabled = false;
            Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
            PlayerPrefs.SetInt("Player", 2);

        }
        else if (Lock[2].enabled == false)
        {
            PlayerPrefs.SetString("Char1", "True");
            PlayerPrefs.SetInt("Player", 2);
        }
        else
        {
            Text_ghalb.text = "Low Ghalb";
            StartCoroutine(ErseText());
        }
    }
    public void Select_char3(int d)
    {
        if (PlayerPrefs.GetInt("Ghalb") > d && Lock[3].enabled==true)
        {
            PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") - d);
           // PlayerPrefs.SetInt("Select", d);
            PlayerPrefs.SetString("Char3", "True");
            Lock[3].enabled = false;
            Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
            PlayerPrefs.SetInt("Player", 3);
        }
        else if (Lock[3].enabled == false)
        {
            PlayerPrefs.SetString("Char3", "True");
            PlayerPrefs.SetInt("Player", 3);
        }
        else
        {
            Text_ghalb.text = "Low Ghalb";
            StartCoroutine(ErseText());
        }
    }
    public void Select_char4(int d)
    {
        if (PlayerPrefs.GetInt("Ghalb") > d && Lock[4].enabled==true)
        {
            PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") - d);
          //  PlayerPrefs.SetInt("Select", d);
            PlayerPrefs.SetString("Char4", "True");
            Lock[4].enabled = false;
            Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
            PlayerPrefs.SetInt("Player", 4);
        }
        else if (Lock[4].enabled == false)
        {
            PlayerPrefs.SetString("Char4", "True");
            PlayerPrefs.SetInt("Player", 4);
        }
        else
        {
            Text_ghalb.text = "Low Ghalb";
            StartCoroutine(ErseText());
        }
    }
    public void Select_char5(int d)
    {
        if (PlayerPrefs.GetInt("Ghalb") > d && Lock[5].enabled == true)
        {
            PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") - d);
          //  PlayerPrefs.SetInt("Select", d);
            PlayerPrefs.SetString("Char5", "True");
            Lock[5].enabled = false;
            Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
            PlayerPrefs.SetInt("Player", 5);
        }
        else if (Lock[5].enabled == false)
        {
            PlayerPrefs.SetString("Char5", "True");
            PlayerPrefs.SetInt("Player", 5);
        }
        else
        {
            Text_ghalb.text = "Low Ghalb";
            StartCoroutine(ErseText());
        }
    }
    public void Select_char6(int d)
    {
        if (PlayerPrefs.GetInt("Ghalb") > d && Lock[6].enabled == true)
        {
            PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") - d);
           // PlayerPrefs.SetInt("Select", d);
            PlayerPrefs.SetString("Char6", "True");
            Lock[6].enabled = false;
            Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
            PlayerPrefs.SetInt("Player", 6);
        }
        else if (Lock[6].enabled == false)
        {
            PlayerPrefs.SetString("Char6", "True");
            PlayerPrefs.SetInt("Player", 6);
        }
        else
        {
            Text_ghalb.text = "Low Ghalb";
            StartCoroutine(ErseText());
        }
    }
    public void Select_char7(int d)
    {
        if (PlayerPrefs.GetInt("Ghalb") > d && Lock[7].enabled==true)
        {
            PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") - d);
           // PlayerPrefs.SetInt("Select", d);
            PlayerPrefs.SetString("Char7", "True");
            Lock[7].enabled = false;
            Ghalb_Text.text = PlayerPrefs.GetInt("Ghalb").ToString();
            PlayerPrefs.SetInt("Player", 7);
        }
        else if (Lock[7].enabled == false)
        {
            PlayerPrefs.SetString("Char7", "True");
            PlayerPrefs.SetInt("Player", 7);

        }
        else
        {
            Text_ghalb.text = "Low Ghalb";
            StartCoroutine(ErseText());
        }
    }
    public void Select_char(int d)
    {
       
            PlayerPrefs.SetString("Char", "True");
       // PlayerPrefs.SetInt("Select", 0);
        PlayerPrefs.SetInt("Player", 0);


    }
    private void Sd()
    {
       if(PlayerPrefs.GetString("Char1")=="True")
            Lock[1].enabled = false;
        if (PlayerPrefs.GetString("Char2") == "True")
            Lock[2].enabled = false;
        if (PlayerPrefs.GetString("Char3") == "True")
            Lock[3].enabled = false;
        if (PlayerPrefs.GetString("Char4") == "True")
            Lock[4].enabled = false;
        if (PlayerPrefs.GetString("Char5") == "True")
            Lock[5].enabled = false;
        if (PlayerPrefs.GetString("Char6") == "True")
            Lock[6].enabled = false;
        if (PlayerPrefs.GetString("Char7") == "True")
            Lock[7].enabled = false;
        
    }
    public Image Loding_Video;
    public void Play_Video()
    {
        

    }
   
 
    IEnumerator ErseText()
    {
        yield return new WaitForSeconds(2);
        Text_ghalb.text = "";
    }
   


    public void ExitGame()
    {
        Application.Quit();
    }

   

  

}

