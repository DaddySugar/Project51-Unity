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

    public GameObject PartSound;
	public Animator anim;
	
	//Animator anim;                          // Reference to the animator component.
	//float restartTimer;     

	
	private void Start()
	{
		currentpart = 0;
		//anim = GetComponent <Animator> ();
	}

	private void Update()
	{
		if (currentpart == 1)
		{
			anim.SetBool("shoot", true);
		}
	}


	private void OnTriggerStay(Collider other)
	{
		
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.GetComponent<Player>().money >= cost && currentpart < maxParts && Time.time > timeToWait)
		{
            GameObject partsound = Instantiate(PartSound, this.transform.position, this.transform.rotation) as GameObject;
            Pickup(other);
        }
	}

	public float Getpst()
	{
		return (float) currentpart / maxParts;
	}
	

	private void Pickup(Collider player)
	{
		currentpart++;
		
		Player stats = player.GetComponent<Player>();

		Debug.Log(" player.isClient " + stats.isClient + " player.isServer " + stats.isServer + " player.isLocalPlayer " + stats.isLocalPlayer);
			

		

		if (!stats.isServer)
		{
			RpcAddParts2(currentpart);
		}
		else
		{
			RpcAddParts(currentpart);
		}
		
		
		
		
			
		
		
		timeToWait = Time.time + 0.1f;
		stats.money -= cost;
		
	}

	[ClientRpc]
	void RpcAddParts(int currPart)
	{
		Debug.Log("rPC currentpart " + currentpart);
		currentpart = currPart;
		Debug.Log("rpC currentpart " + currentpart);

	}

	[Client]
	void RpcAddParts2(int currPart)
	{
		Debug.Log("rPC currentpart2 " + currentpart);
		currentpart = currPart;
		Debug.Log("rpC currentpart2 " + currentpart);
	}

	[Command]
	void CmdAddParts(int currPart)
	{
		Debug.Log("Cmd currentpart " + currentpart);
		currentpart = currPart;

		Debug.Log("Cmd currentpart " + currentpart);

		//objNetId = GetComponent<NetworkIdentity> ();  
		//objNetId.AssignClientAuthority (connectionToClient);
		RpcAddParts(currPart);
		//objNetId.RemoveClientAuthority (connectionToClient);
	}
	
}
