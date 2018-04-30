using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

	//public Slider healthbar; 
	//public HealthBar playerhealthbar = new HealthBar(); 
	//public RectTransform healthBar;
	public Image LocalHealthBarImg;
	
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
		
		LocalHealthBarImg.fillAmount = GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;
		//healthBar.sizeDelta = new Vector2( currentHealth, healthBar.sizeDelta.x);
	}

	public void SetDefaults()
	{
		isDead = false;
		currentHealth = maxHealth;
		LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;
		money = 500;
		
		
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

		
		Debug.Log(transform.name + " " + currentHealth);
		
		if (currentHealth <= 1)
		{
			Die();
		}

		//playerhealthbar.SetHealthAmount(OnChangeHealthBar());
		LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;
		//healthBar.sizeDelta = new Vector2( currentHealth, healthBar.sizeDelta.x);

	}

	private void Die()
	{
		isDead = true;
		
		// disable comp 
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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
	}

	private void Respawn()
	{
		SetDefaults();
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		
		Transform _spawPosition = NetworkManager.singleton.GetStartPosition();
		Debug.Log(_spawPosition);
		transform.position = _spawPosition.position;
		transform.rotation = _spawPosition.rotation;
		hasPlayedDyingAnimation = false;
		hasFinishedDyingTime = 4f;
		Debug.Log(transform.name + "  player respawn");
		_animation.SetBool("die", false);

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
			LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;

		}
		else if (precedentMaxHealth > maxHealth)
		{
			if (currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
			LocalHealthBarImg.fillAmount = (float) GetComponent<Player> ().currentHealth / GetComponent<Player> ().maxHealth;


		}
		precedentMaxHealth = maxHealth;
		
		if (hasPlayedDyingAnimation && Time.time > hasFinishedDyingTime)
		{
			Respawn();
		}
		//set layers
		if (GameManager.players.Count == 1)
		{
			string playerId = "";
			foreach (var playerKey in GameManager.players.Keys)
			{
				playerId = playerKey;
			}
			GameObject obj = GameManager.players[playerId].gameObject;
			//Camera cam = gameObject.GetComponentInChildren<Camera>();
			//cam.cullingMask = maskPlayer1;


		}
		
		else if (GameManager.players.Count == 2)
		{
			List<string> playerIds = new List<string>();
			foreach (var playerKey in GameManager.players.Keys)
			{
				playerIds.Add(playerKey);
			}
			GameObject obj = GameManager.players[playerIds[0]].transform.parent.gameObject;
			Camera cam1 = GameManager.players[playerIds[0]].GetComponentInParent<GameObject>().GetComponentInChildren<Camera>();
			cam1.cullingMask = maskPlayer1;
			Camera cam2 = GameManager.players[playerIds[1]].GetComponentInParent<GameObject>().GetComponentInChildren<Camera>();
			cam2.cullingMask = maskPlayer2;
			
		}
		

		
	}

}
