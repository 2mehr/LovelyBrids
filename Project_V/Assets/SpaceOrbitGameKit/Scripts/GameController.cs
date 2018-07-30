using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class GameController : MonoBehaviour {

	/// <summary>
	/// Main game controller handles game states, updating scores, showing information on UI, creating new coins at the start of each round
	/// and managing gameover sequence. All the static variables used inside gamecontroller are available to use in other classes.
	/// </summary>

	public static int level;					//Current game level - Starts with 1
	public static int totalCoins = 30;			//Total coins available in each level.
	public static int remainingCoins;			//remaining coins player need to collect to finish the level
	public static int totalGatheredCoin;		//total number of collected coins before gameover
	public static bool isGameStarted;			//flag
	public static bool isGameFinished;			//flag
	public static bool isGameOver;              //flag
    public static int Ghalb;
    //reference to UI objects
    public GameObject uiLevelNumber;
	public GameObject uiScore;
	public GameObject uiTapToStart;
	public GameObject uiRestartBtn;
	public Canvas uiFinishPlane;
	public GameObject uiYourScore;
	public GameObject uiBestScore;
	public GameObject uiNewBestScore;
	public GameObject uiLogo;
    public GameObject Ghalb_Score;
	public  GameObject[] coin = new GameObject[2];						//coin prefab
		
	//private variables
	private Vector3 position;
	private float positionPrecision;
	private bool gameoverRunFlag;
	private bool isAdvancing;
	private int bestSavedScore;
	//private GameObject adManager;               //reference to ad manager object

    public Tabligh tabligh ;
    public Game_Over_ONgame Ten_GameOver;
    public Button Play_Tab_ForCountenu;
    List<AudioSource> Audio = new List<AudioSource>();
    public Image Loding_Panel;

	/// <summary>
	/// Init
	/// </summary>
	void Awake () {

        if (PlayerPrefs.GetString("Continue") != "Continue")
        {
            bestSavedScore = PlayerPrefs.GetInt("BestSavedScore", 0);
            level = PlayerPrefs.GetInt("GameLevel", 1);
            totalGatheredCoin = PlayerPrefs.GetInt("TotalGatheredCoin", 0);
        }
        else
        {
            bestSavedScore = PlayerPrefs.GetInt("BestSavedScore");
            level = PlayerPrefs.GetInt("GameLevel");
            totalGatheredCoin = PlayerPrefs.GetInt("TotalGatheredCoin");
        }
           
        //Manual override
        //level = 2;
    
        // adManager = GameObject.FindGameObjectWithTag ("AdManager");
       
      
		uiRestartBtn.SetActive (false);
		uiFinishPlane.gameObject.SetActive (false);
		positionPrecision = totalCoins / 19.0f;
		remainingCoins = totalCoins;
		isGameStarted = false;
		isGameFinished = false;
		isGameOver = false;
		isAdvancing = false;
		gameoverRunFlag = false;
		StartCoroutine(createCoinsInOrbit ());
        Audio.AddRange(GameObject.FindObjectsOfType<AudioSource>());
        foreach (var item in Audio)
        {
            if (PlayerPrefs.GetInt("Sound")==1)
            {
                item.mute = true;
            }
            else
            {
                item.mute = false;
            }
        }

	}
   

	void Start () {

		//show current level on UI
		uiLevelNumber.GetComponent<TextMesh> ().text = level.ToString ();

		if (level == 1) {
			uiLogo.SetActive (true);
		} else {
			uiLogo.SetActive (false);
		}

	}


	/// <summary>
	/// FSM
	/// </summary>
	void Update () {

		//check for level finish state
		if (remainingCoins <= 0) {
			advanceLevel ();
		}

		//hide "tapToStart" when game is started
		if (isGameStarted && uiTapToStart && !isGameOver) {
			uiTapToStart.SetActive (false);
			uiLogo.SetActive (false);
		}

		//Monitor score and update on UI
		uiScore.GetComponent<TextMesh> ().text = totalGatheredCoin.ToString ();
        Ghalb_Score.GetComponent<TextMesh>().text =PlayerPrefs.GetInt("Ghalb").ToString();

        //check for game finish event
        if (isGameOver)
        {
            StartCoroutine(runGameover());
            if (PlayerPrefs.GetInt("plus")>2)
            {
                Ten_GameOver.gameObject.SetActive(true);
                PlayerPrefs.SetInt("plus", 0);
            }
            if (level>=2)
            {
                tabligh.gameObject.SetActive(true);
                Play_Tab_ForCountenu.GetComponent<Image>().color = Color.white;
                Play_Tab_ForCountenu.GetComponent<Image>().raycastTarget = true;
            }
           

        }
        

        //debug
        //print("remainingCoins: " + remainingCoins);
}


	/// <summary>
	/// This will be called from other controllers.
	/// We need to run it just once.
	/// </summary>
	IEnumerator runGameover() {

		if (gameoverRunFlag)
			yield break;
		gameoverRunFlag = true;

       

        //show current score on UI
        uiYourScore.GetComponent<Text> ().text = totalGatheredCoin.ToString ();
		if (bestSavedScore < totalGatheredCoin) {
			//save new score as best score
			bestSavedScore = totalGatheredCoin;
			PlayerPrefs.SetInt("BestSavedScore", bestSavedScore);
			uiNewBestScore.SetActive (true);
		}
		//show best score on UI
		uiBestScore.GetComponent<TextMesh> ().text = bestSavedScore.ToString ();

		//show a full screen ad every now and then (default: once in each 4 gameover)
		//if (UnityEngine.Random.value > 0.75f) {
		//	//if (adManager)
		//	//	adManager.GetComponent<AdManager> ().showInterstitial ();
		//}

		yield return new WaitForSeconds (0.75f);
		uiRestartBtn.SetActive (true);
		uiFinishPlane.gameObject.SetActive (true);
		uiLogo.SetActive (true);
	}


	/// <summary>
	/// Advance to higher levels with increased difficulty.
	/// </summary>
	void advanceLevel() {

		if (isAdvancing)
			return;
		isAdvancing = true;

		isGameFinished = true;
		level++;
		PlayerPrefs.SetInt ("GameLevel", level);
		PlayerPrefs.SetInt ("TotalGatheredCoin", totalGatheredCoin);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

	}

    GameObject g;
    /// <summary>
    /// Create and position the coins on the orbit in realtime!
    /// </summary>
    IEnumerator createCoinsInOrbit() {
		for (int i = 1; i <= totalCoins; i++) {
			position = new Vector3 (Mathf.Sin( (float)i / (Mathf.PI * positionPrecision) ) * PlayerController.orbitRadius, Mathf.Cos( (float)i / (Mathf.PI * positionPrecision) ) * PlayerController.orbitRadius, 0.1f);
            int r = UnityEngine.Random.Range(1, 25);
           
            //if (r <= 22)
            //    g = coin[0];
            //else if (r >= 22)
            //{
            //    g = coin[1];
            //    r = 1;
            //}
               
			GameObject c = Instantiate (coin[0], position, Quaternion.Euler (0, 180, 0)) as GameObject;
			c.name = "Coin-" + i.ToString ();
			yield return new WaitForSeconds (0.01f);
		}
	}
    
    public void ResStart()
    {
        PlayerPrefs.SetString("Continue", "Continue");
    }



}
