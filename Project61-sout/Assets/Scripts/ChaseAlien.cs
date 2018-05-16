	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Security.Principal;
	using UnityEditor;
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

		public int alienDamage = 5;
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
		private float timeToPass = 0f;
		float coeff = 1f;
		public float timeBetweenChanges = 0.4f;

		private float timeToNextHit = 2f;
		private float timeToPass2 = 0f;


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

			if (GameManager.players.Count == 1)
			{
				string playerId = "";
				foreach (var playerKey in GameManager.players.Keys)
				{
					playerId = playerKey;
				}
				pos1 = GameManager.players[playerId].GetComponentInParent<Transform>().position;
				//
				if (hasLostTrack)
				{
					var temp = (pos1 - AlienPosition.position);
					var temp2 = temp.y;
					temp = temp.normalized * 40 * coeff;
					temp.y = temp2 - pos1.y;
					agent.destination = temp + AlienPosition.position;
					if (!agent.hasPath && Time.time > timeToPass2)
					{
						coeff -= 0.15f;
						timeToPass2 = Time.time + timeBetweenChanges;
						if (coeff <= 0)
						{
							coeff = 1;
						}
					}
					
					if ((pos1 - AlienPosition.position).magnitude < 40 && agent.hasPath && Time.time > timeToPass2 + 3f)
					{
						coeff = 1;
						hasLostTrack = false;
					}
					if ((pos1 - AlienPosition.position).magnitude < 10)
					{
						coeff = 1;
						hasLostTrack = false;
					}
				}
				else
				{
					if (!agent.hasPath) 
					{
						hasLostTrack = true;
					}
					else
					{
						hasLostTrack = false;
					}
					fuckYouGame = pos1 -AlienPosition.position;
					agent.destination =  pos1;
				}
				if (fuckYouGame.magnitude < 2 && Time.time > timeToNextHit)
				{
					GameManager.players[playerId].GetComponentInParent<Player>().RpcTakeDamage(alienDamage);
					timeToNextHit = Time.time + 2f;
				}
				
			}
			else if (GameManager.players.Count == 2)
			{
				List<string> playerIds = new List<string>();
				foreach (var playerKey in GameManager.players.Keys)
				{
					playerIds.Add(playerKey);
				}
				pos1 = GameManager.players[playerIds[0]].GetComponentInParent<Transform>().position;
				
				if (playerIds.Count > 1)
				{
						pos2 = GameManager.players[playerIds[1]].GetComponentInParent<Transform>().position;

				}
				if ((AlienPosition.position - pos1).magnitude > (AlienPosition.position - pos2).magnitude)
				{
					pos1 = pos2;
				}

				if (hasLostTrack)
				{
					var temp = (pos1 - AlienPosition.position);
					var temp2 = temp.y;
					temp = temp.normalized * 40 * coeff;
					temp.y = temp2 - pos1.y;
					agent.destination = temp + AlienPosition.position;
					if (!agent.hasPath && Time.time > timeToPass2)
					{
						coeff -= 0.15f;
						timeToPass2 = Time.time + timeBetweenChanges;
						if (coeff <= 0)
						{
							coeff = 1;
						}
					}
					
					if ((pos1 - AlienPosition.position).magnitude < 40 && agent.hasPath && Time.time > timeToPass2 + 3f)
					{
						coeff = 1;
						hasLostTrack = false;
					}
					if ((pos1 - AlienPosition.position).magnitude < 10)
					{
						coeff = 1;
						hasLostTrack = false;
					}
				}
				else
				{
					if (!agent.hasPath && Time.time > timeToPass) 
					{
						hasLostTrack = true;
					}
					else
					{
						hasLostTrack = false;
					
					}
					agent.destination = pos1;
					fuckYouGame = pos1 - AlienPosition.position;
					
				}
				if (fuckYouGame.magnitude < 2 && Time.time > timeToNextHit)
				{
					pos1 = GameManager.players[playerIds[0]].GetComponentInParent<Transform>().position;
				
					if (playerIds.Count > 1)
					{
						pos2 = GameManager.players[playerIds[1]].GetComponentInParent<Transform>().position;

					}

					if ((AlienPosition.position - pos1).magnitude < (AlienPosition.position - pos2).magnitude)
					{
						GameManager.players[playerIds[0]].GetComponentInParent<Player>().RpcTakeDamage(alienDamage);
					}
					else
					{
						GameManager.players[playerIds[1]].GetComponentInParent<Player>().RpcTakeDamage(alienDamage);
					}
					timeToNextHit = Time.time + 2f;
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
