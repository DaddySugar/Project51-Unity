	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Security.Principal;
//	using UnityEditor.PackageManager;
	using UnityEngine;
	using UnityEngine.AI;
	using UnityEngine.Networking;
	using Random = UnityEngine.Random;

public class ChaseAlien : NetworkBehaviour
	{
		private GameObject goal; 
		//public GameObject player;
		public Transform targetTransform; //the enemy's target
		private NavMeshAgent agent;
		private Animator anim;
		private Transform AlienPosition;
		private float radius = 100;
		private LayerMask raycastLayer;
		private Vector3 pos1;
		private Vector3 pos2;
		private Vector3 fuckYouGame;
		private bool hasLostTrack = false;
		private float timeToResetDest = 2f;
		//public GameObject clientnetwoek;
		

		
		
		void Start ()
		{
			agent = GetComponent<NavMeshAgent>();
			anim = GetComponent<Animator>();
			AlienPosition = GetComponent<Transform>();
			raycastLayer = 2 << LayerMask.NameToLayer("LocalPlayer");
			
		}
		
		void Update () 
		{
			
			agent.destination = targetTransform.transform.position;
			if (NetworkManager.singleton == null)
			{
				Debug.Log("nt manager");
			}
			else if (NetworkManager.singleton.client == null)
			{
				Debug.Log("client");
			}
			else if(NetworkManager.singleton.client.connection == null)
			{
				Debug.Log("cnx");
			}
			else if (NetworkManager.singleton.client.connection.playerControllers == null)
			{
				Debug.Log("player ctr");
			}
			
			if (NetworkManager.singleton.client == null)
				return; 
			else if (NetworkManager.singleton.client.connection.playerControllers.Count == 1)
			{
				if (hasLostTrack)
				{

					var temp = (NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position -
					            AlienPosition.position);
					var temp2 = temp.y;
					temp = temp.normalized * 40;
					temp.y = temp2 - NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position.y;

					agent.destination = temp + AlienPosition.position;
					if ((NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position -
					     AlienPosition.position).magnitude < 45)
					{
						hasLostTrack = false;
						timeToResetDest = Time.time + 1 / timeToResetDest;
					}
				}
				else
				{
					if (!agent.hasPath && Time.time > timeToResetDest) 
					{
						hasLostTrack = true;
					}
					else
					{
						hasLostTrack = false;
					
					}
					fuckYouGame = NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position -
					              AlienPosition.position;
					agent.destination =  NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position;
				}
				
			}
			else if (NetworkManager.singleton.client.connection.playerControllers.Count == 2)
			{
				if (hasLostTrack)
				{
					pos1 = NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position;
					pos2 = NetworkManager.singleton.client.connection.playerControllers[1].gameObject.transform.position;
					if ((AlienPosition.position - pos1).magnitude > (AlienPosition.position - pos2).magnitude)
					{
						pos2 = pos1;
					}
					
					var temp = (pos1 - AlienPosition.position);
					var temp2 = temp.y;
					temp = temp.normalized * 40;
					temp.y = temp2 - NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position.y;

					agent.destination = temp + AlienPosition.position;
					if ((NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position -
					     AlienPosition.position).magnitude < 45)
					{
						hasLostTrack = false;
						timeToResetDest = Time.time + 1 / timeToResetDest;
					}
				}
				else
				{
					if (!agent.hasPath && Time.time > timeToResetDest) 
					{
						hasLostTrack = true;
					}
					else
					{
						hasLostTrack = false;
					
					}
					pos1 = NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position;
					pos2 = NetworkManager.singleton.client.connection.playerControllers[1].gameObject.transform.position;

					if ((AlienPosition.position - pos1).magnitude < (AlienPosition.position - pos2).magnitude)
					{
						agent.destination = pos1;
						fuckYouGame = pos1 - AlienPosition.position;
					}
					else
					{
						agent.destination = pos2;
						fuckYouGame = pos2 - AlienPosition.position;
					}
				}
			}
			
			if (fuckYouGame.magnitude < 4)
			{
				anim.SetBool("isAttacking", true);
			}
			else
			{
				anim.SetBool("isAttacking", false);
			}
			
	
			
		}
	
		void SearchForTarget()
		{
			if(!isServer)	
				return;
	
			if (targetTransform == null)
			{
				Collider[] hitColliders = Physics.OverlapSphere(AlienPosition.position, radius, raycastLayer);
	
				if (hitColliders.Length > 0)
				{
					int randomint = Random.Range(0, hitColliders.Length);
					targetTransform = hitColliders[randomint].transform; 
				}
			}
			
		}
		
	}
