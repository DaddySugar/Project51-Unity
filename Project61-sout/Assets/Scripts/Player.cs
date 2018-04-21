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
	
	[SerializeField] private int maxHealth = 100;
	
	[SyncVar]
	private int currentHealth;

	private void Awake()
	{
		SetDefaults();
	}

	public void SetDefaults()
	{
		currentHealth = maxHealth;
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
		
		
		//	call respawn 
	}
	
}
