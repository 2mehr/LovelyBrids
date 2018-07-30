using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class Manger_Game : MonoBehaviour {
    public Tabligh tab;


  
  
   
    public void Butten_Hom()
    {
        PlayerPrefs.SetInt("plus", 0);
        SceneManager.LoadScene(1);
    }


    int GameOverCount;

    public void Reset()
    {
        
        SceneManager.LoadScene(2);
        GameOverCount = PlayerPrefs.GetInt("plus");
        GameOverCount++;
        PlayerPrefs.SetInt("plus", GameOverCount);
        PlayerPrefs.SetString("Continue", "");
    }
}
