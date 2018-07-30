using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour {

	/// <summary>
	/// Launcher controller simply create the missiles in the scene. It uses the parameters from other controllers
	/// (such as GameController or PlayerController) to increase the speed of missile instantiation and thus increasing 
	/// the game's difficulty.
	/// Notice that missiles that are being instantiated have their own controllers (AI) and we do not set their
	/// target, movement or anything here.
	/// </summary>

	public GameObject missile;							//Missile object we need to instantiate
	public AudioClip shootSfx;							//instantiation sfx
	private bool canCreateMissile;						//flag
	private float missileCreationBaseDelay = 0.75f;		//default delay
	private float missileCreationDelay;					//actual delay 
	private float rotationZ;
    public GameObject Player; 
        //the amount of rotation for this launcher pad
    
	/// <summary>
	/// Init
	/// </summary>
	void Start () {
		missileCreationDelay = 0;
		canCreateMissile = false;
		rotationZ = 0;
		StartCoroutine(activeMissileCreation ());
       
    }

    // Gets called when the player opens the notification.
   

    /// <summary>
    /// FSM
    /// </summary>
    void Update () {

		//create a new missile whenever possible.
		if(canCreateMissile && !GameController.isGameFinished && GameController.isGameStarted)
			createMissile ();
		//rotate the laucher pad's body and also sync the direction with the player.
		rotationZ += (Time.deltaTime + PlayerController.dir) * -1f * (1 + (float)GameController.level / 5);
		transform.rotation = Quaternion.Euler (0, 0, rotationZ );
	}


	/// <summary>
	/// Create new missile.
	/// </summary>
	void createMissile() {
		canCreateMissile = false;
		GameObject m = Instantiate (missile, transform.position,Quaternion.identity)as GameObject;
        m.transform.Rotate(Player.transform.forward);
        //m.transform.Rotate(transform.right);
        m.name = "Missile";
		playSfx (shootSfx);
		StartCoroutine(activeMissileCreation ());
	}


	/// <summary>
	/// Active missile creation after a small delay.
	/// </summary>
	IEnumerator activeMissileCreation() {

		//we use this to create more missiles when player advance to higher levels
		missileCreationDelay = missileCreationBaseDelay - ((float)GameController.level / 30);

		//limit the max number of missiles
		if (missileCreationDelay < 1f)
			missileCreationDelay = 1f;
		
		//print ("missileCreationDelay: " + missileCreationDelay);
		//print ("GameController.level: " + GameController.level);

		yield return new WaitForSeconds (missileCreationDelay);
		canCreateMissile = true;
	}


	/// <summary>
	/// Plays the given audio
	/// </summary>
	void playSfx ( AudioClip _clip  ){
		GetComponent<AudioSource>().PlayOneShot(_clip);
	}

}
