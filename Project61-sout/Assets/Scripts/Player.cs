using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

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
	
	
	
	[SerializeField] private int maxHealth = 100;
	
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
	
	
	[ClientRpc]
	public void RpcTakeDamage(int ammount)
	{
		if (isDead)
		{
				return;
		}
		currentHealth -= ammount;
		Debug.Log(transform.name + currentHealth);
		if (currentHealth <= 0)
		{
			Die();
		}
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
		
		//	call respawn 
	}
	
}
