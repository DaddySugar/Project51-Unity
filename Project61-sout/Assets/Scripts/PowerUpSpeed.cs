using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{

	[SerializeField] private float multiplierSpeed = 1.4f;
	[SerializeField] private float duration = 4f;

	void Update()
	{
		var temp = gameObject.transform.rotation.y;
		temp += 10f;
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
		PlayerController stats = player.GetComponent<PlayerController>();
		stats.speed *= multiplierSpeed;

		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
		
		yield return new WaitForSeconds(duration);

		stats.speed /= multiplierSpeed;
		
		Destroy(gameObject);
	}
}
