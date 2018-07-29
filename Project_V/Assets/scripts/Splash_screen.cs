using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Splash_screen : MonoBehaviour {

    void Start()
    {

    var videoPlayer =     GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.Play();
        StartCoroutine(Next_Scene());
       
    }
    IEnumerator Next_Scene()
    {
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene(1);
    }

   
}