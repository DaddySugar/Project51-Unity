using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{

	[SerializeField] private float multiplierSpeed = 1.4f;
	[SerializeField] private float duration = 4f;
	[SerializeField] private int cost = 300;


	void Update()
	{
		var temp = gameObject.transform.rotation.y;
		temp += 10f;
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
		PlayerController stats = player.GetComponent<PlayerController>();
		stats.speed *= multiplierSpeed;
		stats.sprintSpeed *= multiplierSpeed;
		player.GetComponent<Player>().money -= cost;

		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
		
		yield return new WaitForSeconds(duration);

		stats.speed /= multiplierSpeed;
		stats.sprintSpeed /= multiplierSpeed;
		
		Destroy(gameObject);
	}
}
