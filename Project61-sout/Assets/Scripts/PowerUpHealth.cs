using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpHealth : MonoBehaviour {

	[SerializeField] private float multiplierSpeed = 1.4f;
	[SerializeField] private float duration = 4f;

	void Update()
	{
		var temp = gameObject.transform.rotation.y;
		temp += 1f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine(Pickup(other));
		}
	}

	IEnumerator Pickup(Collider player)
	{
		Player stats = player.GetComponent<Player>();
		stats.maxHealth = Convert.ToInt32(stats.maxHealth * multiplierSpeed);
		
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;

		yield return new WaitForSeconds(duration);

		stats.maxHealth = Convert.ToInt32(stats.maxHealth / multiplierSpeed);

		Destroy(gameObject);
	}
}
