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
		switch (currentpart)
		{
				case 1:
					anim.SetBool("shoot", true);
					break;
				
				case 2:
					anim.SetBool("shoot 2", true);
					break;
					
				case 3:
					anim.SetBool("shoot 3", true);
					break;
					
				case 4:
					anim.SetBool("shoot 4", true);
					break;
					
				case 5:
					anim.SetBool("shoot 5", true);
					break;

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
		currentpart = currPart;
	}

	[Client]
	void RpcAddParts2(int currPart)
	{
		currentpart = currPart;
	}

	[Command]
	void CmdAddParts(int currPart)
	{
		currentpart = currPart;


		//objNetId = GetComponent<NetworkIdentity> ();  
		//objNetId.AssignClientAuthority (connectionToClient);
		RpcAddParts(currPart);
		//objNetId.RemoveClientAuthority (connectionToClient);
	}
	
}
