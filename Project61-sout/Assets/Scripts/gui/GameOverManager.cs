﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverManager : NetworkBehaviour
{
	
	
	
	#region FirstImportant

	public SpawnManager_ZombieSpawner wave;
	public Cannon Canon;                    // Reference to the player's health.
	public float restartDelay = 5f;         // Time to wait before restarting the level

	public PlayerUI[] playersUI;

	public Player[] players = GameManager.GetAllPlayers();
	//public List<PlayerSetup> sett; 

	public GameObject[] AllAliens; 
	

	Animator anim;                          // Reference to the animator component.
	float restartTimer;                     // Timer to count up to restarting the level


	private const string levelToLoad = "MainMenu_";

	private bool finished = false;
	
	

	void Awake ()
	{
		// Set up the reference
		anim = GetComponent <Animator> ();
		wave = FindObjectOfType<SpawnManager_ZombieSpawner>();
		//players = GameManager.GetAllPlayers();
		/*sett = new List<PlayerSetup>();
		foreach (var VARIABLE in players)
		{
			sett.Add(VARIABLE.GetComponent<PlayerSetup>());
		}*/

	}

	bool CheckDeaths()
	{
		players = GameManager.GetAllPlayers();
		int a = 0;
		foreach (var VARIABLE in players)
		{
			a += VARIABLE.deaths;
			//Debug.Log(a);
		}

		if (a == 5)
		{
			return true;
		}

		return false;
	}


	void Update ()
	{
		// If the player has run out of health...
		if(Canon.currentpart == 5 && !finished)
		{
			Disable(true, 0);
		}
		
		if (CheckDeaths())
		{
			Disable(false,0);
		}
	}

	private void Disable(bool _IsWin, int _index)
	{
		// ... tell the animator the game is over.
		//anim.SetTrigger ("GameOver");
		
		if (_IsWin)
		{
			anim.SetBool("WonTheGame", true);
		}
		else
		{
			anim.SetBool("LostTheGame", true);
		}

		foreach (var VARIABLE in players)
		{
			VARIABLE.RpcDisableMouvements();
		}
		
		
		playersUI = FindObjectsOfType<PlayerUI>();
		
		
		foreach (var VARIABLE in playersUI)
		{
			VARIABLE.DisableUI();
			//Debug.Log("1");
		}

		AllAliens = GameObject.FindGameObjectsWithTag("Alien");
		foreach (var VARIABLE in AllAliens)
		{
			Destroy(GameObject.FindWithTag("Alien"));
		}

		wave.IsSpawnActivated = false; 
	

		// .. increment a timer to count up to restarting.
		restartTimer += Time.deltaTime;

		if (!finished)
		{
			finished = true; 
			StartCoroutine(Countdown(_index));
		}
		

		// .. if it reaches the restart delay...
		//
			// .. then reload the currently loaded level.
			//Application.LoadLevel(Application.loadedLevel);
		//}
		
		
	}

	IEnumerator Countdown(int _index)
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
		
		int countdown = 10;
		while (countdown > 0)
		{
			//status.text = "JOINING... (" + countdown + ")";

			yield return new WaitForSeconds(1);

			countdown--;
		}
		
		
	}
	
	#endregion
}