using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

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

	public void TakeDamage(int ammount)
	{
		currentHealth -= ammount;
		Debug.Log(transform.name + currentHealth);
	}
}
