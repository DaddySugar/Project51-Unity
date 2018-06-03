using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameOverManager : NetworkBehaviour
{
	public Cannon Canon;                    // Reference to the player's health.
	public float restartDelay = 5f;         // Time to wait before restarting the level

	public PlayerUI[] players;
	//public List<PlayerSetup> sett; 

	public GameObject[] AllAliens; 
	

	Animator anim;                          // Reference to the animator component.
	float restartTimer;                     // Timer to count up to restarting the level


	void Awake ()
	{
		// Set up the reference.
		anim = GetComponent <Animator> ();
		//players = GameManager.GetAllPlayers();
		/*sett = new List<PlayerSetup>();
		foreach (var VARIABLE in players)
		{
			sett.Add(VARIABLE.GetComponent<PlayerSetup>());
		}*/
		
	}


	void Update ()
	{
		// If the player has run out of health...
		if(Canon.currentpart == 2)
		{
			Disable();
		}
	}

	private void Disable()
	{
		// ... tell the animator the game is over.
		anim.SetTrigger ("GameOver");
		players = FindObjectsOfType<PlayerUI>();
		
		
		foreach (var VARIABLE in players)
		{
			VARIABLE.DisableUI();
			Debug.Log("1");
		}

		AllAliens = GameObject.FindGameObjectsWithTag("Alien");
		foreach (var VARIABLE in AllAliens)
		{
			Destroy(GameObject.FindWithTag("Alien"));
		}

		// .. increment a timer to count up to restarting.
		restartTimer += Time.deltaTime;

		// .. if it reaches the restart delay...
		if(restartTimer >= restartDelay)
		{
			// .. then reload the currently loaded level.
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}