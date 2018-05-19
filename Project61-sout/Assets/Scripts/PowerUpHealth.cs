using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class PowerUpHealth : NetworkBehaviour {

	[SerializeField] private float multiplierSpeed = 1.4f;
	[SerializeField] private float duration = 4f;
	[SerializeField] private int cost = 300;


	void Update()
	{
		gameObject.GetComponent<Rigidbody>().MoveRotation(gameObject.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(0f,0f,1f)));
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.GetComponent<Player>().money >= cost)
		{
			StartCoroutine(Pickup(other));
		}
	}

	IEnumerator Pickup(Collider player)
	{
		Player stats = player.GetComponent<Player>();
		stats.maxHealth = Convert.ToInt32(stats.maxHealth * multiplierSpeed);
		stats.money -= cost;
		
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;

		yield return new WaitForSeconds(duration);

		stats.maxHealth = Convert.ToInt32(stats.maxHealth / multiplierSpeed);

		Destroy(gameObject);
	}
}
