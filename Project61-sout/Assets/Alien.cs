using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Alien : NetworkBehaviour {

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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
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
	}
	
	private void Die()
	{
		isDead = true;
		
		// disable comp 
		/*for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}*/
		
		//Disable the collider
		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;
		
		Debug.Log(transform.name + "dead");
		
		//	call respawn 
		//StartCoroutine(Respawn());
	}
}
