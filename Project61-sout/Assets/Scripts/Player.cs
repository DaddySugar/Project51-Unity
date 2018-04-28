using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

	//public Slider healthbar; 
	public HealthBar playerhealthbar = new HealthBar(); 
	
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


	public void Setup()
	{
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		SetDefaults();
		
		
	}

	public void SetDefaults()
	{
		isDead = false;
		currentHealth = maxHealth;

		
		
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

    public float OnChangeHealthBar()
    {
        return  currentHealth / maxHealth;
	    
    }

    [ClientRpc]
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

		playerhealthbar.SetHealthAmount(OnChangeHealthBar());

	}

	private void Die()
	{
		isDead = true;
		
		// disable comp 
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}
		
		//Disable the collider
		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;
		
		Debug.Log(transform.name + "dead");
		
		//	call respawn 
		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn()
	{
		// respawn time set to 3 seconds look in match settings and game manager
		yield return new WaitForSeconds(GameManager.instance._MatchSettings.respawnTime);
		SetDefaults();
		Transform _spawPosition = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawPosition.position;
		transform.rotation = _spawPosition.rotation;
		Debug.Log(transform.name + "  player respawn");
	}


	void Update()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.K))
		{
			RpcTakeDamage(10);
		}
		
		
		
	}

}
