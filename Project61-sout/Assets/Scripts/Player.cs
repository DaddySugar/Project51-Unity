using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

	//public Slider healthbar; 
	//public HealthBar playerhealthbar = new HealthBar(); 
	//public RectTransform healthBar;
	//public Image LocalHealthBarImg;
	
	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		
		get { return _isDead; } 
		protected set { _isDead = value; }
	}
	
	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
    //private float health;
    
	public int maxHealth = 100;

    [SyncVar]
	private int currentHealth;

	private int precedentMaxHealth = 100;
	private float hasFinishedDyingTime= 4f;
	private bool hasPlayedDyingAnimation = false;
	private Animator _animation;
	public int money;
	public int moneyRewardedByKill = 25;
	public LayerMask maskPlayer1;
	public LayerMask maskPlayer2;
	public LayerMask maskDie;
	private LayerMask formerMask;
	private int formerNumberofPlayer = 0;
	private Vector3 formerCameraPosition;
	private Quaternion formerCameraRotation;
	[SyncVar] public bool hasBetrayed = false;

	[SyncVar]
	public int kills;
	[SyncVar]
	public int deaths;

	public int Deaths
	{
		get { return deaths; }
	}
	
	[SyncVar]
	public string username = "Loading...";
	

	void Start()
	{
		_animation = GetComponent<Animator>();

	}

	public void Setup()
	{
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		SetDefaults();
		
		//LocalHealthBarImg.fillAmount = GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;
		//healthBar.sizeDelta = new Vector2( currentHealth, healthBar.sizeDelta.x);
	}

	public void SetDefaults()
	{
		isDead = false;
		currentHealth = maxHealth;
		//LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;
		money = 1000;
		
		
		//Enable the components
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}
		
		//Enable the collider
		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;	
	}


    [Client]
	public void RpcTakeDamage(int ammount)
	{
		if (_isDead)
		{
				return;
		}
		currentHealth -= ammount;

		if (currentHealth <= 1)
		{
			//gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			RpcDie();
		}

		//playerhealthbar.SetHealthAmount(OnChangeHealthBar());
		//LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;
		//healthBar.sizeDelta = new Vector2( currentHealth, healthBar.sizeDelta.x);

	}
	[Client]
	private void RpcDie()
	{
		isDead = true;
		//camera goes up and ignore the gun of its player
		GameObject cam = gameObject.transform.Find("Camera").gameObject;
		SetLayerRecursively(cam.transform.Find("m4_fp").gameObject, 1);
		formerCameraPosition = cam.GetComponent<Transform>().position;
		formerCameraRotation = cam.GetComponent<Transform>().rotation;
		cam.GetComponent<Transform>().position += new Vector3(0, 1f, -2f);
		cam.GetComponent<Transform>().rotation = Quaternion.Euler(40,0,0);
		formerMask = cam.GetComponent<Camera>().cullingMask;
		cam.GetComponent<Camera>().cullingMask = maskDie;
		
		// disable comp 
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}
		
		//Disable the collider
		/*Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;*/
		Debug.Log(transform.name + "dead");
		_animation.SetBool("die", true);
		
		hasPlayedDyingAnimation = true;
		hasFinishedDyingTime = Time.time + hasFinishedDyingTime;
		deaths++;
	}

	[Client]
	private void RpcRespawn()
	{
		
		SetDefaults();
		//restore camera place
		GameObject cam = gameObject.transform.Find("Camera").gameObject;
		cam.GetComponent<Transform>().position = formerCameraPosition;
		cam.GetComponent<Transform>().rotation = formerCameraRotation;
		SetLayerRecursively(cam.transform.Find("m4_fp").gameObject, 0);
		cam.GetComponent<Camera>().cullingMask = formerMask;
		
		Transform _spawPosition = NetworkManager.singleton.GetStartPosition();
		Debug.Log(_spawPosition);
		transform.position = _spawPosition.position;
		transform.rotation = _spawPosition.rotation;
		hasPlayedDyingAnimation = false;
		hasFinishedDyingTime = 4f;
		Debug.Log(transform.name + "  player respawn");
		_animation.SetBool("die", false);

	}
	
	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}
       
		obj.layer = newLayer;
       
		foreach (Transform child in obj.transform)
		{
			if (null == child)
			{
				continue;
			}
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	

	void Update()
	{
		
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.K))
		{
			RpcTakeDamage(10);
		}

		if (precedentMaxHealth < maxHealth)
		{
			currentHealth = maxHealth;
			//LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;

		}
		else if (precedentMaxHealth > maxHealth)
		{
			if (currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
			//LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;


		}
		precedentMaxHealth = maxHealth;
		
		if (hasPlayedDyingAnimation && Time.time > hasFinishedDyingTime)
		{
			RpcRespawn();
		}
		//set layers
		if (GameManager.players.Count == 1 && GameManager.players.Count != formerNumberofPlayer)
		{
			string playerId = "";
			foreach (var playerKey in GameManager.players.Keys)
			{
				playerId = playerKey;
			}
			GameObject obj = GameManager.players[playerId].gameObject;
			obj.GetComponentInChildren<Camera>().cullingMask = maskPlayer1;
			SetLayerRecursively(obj.transform.Find("Graphics").gameObject, 10);
			formerNumberofPlayer = 1;

		}
		
		else if (GameManager.players.Count == 2 && GameManager.players.Count != formerNumberofPlayer)
		{
			List<string> playerIds = new List<string>();
			foreach (var playerKey in GameManager.players.Keys)
			{
				playerIds.Add(playerKey);
			}
			GameObject objPlayer1 = GameManager.players[playerIds[0]].gameObject;
			GameObject objPlayer2 = GameManager.players[playerIds[1]].gameObject;
			objPlayer1.GetComponentInChildren<Camera>().cullingMask = maskPlayer1;
			objPlayer2.GetComponentInChildren<Camera>().cullingMask = maskPlayer2;
			SetLayerRecursively(objPlayer1.transform.Find("Graphics").gameObject, 10);
			SetLayerRecursively(objPlayer2.transform.Find("Graphics").gameObject, 11);
			formerNumberofPlayer = 2;

		}
		
	}

	[Client]
	public void RpcsetBetray()
	{
		hasBetrayed = true;
	}
	
	public float GetHealthpct()
	{
		return (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;
	}

}
