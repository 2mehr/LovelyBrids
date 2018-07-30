using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tab_Item_Char : MonoBehaviour {

    string TOKEN = "WISVMG4VQZHADM5N52UFU9W3A6NKNF";
    public string UNIT_CODE = "619C302369313275A3C1E8D2081FD0";
    public int Reward_Video;
    public Text verifiedToken = null;
    public Text Ghalb_No;
    bool testMode = false;
    string _token = "";

    void Start()
    {
        TaplighInterface.Instance.InitializeTapligh(TOKEN);
        TaplighInterface.Instance.OnAdListener = OnAdResult;
      
        GetResultArguments("0;;;");
        IsInitialized();
    }

    /************************************************ Main methods ************************************************/

    public void LoadAd()
    {
        TaplighInterface.Instance.OnLoadReadyListener = OnAdReady;
        TaplighInterface.Instance.OnLoadErrorListener = OnLoadError;
        TaplighInterface.Instance.LoadAd(UNIT_CODE);
    }

    public void ShowAd()
    {
        TaplighInterface.Instance.ShowAd(UNIT_CODE);
    }

    public void SetTestMode()
    {
        testMode = !testMode;
        string str = "";

        if (testMode)
        {
            str = "ON";
        }
        else
        {
            str = "OFF";
        }

        TaplighInterface.Instance.SetTestEnable(testMode);
        //testModeText.text  = "Test Mode is : " + str;
        verifiedToken.text = "Test Mode is : " + str;
    }

    public void VerifyToken()
    {
        TaplighInterface.Instance.OnTokenVerifyFinishedListener = OnVerifyListener;
        TaplighInterface.Instance.VerifyToken(_token);
    }

    public void GetTaplighVersion()
    {
        verifiedToken.text = "Tapligh SDK Version : " + TaplighInterface.Instance.GetTaplighVersion();
    }


    /************************************************ Private methods ************************************************/

    private void OnAdResult(AdResult result, string token)
    {
        string message = "Ad Result : ";

        switch (result)
        {
            case AdResult.BAD_TOKEN_USED: message += "Bad Token Used"; break;
            case AdResult.INTERNAL_ERROR: message += "Internal Error"; break;
            case AdResult.NO_AD_READY: message += "No Ad Ready"; break;
            case AdResult.NO_INTERNET_ACSSES: message += "No Internet Access"; break;
            case AdResult.AD_VIEWED_COMPLETELY: message += "Ad View Completelty"; break;
            case AdResult.AD_CLICKED: message += "Ad Clicked"; break;
            case AdResult.AD_IMAGE_CLOSED: message += "Ad Image Closed"; break;
            case AdResult.AD_VIDEO_CLOSED_AFTER_FULL_VIEW: message += "Ad Closed After Full View"; break;
            case AdResult.AD_VIDEO_CLOSED_ON_VIEW: message += "Ad Video Closed On View"; break;
        }

        Debug.Log(message);
        verifiedToken.text = message;
    }

    private void OnRewardReady(string reward)
    {
        Debug.Log("Your Reward is : (" + reward + ")");
        verifiedToken.text = "Reward : " + reward;
      
    }

    private void OnAdReady(string unit, string token)
    {
        Debug.Log("LOAD SUCCESSFULL. THE TOKEN IS : " + token);
        Debug.Log("LOAD SUCCESSFULL. THE UNIT IS : " + unit);
        if (verifiedToken != null)
        {
            _token = token;
            verifiedToken.text = "Token : " + token;
        }
    }

    private void OnLoadError(string unit, LoadErrorStatus error)
    {
        string message = "On Load Error : ";

        switch (error)
        {
            case LoadErrorStatus.NO_INTERNET_ACCSSES: message += "No Internet Access"; break;
            case LoadErrorStatus.BAD_TOKEN_USED: message += "Bad Token Used"; break;
            case LoadErrorStatus.AD_UNIT_DISABLED: message += "Ad Unit Disabled"; break;
            case LoadErrorStatus.AD_UNIT_NOT_FOUND: message += "Ad Unit Not Found"; break;
            case LoadErrorStatus.INTERNAL_ERROR: message += "Internal Error"; break;
            case LoadErrorStatus.NO_AD_READY: message += "No Ad Ready"; break;
            case LoadErrorStatus.AD_UNIT_NOT_READY: message += "Ad Unit Not Ready"; break;
        }

        Debug.Log("ON LOAD ERROR : THE UNIT IS : " + unit);
        Debug.Log(message);
        verifiedToken.text = message;
    }

    private List<string> GetResultArguments(string result)
    {
        List<string> arguments = new List<string>();

        int deviderIndex = result.IndexOf(';');
        arguments.Add(result.Substring(0, deviderIndex));
        Debug.Log("First Message : " + arguments[0]);

        for (; deviderIndex < result.Length; deviderIndex++)
        {
            if (result[deviderIndex] != ';')
                break;
        }

        arguments.Add(result.Substring(deviderIndex));
        Debug.Log("First Message : " + arguments[1]);

        return arguments;
    }

    private void OnVerifyListener(TokenResult tokenResult)
    {
        verifiedToken.text = "Token Verify : ";
        switch (tokenResult)
        {
            case TokenResult.INTERNAL_ERROR: verifiedToken.text += "ENTERNAL ERRROR"; break;
            case TokenResult.NOT_USED: verifiedToken.text += "NOT USED"; break;
            case TokenResult.SUCCESS: verifiedToken.text += "SUCCESS"; PlayerPrefs.SetInt("Ghalb", PlayerPrefs.GetInt("Ghalb") + Reward_Video); Ghalb_No.text = PlayerPrefs.GetInt("Ghalb").ToString() ; break;
            case TokenResult.TOKEN_EXPIRED: verifiedToken.text += "TOKEN EXPIRED"; break;
            case TokenResult.TOKEN_NOT_FOUND: verifiedToken.text += "TOKEN NOT FOUND"; break;

        }
    }

    private void IsInitialized()
    {
        verifiedToken.text = "Tapligh SDK Initialized : " + TaplighInterface.Instance.IsInitializeDone();
    }

}

