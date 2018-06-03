using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Cannon : NetworkBehaviour {

	[SerializeField] private int cost = 500;
	private const int maxParts = 5;
	
	[SyncVar]
	public  int currentpart;

	private float timeToWait = 0f;
	
	private NetworkIdentity objNetId;
	public GameObject objectID; 

	
	private void Start()
	{
		currentpart = 0;
	}


	private void OnTriggerStay(Collider other)
	{
		
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.GetComponent<Player>().money >= cost && currentpart < maxParts && Time.time > timeToWait)
		{
			Pickup(other);
			
		}
	}

	public float Getpst()
	{
		return (float) currentpart / maxParts;
	}
	

	private void Pickup(Collider player)
	{
		if (!isServer)
		{
			Debug.Log("server" +
			          "");
			CmdAddParts();
		}
		else
		{
			currentpart++;
		}
			
		Player stats = player.GetComponent<Player>();
		//currentpart++;
		
		timeToWait = Time.time + 0.1f;
		stats.money -= cost;
		
	}

	[ClientRpc]
	void RpcAddParts()
	{
		currentpart++;
	}

	[Command]
	void CmdAddParts()
	{
		//objNetId = GetComponent<NetworkIdentity> ();  
		//objNetId.AssignClientAuthority (connectionToClient);
		RpcAddParts();
		//objNetId.RemoveClientAuthority (connectionToClient);
	}
	
}
