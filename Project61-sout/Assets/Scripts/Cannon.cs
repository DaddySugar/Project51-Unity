using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Cannon : NetworkBehaviour {

	[SerializeField] private int cost = 5000;
	private int maxParts = 5;
	[SyncVar]
	public int currentpart;

	private void Start()
	{
		currentpart = 0; 
	}

	void Update()
	{
		
	}

	

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.GetComponent<Player>().money >= cost && currentpart < maxParts)
		{
			StartCoroutine(Pickup(other));
		}
	}
	

	IEnumerator Pickup(Collider player)
	{
		Player stats = player.GetComponent<Player>();
		currentpart++;
		stats.money -= cost;
		
		yield return new WaitForSeconds(2);
	}
}
