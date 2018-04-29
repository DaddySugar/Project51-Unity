using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpStrenght : MonoBehaviour {

	[SerializeField] private float multiplierSpeed = 1.4f;
	[SerializeField] private float duration = 4f;
	[SerializeField] private int cost = 300;

	void Update()
	{
		var temp = gameObject.transform.rotation.y;
		temp += 1f;
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
		PlayerShoot stats = player.GetComponent<PlayerShoot>();
		stats.Weapon.damage = Convert.ToInt32(stats.Weapon.damage * multiplierSpeed);
		player.GetComponent<Player>().money -= cost;

		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;

		yield return new WaitForSeconds(duration);

		stats.Weapon.damage = Convert.ToInt32(stats.Weapon.damage / multiplierSpeed);

		Destroy(gameObject);
	}
}
