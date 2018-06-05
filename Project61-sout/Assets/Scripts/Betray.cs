using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Betray : MonoBehaviour {

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !other.GetComponent<Player>().hasBetrayed)
		{
			StartCoroutine(Betraybis(other));
		}
	}
	
	IEnumerator Betraybis(Collider player)
	{
		player.GetComponent<Player>().CmdsetBetray();
		GameObject alie = gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject;
		alie.GetComponent<MeshRenderer>().material.color = Color.red;
		yield return new WaitForSeconds(2f);
		
		alie.GetComponent<MeshRenderer>().material.color = Color.white;

		
		
	}
}
