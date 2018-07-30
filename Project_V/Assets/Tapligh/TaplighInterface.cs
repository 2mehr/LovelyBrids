using UnityEngine;
//using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public enum AdResult {
    NO_INTERNET_ACSSES,
    BAD_TOKEN_USED,
    NO_AD_READY,
    INTERNAL_ERROR,
    AD_AVAILABLE,
    AD_VIEWED_COMPLETELY,
    AD_CLICKED,
    AD_IMAGE_CLOSED,
    AD_VIDEO_CLOSED_AFTER_FULL_VIEW,
    AD_VIDEO_CLOSED_ON_VIEW,
    LOAD_ERROR_STATUS
}

public enum LoadErrorStatus {
    NO_INTERNET_ACCSSES,
    BAD_TOKEN_USED,
    AD_UNIT_DISABLED,
    AD_UNIT_NOT_FOUND,
    INTERNAL_ERROR,
    NO_AD_READY,
    AD_UNIT_NOT_READY
}

public enum TokenResult {
    TOKEN_NOT_FOUND,
    TOKEN_EXPIRED,
    NOT_USED,
    SUCCESS,
    INTERNAL_ERROR
}

class TaplighInterface : MonoBehaviour {

    #if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJavaClass _taplighJavaInterface;
        AndroidJavaObject _currentActivity;
    #endif

    static private TaplighInterface instance;
    static private string TaplighStr = "com.tapligh.sdk.ADView.TaplighUnity";

    static private Action<AdResult, string> _onAdListener = null;
    public System.Action<AdResult, string> OnAdListener {
        get { return _onAdListener; }
        set { _onAdListener = value; }
    }

    static private Action<string, string> _onLoadReadyListener = null;
    public System.Action<string, string> OnLoadReadyListener {
        get { return _onLoadReadyListener; }
        set { _onLoadReadyListener = value; }
    }

    static private Action<string, LoadErrorStatus> _onLoadErrorListener = null;
    public System.Action<string, LoadErrorStatus> OnLoadErrorListener {
        get { return _onLoadErrorListener; }
        set { _onLoadErrorListener = value; }
    }

    static private Action<TokenResult> _onTokenVerifyFinishedListener = null;
    public System.Action<TokenResult> OnTokenVerifyFinishedListener {
        get { return _onTokenVerifyFinishedListener; }
        set { _onTokenVerifyFinishedListener = value; }
    }

    static private Action<string> _onRewardReadyListener = null;
    public System.Action<string> OnRewardReadyListener {
        get { return _onRewardReadyListener; }
        set { _onRewardReadyListener = value; }
    }

    static public TaplighInterface Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject("TaplighUnityObject");
                obj.AddComponent<TaplighInterface>();
                instance = obj.GetComponent<TaplighInterface>();
            }
            return instance;
        }
    }
    
    void Awake() {
        Debug.Log("Tapligh CREATED - CURRENT AVTIVITY IS DONE");
        DontDestroyOnLoad(this.gameObject);
    }

    public void InitializeTapligh(string token) {
        Debug.Log("<<<<<<------ Start Initializing ------>>>>>>");
        Debug.Log("Object Created and Java calsses are initiated " + TaplighStr);

        #if !UNITY_EDITOR && UNITY_ANDROID
            _taplighJavaInterface = new AndroidJavaClass(TaplighStr);
            _taplighJavaInterface.CallStatic("initialize" , token );
            Debug.Log("Tapligh Initilizing is Done"); 
        #endif

        Debug.Log("END OF SET JAVA OBJECT");
    }

    public void LoadAd(string unitCode) {
        Debug.Log("<<<<<<------ Start Loading Ad ------>>>>>>");
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null){
                _taplighJavaInterface.CallStatic("loadAd", this.gameObject.name, "onAdReady", "onLoadError", unitCode);
            }else{
                Debug.Log(" Tapligh Object in Unity is Null");
            }
        #endif
        Debug.Log("<<<<<<------ Ad Loaded ------>>>>>>");
    }

    private void onAdReady(string response) {
        List<string> result = GetResultArguments(response);
        String unit = result[0];
        String token = result[1];
        // Debug.Log("<<<<<<------ On Ad Ready | Unit : " + unit + " -------->>>>>>");
        // Debug.Log("<<<<<<------ On Ad Ready | Token : " + token + " -------->>>>>>");
        if (_onLoadReadyListener != null) { _onLoadReadyListener(unit, token); }
    }

    private void onLoadError(string response) {
        List<string> result = GetResultArguments(response);
        String unit = result[0];
        String number = result[1];
        // Debug.Log("<<<<<<------ On Ad Ready | Unit : " + unit + " -------->>>>>>");
        // Debug.Log("<<<<<<------ On Ad Ready | Number : " + number + " -------->>>>>>");
        LoadErrorStatus res_num = (LoadErrorStatus)(Int32.Parse(number));
        if (_onLoadErrorListener != null) { _onLoadErrorListener(unit, res_num); } 
    }

    public void ShowAd(string unitCode) {
        Debug.Log("<<<<<<------ Start Show Ad ------>>>>>>");
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null) {
                _taplighJavaInterface.CallStatic("showAd", this.gameObject.name, "onAdResult", "onRewardReady", unitCode);
            } else { 
                Debug.Log("Tapligh Object in Unity is Null");
            }
        #endif
        Debug.Log("<<<<<<------ Ad Appeared ------>>>>>>");
    }

    private void onAdResult(string adResponse) {
        Debug.Log("<<<<<<------ On Ad Result ------>>>>>>");
        List<string> results = GetResultArguments(adResponse);
        AdResult response = (AdResult)(Int32.Parse(results[0]));
        if (_onAdListener != null) { _onAdListener(response, results[1]); } // result[1] is token
    }

    public void onRewardReady(string reward) {
        Debug.Log("<<<<<<------ On Reward Ready ------>>>>>>");
        if (_onRewardReadyListener != null) { _onRewardReadyListener(reward); }
    }

    public string GetTaplighVersion() {
        string taplighVersion = "";

        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null){
                taplighVersion = _taplighJavaInterface.CallStatic<string>("getTaplighVersion");
            }else{ 
                Debug.Log("Tapligh Object in Unity is Null"); 
            }
        #endif

        return taplighVersion; 
    }

    public void VerifyToken(string token) {
        Debug.Log("VERIFY TOKEN");

        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null) 
               _taplighJavaInterface.CallStatic("verifyToken", this.gameObject.name, "OnTokenVerifyJavaListener", token);
            else 
                Debug.Log("Tapligh Object in Unity is Null");
        #endif
    }

    public bool IsInitializeDone() {
        Debug.Log("Is Initialize Done");
        bool IsInitialize = false;

        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null)
                 IsInitialize = _taplighJavaInterface.CallStatic<bool>("isInitializeDone");
            else
                Debug.Log("Tapligh Object in Unity is Null");
        #endif

        return IsInitialize;
    }

    private void OnTokenVerifyJavaListener(string adResponse) {
        TokenResult response = (TokenResult)(Int32.Parse(adResponse));
        if (_onTokenVerifyFinishedListener != null) { _onTokenVerifyFinishedListener(response); }
    }

    public void SetTestEnable(bool isTestEnable) {
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null)
                _taplighJavaInterface.CallStatic("setTestEnable", isTestEnable  );
            else
                Debug.Log(" Tapligh Object in Unity is Null"); 
        #endif
        Debug.Log("SetTestEnable");
    }

    private List<string> GetResultArguments( string result ) {
        List<string> arguments = new List<string>();

        int deviderIndex = result.IndexOf(';');
        arguments.Add( result.Substring( 0 , deviderIndex ) );

        for ( ; deviderIndex < result.Length ; deviderIndex++) {
            if (result[deviderIndex] != ';') { break; }
        }

        arguments.Add( result.Substring( deviderIndex)); 

        return arguments; 
    }

}