using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Betray : MonoBehaviour {

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
		{
			Betraybis(other);
		}
	}
	
	private void Betraybis(Collider player)
	{
		player.GetComponent<Player>().RpcsetBetray();
	}
}
