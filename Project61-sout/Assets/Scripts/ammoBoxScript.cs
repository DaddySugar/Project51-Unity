using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBoxScript : MonoBehaviour {

	[SerializeField] private int cost = 100;

	[SerializeField] private int ammoBought = 60;

	private float timeToWaitToBuy = 0f;
	// Use this for initialization
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.GetComponent<Player>().money >= cost && Time.time > timeToWaitToBuy && other.GetComponent<PlayerShoot>().Weapon.BulletsTotal < other.GetComponent<PlayerShoot>().Weapon.maxBulletsTotal)
		{
			Pickup(other);
		}
	}
	
	private void Pickup(Collider player)
	{
		PlayerShoot stats = player.GetComponent<PlayerShoot>();
		stats.Weapon.BulletsTotal = stats.Weapon.BulletsTotal + ammoBought;
		if (stats.Weapon.BulletsTotal > stats.Weapon.maxBulletsTotal)
		{
			stats.Weapon.BulletsTotal = stats.Weapon.maxBulletsTotal;
		}
		player.GetComponent<Player>().money -= cost;
		timeToWaitToBuy = Time.time + 0.1f;
	}
	
}
